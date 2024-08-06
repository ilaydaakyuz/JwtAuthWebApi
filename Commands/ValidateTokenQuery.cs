using MediatR;

public class ValidateTokenQuery : IRequest<ValidationResult>
{
    public string? Token { get; set; }
}