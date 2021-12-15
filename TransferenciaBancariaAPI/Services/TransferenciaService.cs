using System.Net;
using TransferenciaBancariaAPI.Clients;
using TransferenciaBancariaAPI.Interface;
using TransferenciaBancariaAPI.Models;

namespace TransferenciaBancariaAPI.Services
{
    public class TransferenciaService : ITransferenciaService
    {
        private AcessoClientAPI clientAPI;
        private ILogger<TransferenciaService> _logger;
        private IElasticSearchService _elasticClient;

        public TransferenciaService(ILogger<TransferenciaService> logger, IElasticSearchService elasticClient)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            clientAPI = new AcessoClientAPI();
        }

        public async void transferValue(Transferencia transferencia)
        {
            try
            {
                if (await accountNotExists(transferencia.AccountDestination))
                {
                    throw new Exception($"The account destination {transferencia.AccountDestination} not exists.");
                }

                await clientAPI.PostAccount(new AccountPost
                {
                    AccountNumber = transferencia.AccountOrigin,
                    Value = transferencia.Value,
                    Type = "Debit"
                });

                await clientAPI.PostAccount(new AccountPost
                {
                    AccountNumber = transferencia.AccountDestination,
                    Value = transferencia.Value,
                    Type = "Credit"
                });

                _logger.LogInformation("Operation completed successfully");
                transferencia.Status = StatusType.CONFIRMED;
                _elasticClient._client.Index(transferencia, idx => idx.Index("transferencia"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                transferencia.Status = StatusType.ERROR;
                transferencia.Error = ex.Message;
                _elasticClient._client.Index(transferencia, idx => idx.Index("transferencia"));
            }

        }

        private async Task<bool> accountNotExists(string accountNumber)
        {
            var response = await clientAPI.GetAccount(accountNumber);

            return response.StatusCode != HttpStatusCode.OK;

        }
    }
}