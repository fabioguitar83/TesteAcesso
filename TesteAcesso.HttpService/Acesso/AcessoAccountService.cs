using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TesteAcesso.Model.Acesso;
using TesteAcesso.Model.Interfaces;

namespace TesteAcesso.HttpService.Acesso
{
    public class AcessoAccountService : IAcessoAccountService
    {

        private readonly IConfiguration _config;

        public AcessoAccountService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<AccountResponse> GetAccountAsync(string accountNumber)
        {
            var urlAcesso = _config["Acesso:Url"];

            var getResp = Url.Combine(urlAcesso, "/api/Account/", accountNumber);

            var response = await getResp.GetJsonAsync<AccountResponse>();

            return response;
        }

        public async Task PostTransactionAsync(TransactionAccountRequest transactionAccount)
        {
            var urlAcesso = _config["Acesso:Url"];

            var getResp = Url.Combine(urlAcesso, "/api/Account/");

            var response = await getResp.PostJsonAsync(transactionAccount);

            if (!response.IsSuccessStatusCode) 
            {
                throw new System.Exception(response.Content.ToString());
            }

        }
    }
}
