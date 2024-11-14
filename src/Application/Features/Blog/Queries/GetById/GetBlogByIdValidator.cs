using FluentValidation;

namespace Application.Features.Koi.Queries.GetById;
public class GetBlogByIdValidator : AbstractValidator<GetBlogByIdQuery>
{
    public GetBlogByIdValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required");
    }
}
