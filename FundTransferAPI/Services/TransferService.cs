using System.Net;
using FundTransferAPI.Clients;
using FundTransferAPI.Interface;
using FundTransferAPI.Models;

namespace FundTransferAPI.Services
{
    public class TransferService : ITransferService
    {
        private AcessoClientAPI clientAPI;
        private ILogger<TransferService> _logger;
        private IElasticSearchService _elasticClient;

        public TransferService(ILogger<TransferService> logger, IElasticSearchService elasticClient)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            clientAPI = new AcessoClientAPI();
        }

        public async void transferValue(Transfer transfer)
        {
            try
            {
                if (await accountNotExists(transfer.AccountDestination))
                {
                    throw new Exception($"The account destination {transfer.AccountDestination} not exists.");
                }

                await clientAPI.PostAccount(new AccountPost
                {
                    AccountNumber = transfer.AccountOrigin,
                    Value = transfer.Value,
                    Type = "Debit"
                });

                await clientAPI.PostAccount(new AccountPost
                {
                    AccountNumber = transfer.AccountDestination,
                    Value = transfer.Value,
                    Type = "Credit"
                });

                _logger.LogInformation("Operation completed successfully");
                transfer.Status = StatusType.CONFIRMED;
                _elasticClient._client.Index(transfer, idx => idx.Index("transfer"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                transfer.Status = StatusType.ERROR;
                transfer.Error = ex.Message;
                _elasticClient._client.Index(transfer, idx => idx.Index("transfer"));
            }

        }

        private async Task<bool> accountNotExists(string accountNumber)
        {
            var response = await clientAPI.GetAccount(accountNumber);

            return response.StatusCode != HttpStatusCode.OK;

        }
    }
}