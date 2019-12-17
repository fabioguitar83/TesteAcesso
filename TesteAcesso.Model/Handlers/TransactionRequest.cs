using MediatR;

namespace TesteAcesso.Model.Handlers
{
    public class TransactionRequest : IRequest<TransactionResponse>
    {
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public decimal Value { get; set; }
    }
}
