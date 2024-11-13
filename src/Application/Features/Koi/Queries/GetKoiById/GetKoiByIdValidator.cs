using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KN_EXE201.Application.Features.Koi.Queries.GetActiveAuctionByKoiId;

namespace Application.Features.Koi.Queries.GetKoiById;
public class GetKoiByIdValidator : AbstractValidator<GetKoiByIdQuery>
{
    public GetKoiByIdValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required");
    }
}
