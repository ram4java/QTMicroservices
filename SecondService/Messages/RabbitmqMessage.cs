using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace SecondService.Messages
{
    public class RabbitmqMessage
    {
        string exchangeName = "qt-exchange";
        string queueName = "qt-queue";
        string routingKey = "qt-routing-key";

        public async Task SendMessage<T>(T msg)
        {
            //1. create factory as localhost who can create channels, excanges, queues
            var factory = new ConnectionFactory { HostName = "localhost" };

            //2. create a connection using above factory
            var connection = await factory.CreateConnectionAsync();

            //3. create a channel to push messages to queue using above connection
            using var channel = await connection.CreateChannelAsync();

            // 4. Declare the Exchange (so it shows up in Admin UI)
            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true
            );

            // 5. Declare the Queue
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            // 6. Bind them together (Crucial for routing!)
            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey
            );



            //7. create a message in base64 format to be pushed into the queue
            var json = JsonSerializer.Serialize(msg);
            var body = Encoding.UTF8.GetBytes(json);

            //8. push this message into the queue
            await channel.BasicPublishAsync(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    body: body);
        }
    }
}
