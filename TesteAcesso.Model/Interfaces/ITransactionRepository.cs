using System;
using System.Threading.Tasks;
using TesteAcesso.Model.Entity;

namespace TesteAcesso.Model.Interfaces
{
    public interface ITransactionRepository
    {
        Task InsertAsync(Transaction transaction);

        Task UpdateAsync(Transaction transaction);
        Task<Transaction> GetAsync(string id);
    }
}
