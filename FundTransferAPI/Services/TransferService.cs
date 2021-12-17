using System.Net;
using FundTransferAPI.Clients;
using FundTransferAPI.Interface;
using FundTransferAPI.Models;
using Nest;

namespace FundTransferAPI.Services
{
    public class TransferService : ITransferService
    {
        private IClientAPI _clientAPI;
        private ILogger<TransferService> _logger;
        private IElasticClient _elasticClient;

        public TransferService(
            ILogger<TransferService> logger,
            IElasticSearchService elasticClient,
            IClientAPI clientAPI)
        {
            _logger = logger;
            _elasticClient = elasticClient.GetClient();
            _clientAPI = clientAPI;
        }

        public async Task<bool> transferValue(Transfer transfer)
        {
            try
            {
                if (transfer.AccountDestination is not null &&
                    await accountNotExists(transfer.AccountDestination))
                    throw new Exception($"The account destination {transfer.AccountDestination} not exists.");

                postToAccounts(transfer);
                confirmTransaction(transfer);

                return true;
            }
            catch (Exception ex)
            {
                registerError(transfer, ex.Message);
                return false;
            }

        }

        private void registerError(Transfer transfer, string messageError)
        {
            transfer.changeStatus(StatusType.ERROR, messageError);
            _elasticClient.Index(transfer, idx => idx.Index("transfer"));
            _logger.LogError(messageError);
        }

        private void confirmTransaction(Transfer transfer)
        {
            transfer.changeStatus(StatusType.CONFIRMED);
            _elasticClient.Index(transfer, idx => idx.Index("transfer"));
            _logger.LogInformation("Operation completed successfully");
        }

        private async void postToAccounts(Transfer transfer)
        {
            await _clientAPI.PostAccount(new AccountAcesso
            {
                AccountNumber = transfer.AccountOrigin,
                Value = transfer.Value,
                Type = "Debit"
            });

            await _clientAPI.PostAccount(new AccountAcesso
            {
                AccountNumber = transfer.AccountDestination,
                Value = transfer.Value,
                Type = "Credit"
            });
        }

        private async Task<bool> accountNotExists(string accountNumber)
        {
            var response = await _clientAPI.GetAccount(accountNumber);

            return response.StatusCode != HttpStatusCode.OK;

        }
    }
}