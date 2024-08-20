using MediatR;
using MyWebApi.Domain.Commands;
using MyWebApi.Domain.Interfaces;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, string>
{
    private readonly IRabbitMqService _rabbitMqService;

    public SendMessageCommandHandler(IRabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    public async Task<string> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // Mesajı RabbitMQ'ya gönderiyoruz ve cevap bekliyoruz
        var response = await _rabbitMqService.SendAndReceiveMessageAsync(request.Message, request.CorrelationId);
        return response;
    }
}



