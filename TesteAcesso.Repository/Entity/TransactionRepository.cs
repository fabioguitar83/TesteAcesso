using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TesteAcesso.Model.Entity;
using TesteAcesso.Model.Interfaces;
using TesteAcesso.Repository.Infrastructure;

namespace TesteAcesso.Repository.Entity
{
    public class TransactionRepository : Common.Repository, ITransactionRepository
    {
        public TransactionRepository(Context context) : base(context)
        {

        }

        public async Task InsertAsync(Transaction transaction)
        {
            await Context.Transaction.AddAsync(transaction);

            await SaveChangesAsync();
        }


        public async Task UpdateAsync(Transaction transaction)
        {
            var transactionUpdate = await Context.Transaction.FirstOrDefaultAsync(p => p.Id == transaction.Id);

            transactionUpdate.Message = transaction.Message;
            transactionUpdate.StatusId = transaction.StatusId;
            transactionUpdate.UpdateDate = transaction.UpdateDate;
            transactionUpdate.Value = transaction.Value;
            transactionUpdate.AccountDestination = transaction.AccountDestination;
            transactionUpdate.AccountOrigin = transaction.AccountOrigin;

            await SaveChangesAsync();
        }

        public async Task<Transaction> GetAsync(string id)
        {
            return await Context.Transaction.Include(p => p.Status).FirstOrDefaultAsync(p => p.Id == id);

        }
    }
}
