using System.Text;
using FundTransferAPI.Interface;

namespace FundTransferAPI.Models
{
    class Message : IMessage
    {
        public string MessageText { get; set; }

        public Message(string messageText)
        {
            MessageText = messageText;
        }

        public byte[] toByte()
        {
            return Encoding.UTF8.GetBytes(MessageText);
        }
    }
}