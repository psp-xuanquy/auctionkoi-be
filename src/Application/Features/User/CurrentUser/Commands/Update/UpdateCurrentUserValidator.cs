using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.User.CurrentUser.Commands.Update;
public class UpdateCurrentUserValidator : AbstractValidator<UpdateCurrentUserCommand>
{
    public UpdateCurrentUserValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        
    }
}
