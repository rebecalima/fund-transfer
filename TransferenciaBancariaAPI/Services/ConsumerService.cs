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
        private readonly IElasticSearchService _elasticClient;
        private readonly ITransferenciaService _transferenciaService;
        public ConsumerService(
            IMessageService service,
            ILogger<ConsumerService> logger,
            IElasticSearchService elasticClient,
            ITransferenciaService transferenciaService)
        {
            _service = service;
            _logger = logger;
            _elasticClient = elasticClient;
            _transferenciaService = transferenciaService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string queue = "account-transfer-pending";
            var consumer = new EventingBasicConsumer(_service._channel);
            consumer.Received += (sender, EventArgs) =>
            {
                var contentArray = EventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var transferencia = JsonConvert.DeserializeObject<Transferencia>(contentString);
                transferencia.Status = StatusType.PROCESSING;
                var message = JsonConvert.SerializeObject(transferencia);

                _logger.LogInformation($"A new message was consumed. Message: {message}");

                _elasticClient._client.Index(transferencia, idx => idx.Index("transferencia"));
                _transferenciaService.transferValue(transferencia);

                _service._channel.BasicAck(EventArgs.DeliveryTag, false);

            };
            _service._channel.BasicConsume(queue, false, consumer);
            return Task.CompletedTask;
        }
    }

}