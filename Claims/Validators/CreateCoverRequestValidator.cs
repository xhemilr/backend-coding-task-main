using Claims.Application.Requests.Cover;
using FluentValidation;

namespace Claims.Validators
{
    public class CreateCoverRequestValidator : AbstractValidator<CreateCoverRequest>
    {
        public CreateCoverRequestValidator()
        {
            RuleFor(x => x.StartDate).NotNull().NotEmpty().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
            RuleFor(x => x.EndDate).NotNull().NotEmpty();
            RuleFor(x => x.Type).NotNull().NotEmpty();
            RuleFor(x => x.Premium).NotEmpty();
        }
    }
}
