using FluentValidation;
using PM.Common.Queries;

namespace PM.Api.Validation
{
    public class SearchProductGroupQueryValidator : AbstractValidator<SearchProductGroupQuery>
    {
        public SearchProductGroupQueryValidator()
        {
            When(x => x.ProductGroupId != null, () => {
                RuleFor(x => x.ProductGroupId).GreaterThan(0).WithMessage("PLease enter valid product group id.");
            });

        }
    }
}
