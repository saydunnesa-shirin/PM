using FluentValidation;
using PM.Common.Commands;

namespace PM.Api.Validation
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("PLease enter product name."); ;
            RuleFor(x => x.ProductGroupId).GreaterThan(0).WithMessage("PLease enter product group.");
            RuleFor(x => x.EntryTime).NotNull().WithMessage("PLease enter product entry time."); ;

            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.VatRate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PriceWithVat).GreaterThanOrEqualTo(0);

            When(x => x.Price == 0, () => {
                RuleFor(x => x.PriceWithVat).GreaterThan(0);
                RuleFor(x => x.VatRate).GreaterThan(0);
            });

            When(x => x.VatRate == 0, () => {
                RuleFor(x => x.PriceWithVat).GreaterThan(0);
                RuleFor(x => x.Price).GreaterThan(0);
            });

            When(x => x.PriceWithVat == 0, () => {
                RuleFor(x => x.Price).GreaterThan(0);
                RuleFor(x => x.VatRate).GreaterThan(0);
            });

            RuleFor(x => x.StoreIds.Count).GreaterThan(0).WithMessage("PLease enter store(s)."); ;

        }
    }
}
