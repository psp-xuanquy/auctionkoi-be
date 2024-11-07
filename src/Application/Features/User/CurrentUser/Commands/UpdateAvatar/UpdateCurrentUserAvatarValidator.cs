using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.User.CurrentUser.Commands.UpdateAvatar;
public class UpdateCurrentUserAvatarValidator : AbstractValidator<UpdateCurrentUserAvatarCommand>
{
    public UpdateCurrentUserAvatarValidator()
    {
        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        
    }
}
