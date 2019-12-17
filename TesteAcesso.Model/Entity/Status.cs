using System.Collections.Generic;

namespace TesteAcesso.Model.Entity
{
    public class Status
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Transaction> Transfers { get; set; }

        public enum eStatus
        {
            InQueue = 1,
            Processing = 2,
            Confirmed = 3,
            Error = 4
        }
    }
}
