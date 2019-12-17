using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TesteAcesso.Model.Entity;
using TesteAcesso.Repository.Infrastructure;
using static TesteAcesso.Model.Entity.Status;

namespace TesteAcesso.Repository.Initialize
{
    public static class InicializaBD
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();

            if (context.Status.Count() <= 0)
            {
                context.Status.Add(new Status() { Id = eStatus.InQueue.GetHashCode(), Description = "In Queue" }) ;
                context.Status.Add(new Status() { Id = eStatus.Processing.GetHashCode(), Description = "Processing" });
                context.Status.Add(new Status() { Id = eStatus.Confirmed.GetHashCode(), Description = "Confirmed" });
                context.Status.Add(new Status() { Id = eStatus.Error.GetHashCode(), Description = "Error" });
            }

            context.SaveChanges();
        }
    }
}
