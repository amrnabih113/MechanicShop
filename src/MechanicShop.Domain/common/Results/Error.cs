namespace MechanicShop.Domain.Common.Results;

public readonly record struct Error
{
    private Error(ErrorKind type, string description, string code)
    {
        Type = type;
        Description = description;
        Code = code;
    }

    public ErrorKind Type { get; }
    public string Description { get; }
    public string Code { get; }

    public static Error Failure(string description = "General failure", string code = nameof(Failure))
        => new(ErrorKind.Failure, description, code);

    public static Error Unexpected(string description = "An unexpected error occurred", string code = nameof(Unexpected))
        => new(ErrorKind.Unexpected, description, code);

    public static Error Validation(string description = "Validation error", string code = nameof(Validation))
        => new(ErrorKind.Validation, description, code);

    public static Error Conflict(string description = "Conflict error", string code = nameof(Conflict))
        => new(ErrorKind.conflict, description, code);

    public static Error NotFound(string description = "Resource not found", string code = nameof(NotFound))
        => new(ErrorKind.NotFound, description, code);

    public static Error UnAuthorized(string description = "Unauthorized access", string code = nameof(UnAuthorized))
        => new(ErrorKind.UnAuthorized, description, code);

    public static Error Forbidden(string description = "Forbidden access", string code = nameof(Forbidden))
        => new(ErrorKind.Forbidden, description, code);



}
