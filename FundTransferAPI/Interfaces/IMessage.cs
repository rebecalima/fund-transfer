namespace FundTransferAPI.Interface
{
    public interface IMessage
    {
        public string MessageText { get; set; }

        public byte[] toByte();
    }
}