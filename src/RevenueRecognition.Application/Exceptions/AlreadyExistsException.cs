namespace RevenueRecognition.Application.Exceptions;

public class AlreadyExistsException : ApplicationException
{
    public AlreadyExistsException(string msg) : base(msg, 409){} 
}