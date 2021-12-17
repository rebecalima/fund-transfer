using RabbitMQ.Client;

namespace FundTransferAPI.Interface
{
    public interface IMessageService
    {
        public IModel getChannel();
    }
}