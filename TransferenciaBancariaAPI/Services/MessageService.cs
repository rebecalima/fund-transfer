using RabbitMQ.Client;
using TransferenciaBancariaAPI.Interface;

namespace TransferenciaBancariaAPI.Services
{
    class MessageService : IMessageService
    {
        private ConnectionFactory _factory;
        private IConnection _conn;
        public IModel _channel { get; }
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
    }
}