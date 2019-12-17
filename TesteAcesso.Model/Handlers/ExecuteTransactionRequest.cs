using MediatR;

namespace TesteAcesso.Model.Handlers
{
    public class ExecuteTransactionRequest: IRequest<ExecuteTransactionResponse>
    {
        public ExecuteTransactionRequest(string transactionId)
        {
            TransactionId = transactionId;
        }
        public string TransactionId { get; set; }
    }
}
