using System;
using System.Collections.Generic;
using System.Text;

namespace TesteAcesso.Model.Acesso
{
    public class TransactionAccountRequest
    {
        public string AccountNumber { get; set; }
        public decimal Value { get; set; }
        public string Type { get; set; }

        public enum eTransactionType
        {
            Credit,
            Debit
        }
    }
}
