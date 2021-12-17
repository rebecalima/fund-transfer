using FundTransferAPI.Interface;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Nest;

namespace FundTransferAPI.Services
{
    class ConsumerService : BackgroundService
    {
        private readonly IModel _service;
        private readonly ILogger<ConsumerService> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ITransferService _transferService;
        public ConsumerService(
            IMessageService service,
            ILogger<ConsumerService> logger,
            IElasticSearchService elasticClient,
            ITransferService transferService)
        {
            _service = service.getChannel();
            _logger = logger;
            _elasticClient = elasticClient.GetClient();
            _transferService = transferService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string queue = "account-transfer-pending";
            var consumer = new EventingBasicConsumer(_service);
            consumer.Received += (sender, EventArgs) =>
            {
                var contentArray = EventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var transfer = JsonConvert.DeserializeObject<Transfer>(contentString);

                transfer.changeStatus(StatusType.PROCESSING);

                var message = JsonConvert.SerializeObject(transfer);
                _logger.LogInformation($"A new message was consumed. Message: {message}");

                _elasticClient.Index(transfer, idx => idx.Index("transfer"));
                _transferService.transferValue(transfer);

                _service.BasicAck(EventArgs.DeliveryTag, false);

            };
            _service.BasicConsume(queue, false, consumer);
            return Task.CompletedTask;
        }
    }

}