using System.Diagnostics.CodeAnalysis;

namespace Exceptions;

[ExcludeFromCodeCoverage]
public class NotFoundException(string message) : PassInException(message)
{
}