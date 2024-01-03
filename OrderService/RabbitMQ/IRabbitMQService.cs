using RabbitMQ.Client;

namespace OrderService.RabbitMQ
{
    public interface IRabbitMQService
    {
        IConnection Connection { get; }

        Task PublishEvent<T>(string exchangeName, string exchangeType, string routingKey, T message);
    }
}
