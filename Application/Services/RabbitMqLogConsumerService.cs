
using System.Text;
using MyWebApi.Infrastructure.Messaging.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqLogConsumerService(IConnection _connection, RabbitMqLogConsumer _logConsumer, ILogger<RabbitMqLogConsumerService> _logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.QueueDeclare(queue: "logQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message: {Message}", message);

                await _logConsumer.ProcessLogAsync(message);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: "logQueue", autoAck: true, consumer: consumer);

            await Task.Delay(-1, stoppingToken);
        }
    }
}