using System;
using MongoDB.Driver;
using Stripe;
namespace Task_Management_Service.Domain;

public class AppException : Exception
{
    public AppException(string message) : base(message)
    {
    }

    public AppException(string[] errorData, string exceptionType, int statusCode) : base()
    {
        ExceptionType = exceptionType;
        StatusCode = statusCode;
        ErrorData = errorData[0].Split(';').Select(e => e.Trim()).Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
    }

    public int StatusCode { get; set; }
    public string ExceptionType { get; set; }
    public string[] ErrorData { get; set; }
}

public class BadRequestException : AppException
{
    public BadRequestException(string error) 
        : base(new[] { error }, "BadRequest", 400) { }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string error) 
        : base(new[] { error }, "Unauthorized", 401) { }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string error) 
        : base(new[] { error }, "Forbidden", 403) { }
}

public class NotFoundException : AppException
{
    public NotFoundException(string error) 
        : base(new[] { error }, "NotFound", 404) { }
}

public class MethodNotAllowedException : AppException
{
    public MethodNotAllowedException(string error) 
        : base(new[] { error }, "MethodNotAllowed", 405) { }
}

public class ConflictException : AppException
{
    public ConflictException(string error) 
        : base(new[] { error }, "Conflict", 409) { }
}

public class UnsupportedMediaTypeException : AppException
{
    public UnsupportedMediaTypeException(string error) 
        : base(new[] { error }, "UnsupportedMediaType", 415) { }
}

public class UnprocessableEntityException : AppException
{
    public UnprocessableEntityException(string error) 
        : base(new[] { error }, "UnprocessableEntity", 422) { }
}

public class TooManyRequestsException : AppException
{
    public TooManyRequestsException(string error) 
        : base(new[] { error }, "TooManyRequests", 429) { }
}

public class InternalServerException : AppException
{
    public InternalServerException(string error) 
        : base(new[] { error }, "InternalServerError", 500) { }
}

public class BadGatewayException : AppException
{
    public BadGatewayException(string error) 
        : base(new[] { error }, "BadGateway", 502) { }
}

public class ServiceUnavailableException : AppException
{
    public ServiceUnavailableException(string error) 
        : base(new[] { error }, "ServiceUnavailable", 503) { }
}

public class GatewayTimeoutException : AppException
{
    public GatewayTimeoutException(string error) 
        : base(new[] { error }, "GatewayTimeout", 504) { }
}



