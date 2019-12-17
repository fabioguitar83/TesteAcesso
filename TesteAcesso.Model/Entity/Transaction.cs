using System;
using System.Collections.Generic;
using System.Text;

namespace TesteAcesso.Model.Entity
{
    public class Transaction
    {
        public string Id { get; set; }
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public decimal Value { get; set; }
        public int StatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Message { get; set; }

        public virtual Status Status { get; set; }

    }
}
