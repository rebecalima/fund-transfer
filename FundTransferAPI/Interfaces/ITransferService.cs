namespace FundTransferAPI.Interface
{
    public interface ITransferService
    {
        public Task<bool> transferValue(Transfer transfer);
    }
}