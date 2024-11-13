using FluentValidation;
using KN_EXE201.Application.Features.Category.Queries.GetById;
using KN_EXE201.Application.Features.Koi.Queries.GetActiveAuctionByKoiId;

namespace KN_EXE201.Application.Features.User.Queries.GetById
{
    public class GetActiveAuctionByKoiIdValidator : AbstractValidator<GetActiveAuctionByKoiIdQuery>
    {
        public GetActiveAuctionByKoiIdValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required");
        }
    }
}
