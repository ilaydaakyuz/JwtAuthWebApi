using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using MyWebApi.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyWebApi.Application.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;

        public RabbitMqService(IConnection connection)
        {
            _channel = connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;

            _consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: _replyQueueName, autoAck: true, consumer: _consumer);
        }

        public async Task<string> SendAndReceiveMessageAsync(string message, string correlationId)
        {
            var tcs = new TaskCompletionSource<string>();

            _consumer.Received += (model, ea) =>
 {
     var body = ea.Body.ToArray();
     var responseMessage = Encoding.UTF8.GetString(body);

     if (ea.BasicProperties.CorrelationId == correlationId)
     {
         tcs.SetResult(responseMessage);
     }
 };


            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = correlationId;
            properties.ReplyTo = _replyQueueName;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: "messageQueue", basicProperties: properties, body: messageBytes);

            return await tcs.Task;
        }
    }

}