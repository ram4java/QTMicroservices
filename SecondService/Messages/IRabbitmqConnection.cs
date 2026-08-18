using RabbitMQ.Client;

namespace SecondService.Messages
{
    public interface IRabbitmqConnection
    {
        IConnection connection { get; }
        
    }
}
