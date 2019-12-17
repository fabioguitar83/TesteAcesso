using FluentValidation;
using TesteAcesso.Model.Handlers;

namespace TesteAcesso.Handler.Validatiors
{
    public class ExecuteTransactionRequestValidator: AbstractValidator<ExecuteTransactionRequest>
    {
        public ExecuteTransactionRequestValidator()
        {
            RuleFor(a => a.TransactionId)
               .NotEmpty()
               .WithMessage("TransactionId is required");
        }
    }
}
