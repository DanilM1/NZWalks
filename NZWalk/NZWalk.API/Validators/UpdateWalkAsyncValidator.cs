using FluentValidation;

namespace NZWalk.API.Validators
{
    public class UpdateWalkAsyncValidator : AbstractValidator<Models.DTO.UpdateWalkRequest>
    {
        public UpdateWalkAsyncValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
