using FluentValidation;

namespace NZWalk.API.Validators
{
    public class UpdateWalkDifficultyAsyncValidator : AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>
    {
        public UpdateWalkDifficultyAsyncValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
