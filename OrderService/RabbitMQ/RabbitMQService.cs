using System.Text.Json;
using System.Text;

using RabbitMQ.Client;

namespace OrderService.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        public IConnection Connection { get; set; }

        public RabbitMQService()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            Connection = factory.CreateConnection();
        }

        public Task PublishEvent<T>(string exchangeName, string exchangeType, string routingKey, T message)
        {
            using (var channel = Connection.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, exchangeType, durable: true, autoDelete: false);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(exchangeName, routingKey, null, body);
            }

            return Task.CompletedTask;
        }
    }
}
