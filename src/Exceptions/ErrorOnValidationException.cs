namespace Exceptions;

public class ErrorOnValidationException(string message) : PassInException(message)
{
}