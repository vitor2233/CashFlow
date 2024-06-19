using AutoMapper;
using CashFlow.Application.UseCases.Users.Validator;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IAccessTokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserUseCase(IMapper mapper, IPasswordEncripter passwordEncripter,
        IUserReadOnlyRepository userReadOnlyRepository, IUserWriteOnlyRepository userWriteOnlyRepository,
        IUnitOfWork unitOfWork, IAccessTokenGenerator tokenGenerator)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _userWriteOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailExists = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, "Email já está sendo utilizado"));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
