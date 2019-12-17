using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TesteAcesso.Model.Interfaces
{
    public interface IRepository
    {
        Task SaveChangesAsync();
    }
}
