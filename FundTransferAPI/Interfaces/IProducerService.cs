namespace FundTransferAPI.Interface
{
    public interface IProducerService
    {
        public bool enqueue(IMessage message, string queue);
    }
}