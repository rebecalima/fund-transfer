using RabbitMQ.Client;

namespace TransferenciaBancariaAPI.Interface
{
    public interface IMessageService
    {
        public IModel _channel { get; }
    }
}