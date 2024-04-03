namespace Exceptions;

public class NotFoundException(string message) : PassInException(message)
{
}