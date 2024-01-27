using Hestia.Domain.Exceptions;

namespace Hestia.API.Exceptions;

public class MissingEnvironmentVariableException(string missingEnvironmentVariable)
    : HestiaException("Missing environment variable: " + missingEnvironmentVariable);