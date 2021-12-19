using FluentValidation;
using PM.Common.Queries;

namespace PM.Api.Validation
{
    public class SearchProductQueryValidator : AbstractValidator<SearchProductQuery>
    {
        public SearchProductQueryValidator()
        {
            When(x => x.ProductId != null, () => {
                RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("PLease enter valid product id.");
            });

            When(x => x.ProductGroupId != null, () => {
                RuleFor(x => x.ProductGroupId).GreaterThan(0).WithMessage("PLease enter valid product id.");
            });

        }
    }
}
