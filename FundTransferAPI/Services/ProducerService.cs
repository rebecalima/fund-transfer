using FundTransferAPI.Interface;
using RabbitMQ.Client;

namespace FundTransferAPI.Services
{
    class ProducerService : IProducerService
    {
        private readonly IModel _service;

        public ProducerService(IMessageService service)
        {
            _service = service.getChannel();
        }
        public bool enqueue(IMessage message, string queue)
        {
            var body = message.toByte();
            _service.BasicPublish("", queue, true, null, body);

            return true;
        }
    }
}