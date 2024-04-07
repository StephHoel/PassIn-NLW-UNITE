using System.Diagnostics.CodeAnalysis;

namespace Exceptions;

[ExcludeFromCodeCoverage]
public class ErrorOnValidationException(string message) : PassInException(message)
{
}