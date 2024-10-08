using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace KoiAuction.Application.Common.Validation
{
    public class ValidatorProvider : IValidatorProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator<T> GetValidator<T>()
        {
            return _serviceProvider.GetService<IValidator<T>>()!;
        }
    }
}
