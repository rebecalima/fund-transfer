using System.Net;
using FundTransferAPI.Interface;
using FundTransferAPI.Models;

namespace FundTransferAPI.Clients
{
    public class AcessoClientAPI : IClientAPI
    {
        private HttpClient _client;
        public AcessoClientAPI(IHttpClientFactory client, IConfiguration configuration)
        {
            _client = client.CreateClient(configuration["ClientAPI:Name"]);
        }

        public async Task<HttpResponseMessage> GetAccount(string accountNumber)
        {
            var response = await _client.GetAsync($"api/Account/{accountNumber}");

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new Exception("There was a problem on the client.");

            return response;
        }

        public async Task<HttpResponseMessage> PostAccount(AccountAcesso account)
        {
            var response = await _client.PostAsJsonAsync("api/Account", account);

            if (response.StatusCode == HttpStatusCode.OK)
                return response;

            var error = "There was a problem on the client.";

            if (response.StatusCode == HttpStatusCode.NotFound)
                error = $"The account {account.AccountNumber} to operation {account.Type} not exists.";

            if (response.StatusCode == HttpStatusCode.BadRequest)
                error = $"The account {account.AccountNumber} not has enough balance to operation {account.Type}.";

            throw new Exception(error);
        }

    }
}