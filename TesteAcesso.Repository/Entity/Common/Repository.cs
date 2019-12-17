using System.Threading.Tasks;
using TesteAcesso.Model.Interfaces;
using TesteAcesso.Repository.Infrastructure;

namespace TesteAcesso.Repository.Entity.Common
{
    public abstract class Repository : IRepository
    {
        protected Context Context { get; }

        public Repository(Context context)
        {
            Context = context;
        }

        public async Task SaveChangesAsync()
        {
            if (Context.ChangeTracker.HasChanges())
            {
                await Context.SaveChangesAsync();
            }
        }
    }
}
