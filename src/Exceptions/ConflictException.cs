using System.Diagnostics.CodeAnalysis;

namespace Exceptions;

[ExcludeFromCodeCoverage]
public class ConflictException(string message) : PassInException(message)
{
}