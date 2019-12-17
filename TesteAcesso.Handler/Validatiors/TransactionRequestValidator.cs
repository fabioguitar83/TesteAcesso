using FluentValidation;
using TesteAcesso.Model.Handlers;

namespace TesteAcesso.Handler.Validatiors
{
    public class TransactionRequestValidator: AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(a => a.AccountDestination)
                .NotEmpty()
                .WithMessage("AccountDestination is required");

            RuleFor(a => a.AccountOrigin)
               .NotEmpty()
               .WithMessage("AccountOrigin is required");

            RuleFor(a => a.Value)
               .NotEmpty()
               .WithMessage("Value is required");

            RuleFor(a => a.Value)
               .GreaterThan(0)
               .WithMessage("Value must be greater than 0");
        }
    }
}
