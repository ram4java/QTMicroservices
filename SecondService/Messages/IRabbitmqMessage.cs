using RabbitMQ.Client;

namespace SecondService.Messages
{
    public interface IRabbitmqMessage
    {
        void SendMessage<T>(T msg);
    }
}
