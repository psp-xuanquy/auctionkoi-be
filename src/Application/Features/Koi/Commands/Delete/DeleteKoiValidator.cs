//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KoiAuction.Application.Features.Koi.Commands.Delete
//{
//    public class DeleteKoiValidator : AbstractValidator<DeleteKoiCommand>
//    {
//        public DeleteKoiValidator()
//        {
//            ConfigureValidationRules();
//        }

//        private void ConfigureValidationRules()
//        {
//            RuleFor(x => x.ID)
//                .NotEmpty().WithMessage("ID is required");
//        }
//    }
//}
