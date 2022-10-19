using FluentValidation;

namespace NZWalk.API.Validators
{
    public class AddWalkDifficultyAsyncValidator : AbstractValidator<Models.DTO.AddWalkDifficultyRequest>
    {
        public AddWalkDifficultyAsyncValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
