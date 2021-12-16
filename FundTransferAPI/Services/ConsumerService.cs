using FundTransferAPI.Interface;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FundTransferAPI.Services
{
    class ConsumerService : BackgroundService
    {
        private readonly IMessageService _service;
        private readonly ILogger<ConsumerService> _logger;
        private readonly IElasticSearchService _elasticClient;
        private readonly ITransferService _transferService;
        public ConsumerService(
            IMessageService service,
            ILogger<ConsumerService> logger,
            IElasticSearchService elasticClient,
            ITransferService transferService)
        {
            _service = service;
            _logger = logger;
            _elasticClient = elasticClient;
            _transferService = transferService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string queue = "account-transfer-pending";
            var consumer = new EventingBasicConsumer(_service._channel);
            consumer.Received += (sender, EventArgs) =>
            {
                var contentArray = EventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var transfer = JsonConvert.DeserializeObject<Transfer>(contentString);
                transfer.Status = StatusType.PROCESSING;
                var message = JsonConvert.SerializeObject(transfer);

                _logger.LogInformation($"A new message was consumed. Message: {message}");

                _elasticClient._client.Index(transfer, idx => idx.Index("transfer"));
                _transferService.transferValue(transfer);

                _service._channel.BasicAck(EventArgs.DeliveryTag, false);

            };
            _service._channel.BasicConsume(queue, false, consumer);
            return Task.CompletedTask;
        }
    }

}