using AutoMapper;
using CashFlow.Application.UseCases.Users.Validator;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IPasswordEncripter passwordEncripter,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _passwordEncripter = passwordEncripter;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();
        Validate(request, loggedUser);

        var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var passwordMatch = _passwordEncripter.Verify(request.Password, loggedUser.Password);

        if (!passwordMatch)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, "Invalid password."));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
