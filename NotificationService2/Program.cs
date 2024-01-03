using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartConsuming();
        }

        private static void StartConsuming()
        {
            try
            {
                var connectionFactory = new ConnectionFactory { HostName = "localhost" };
                using (var connection = connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("order_creation_exchange", ExchangeType.Fanout, durable: true, autoDelete: false);
                    channel.ExchangeDeclare("order_update_exchange", ExchangeType.Topic, durable: true, autoDelete: false);

                    var queueName = "notification_service_2_queue";

                    channel.QueueDeclare(queue: queueName,
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

                    channel.QueueBind(queueName, "order_creation_exchange", string.Empty);
                    channel.QueueBind(queueName, "order_update_exchange", "order");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = System.Text.Encoding.UTF8.GetString(body);

                        Console.WriteLine($"Received message from Exchange: {ea.Exchange}");
                        Console.WriteLine($"Routing Key: {ea.RoutingKey}");
                        Console.WriteLine($"Message Body: {message}");
                        Console.WriteLine("-------------------------------------------------");
                        Console.WriteLine();
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                    Console.WriteLine("Notification Service 2 is listening for order creation/updation events.");
                    Console.WriteLine($" [*] Waiting for messages on queue: {queueName}");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Notification Service 2: {ex.Message}");
            }
        }
    }
}