using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private IModel _channel;
 
    public RabbitMqConsumerService(IConnection connection)
    {
        _connection = connection;
    }
 
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "messageQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
 
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
 
            var response = $"Processed: {message}";
 
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = ea.BasicProperties.CorrelationId;
 
            _channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: replyProps, body: Encoding.UTF8.GetBytes(response));
        };
 
        _channel.BasicConsume(queue: "messageQueue", autoAck: true, consumer: consumer);
 
        return Task.CompletedTask;
    }
}