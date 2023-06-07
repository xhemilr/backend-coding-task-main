using Claims.Application.Requests.Claim;
using FluentValidation;

namespace Claims.Validators
{
    public class CreateClaimRequestValidator : AbstractValidator<CreateClaimRequest>
    {
        public CreateClaimRequestValidator()
        {
            RuleFor(x => x.CoverId).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Type).NotNull().NotEmpty();
            RuleFor(x => x.DamageCost).NotNull().LessThanOrEqualTo(100000);
            RuleFor(x => x.Created).NotNull();
        }
    }
}
