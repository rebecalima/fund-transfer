using System.Net;
using TransferenciaBancariaAPI.Models;

namespace TransferenciaBancariaAPI.Clients
{
    public class AcessoClientAPI
    {
        private HttpClient _client;
        public AcessoClientAPI()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://acessoaccount.herokuapp.com/api");
        }

        public async Task<HttpResponseMessage> GetAccount(string accountNumber, string endpoint = "api/Account")
        {
            var response = await _client.GetAsync($"{endpoint}/{accountNumber}");
            return response;
        }

        public async Task<HttpResponseMessage> PostAccount(AccountPost account, string endpoint = "api/Account")
        {
            var response = await _client.PostAsJsonAsync(endpoint, account);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception($"The account {account.AccountNumber} to operation {account.Type} not exists.");
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception($"The account {account.AccountNumber} not has enough balance.");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }

            return response;
        }

    }
}