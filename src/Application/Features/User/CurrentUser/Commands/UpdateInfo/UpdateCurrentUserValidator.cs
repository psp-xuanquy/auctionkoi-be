using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.User.CurrentUser.Commands.UpdateInfo;
public class UpdateCurrentUserInfoValidator : AbstractValidator<UpdateCurrentUserInfoCommand>
{
    public UpdateCurrentUserInfoValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        
    }
}
