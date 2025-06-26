namespace RevenueRecognition.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string msg) : base(msg){} 
}