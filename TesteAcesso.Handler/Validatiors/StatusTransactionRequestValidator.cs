using FluentValidation;
using TesteAcesso.Model.Handlers;

namespace TesteAcesso.Handler.Validatiors
{
    public class StatusTransactionRequestValidator: AbstractValidator<StatusTransactionRequest>    
    {
        public StatusTransactionRequestValidator()
        {
            RuleFor(a => a.TransactionId)
                .NotEmpty()
                .WithMessage("TransactionId is required");
        }
    }
}
