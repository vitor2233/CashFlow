
using CashFlow.Application.UseCases.Login.Validator;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Login;

internal class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _tokenGenerator;

    public DoLoginUseCase(IUserReadOnlyRepository userReadOnlyRepository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncripter = passwordEncripter;
        _tokenGenerator = tokenGenerator;
    }
    public async Task<ResponseLoginJson> Execute(RequestLoginJson request)
    {
        Validate(request);

        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email)
            ?? throw new InvalidLoginException();

        var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);
        if (!passwordMatch)
        {
            throw new InvalidLoginException();
        }

        return new ResponseLoginJson
        {
            Token = _tokenGenerator.Generate(user),
        };
    }

    private void Validate(RequestLoginJson request)
    {
        var result = new DoLoginValidator().Validate(request);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
