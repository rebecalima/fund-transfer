using FundTransferAPI.Interface;

namespace FundTransferAPI.Services
{
    class ProducerService : IProducerService
    {
        private readonly IMessageService _service;

        public ProducerService(IMessageService service)
        {
            _service = service;
        }
        public bool enqueue(IMessage message, string queue)
        {
            var body = message.toByte();
            _service._channel.BasicPublish("", queue, true, null, body);

            return true;
        }
    }
}