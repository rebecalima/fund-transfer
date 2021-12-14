using System.Text;
using TransferenciaBancariaAPI.Interface;

namespace TransferenciaBancariaAPI.Models
{
    class Message : IMessage
    {
        public string MessageText { get; set; }
        public DateTime DateMessage { get; }

        public Message(string messageText)
        {
            MessageText = messageText;
            DateMessage = DateTime.UtcNow;
        }

        public byte[] toByte()
        {
            return Encoding.UTF8.GetBytes($"message: {MessageText} - date: {DateMessage}");
        }
    }
}