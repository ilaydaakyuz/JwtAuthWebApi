using MediatR;

namespace MyWebApi.Domain.Commands;

public class SendMessageCommand : IRequest<string>
{
    public string Message { get; set; }
    public string CorrelationId { get; set; }

    public SendMessageCommand(string message)
    {
        Message = message;
        CorrelationId = Guid.NewGuid().ToString(); // Benzersiz bir ID üretiyoruz
    }
}
