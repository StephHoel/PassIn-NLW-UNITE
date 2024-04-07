using System.Diagnostics.CodeAnalysis;

namespace Exceptions;

[ExcludeFromCodeCoverage]
public class PassInException(string message) : SystemException(message)
{
}