namespace MyWebApi.Domain.Enums;

public enum ErrorType
{
    NotFound,
    ValidationError,
    Unauthorized,
    Forbidden,
    InternalServerError,
    BadRequest,
    Conflict,
    ServiceUnavailable,
    Unknown
}
