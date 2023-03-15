using Entities.Concrete;
using FluentValidation;

namespace Business.Repositories.OperationClaimRepository.Validation.FleuntValidation
{
    public class OperationClaimValidator: AbstractValidator<OperationClaim>
    {
        public OperationClaimValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Yetki adı boş olamaz");
        }
    }
}
