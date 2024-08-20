using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyWebApi.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqConsumer
    {
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqConsumer> _logger;

        public RabbitMqConsumer(IConnection connection, ILogger<RabbitMqConsumer> logger)
        {
            _channel = connection.CreateModel();
            _logger = logger;
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var correlationId = ea.BasicProperties.CorrelationId;
                var replyTo = ea.BasicProperties.ReplyTo;

                _logger.LogInformation("Message received: {Message}, CorrelationId: {CorrelationId}, ReplyTo: {ReplyTo}", message, correlationId, replyTo);

                // Mesajı işleyin ve yanıt oluşturun
                var response = ProcessMessage(message);

                // Yanıtı geri gönderin
                var replyProperties = _channel.CreateBasicProperties();
                replyProperties.CorrelationId = correlationId;

                var responseBytes = Encoding.UTF8.GetBytes(response);

                if (!string.IsNullOrEmpty(replyTo))
                {
                    _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: replyProperties, body: responseBytes);
                    _logger.LogInformation("Response sent: {Response}, CorrelationId: {CorrelationId}", response, correlationId);
                }
                else
                {
                    _logger.LogError("ReplyTo is null or empty. Unable to send response.");
                }
            };

            _channel.BasicConsume(queue: "messageQueue", autoAck: true, consumer: consumer);
        }

        private string ProcessMessage(string message)
        {
            // Mesajı işleme mantığınız
            return $"Processed: {message}";
        }
    }
}