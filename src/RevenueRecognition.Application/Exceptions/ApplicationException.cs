namespace RevenueRecognition.Application.Exceptions;

public class ApplicationException : Exception
{
    public readonly int StatusCode;

    public ApplicationException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}