namespace RevenueRecognition.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string msg) : base(msg, 404){} 
}