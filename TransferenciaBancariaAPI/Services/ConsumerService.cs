using TransferenciaBancariaAPI.Interface;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace TransferenciaBancariaAPI.Services
{
    class ConsumerService : BackgroundService
    {
        private readonly IMessageService _service;
        private readonly ILogger<ConsumerService> _logger;
        public ConsumerService(IMessageService service, ILogger<ConsumerService> logger)
        {
            _service = service;
            _logger = logger;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string queue = "account-transfer-pending";
            var consumer = new EventingBasicConsumer(_service._channel);
            consumer.Received += (sender, EventArgs) =>
            {
                var contentArray = EventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);

                _logger.LogInformation($"A new message was consumed. Message: {contentString}");
                _service._channel.BasicAck(EventArgs.DeliveryTag, false);

            };
            _service._channel.BasicConsume(queue, false, consumer);
            return Task.CompletedTask;
        }
    }
}