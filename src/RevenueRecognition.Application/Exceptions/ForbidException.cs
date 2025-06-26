namespace RevenueRecognition.Application.Exceptions;

public class ForbidException : ApplicationException
{
    public ForbidException(string msg) : base(msg, 403){} 
}