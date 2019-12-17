using MediatR;

namespace TesteAcesso.Model.Handlers
{
    public class StatusTransactionRequest : IRequest<StatusTransactionResponse>
    {
        public StatusTransactionRequest(string transactionId)
        {
            TransactionId = transactionId;
        }
        public string TransactionId { get; set; }
    }
}
