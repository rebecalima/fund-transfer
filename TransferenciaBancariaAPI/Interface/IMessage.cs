namespace TransferenciaBancariaAPI.Interface
{
    public interface IMessage
    {
        public string MessageText { get; set; }
        public DateTime DateMessage { get; }

        public byte[] toByte();
    }
}