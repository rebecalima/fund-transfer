using FundTransferAPI.Models;

namespace FundTransferAPI.Interface
{
    public interface IClientAPI
    {
        public Task<HttpResponseMessage> GetAccount(string accountNumber);
        public Task<HttpResponseMessage> PostAccount(AccountAcesso account);
    }
}