using FluentValidation;
using KN_EXE201.Application.Features.Category.Queries.GetById;

namespace KN_EXE201.Application.Features.User.Queries.GetById
{
    public class GetKoiFarmBreederByIdValidator : AbstractValidator<GetKoiFarmBreederByIdQuery>
    {
        public GetKoiFarmBreederByIdValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID là bắt buộc");
        }
    }
}
