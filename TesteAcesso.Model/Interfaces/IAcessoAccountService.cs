using System.Threading.Tasks;
using TesteAcesso.Model.Acesso;

namespace TesteAcesso.Model.Interfaces
{
    public interface IAcessoAccountService
    {
        Task<AccountResponse> GetAccountAsync(string accountNumber);
        Task PostTransactionAsync(TransactionAccountRequest transactionAccount);
    }
}
