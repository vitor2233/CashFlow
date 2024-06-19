
using System.Net;

namespace CashFlow.Exception.ExceptionBase;

public class InvalidLoginException : CashFlowException
{
    public InvalidLoginException() : base("Email e/ou senha inválidos")
    { }
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return new List<string>() { Message };
    }
}