using RabbitMQ.Client;

namespace FundTransferAPI.Interface
{
    public interface IMessageService
    {
        public IModel _channel { get; }
    }
}