using MeesageService.Shared.Enums;

namespace MeesageService.Shared.Behaviour;

public sealed record Errors
{
    public static readonly Errors None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Errors Null = new("Error.Null", "Null Value was provided", ErrorType.Failure);

    private Errors(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }
    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }
    public static Errors Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);
    public static Errors Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static Errors Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Errors NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);
}