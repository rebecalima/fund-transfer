using TransferenciaBancariaAPI.Interface;

namespace TransferenciaBancariaAPI.Service
{
    class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
        }
        public bool Enqueue(string message)
        {
            _logger.LogInformation("Enqueue");
            return true;
        }
    }
}