using RabbitMQ.Client;
using FundTransferAPI.Interface;

namespace FundTransferAPI.Services
{
    class MessageService : IMessageService
    {
        private ConnectionFactory _factory;
        private IConnection _conn;
        private IModel _channel { get; }
        public MessageService()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = "guest",
                Password = "guest"
            };
            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(
                queue: "account-transfer-pending",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueDeclare(
                queue: "account-transfer-failed",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

        }

        public IModel getChannel()
        {
            return _channel;
        }
    }
}