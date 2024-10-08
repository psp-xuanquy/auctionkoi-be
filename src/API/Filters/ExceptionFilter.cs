using FluentValidation;
using KoiAuction.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using KoiAuction.Application.Common.Exceptions;
using System.Diagnostics;

namespace KoiAuction.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException exception:
                    foreach (var error in exception.Errors)
                    {
                        context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState))
                    .AddContextInformation(context);
                    context.ExceptionHandled = true;
                    break;
                case ForbiddenAccessException:
                    context.Result = new ForbidResult();
                    context.ExceptionHandled = true;
                    break;
                case UnauthorizedAccessException:
                    context.Result = new ForbidResult();
                    context.ExceptionHandled = true;
                    break;
                case NotFoundException exception:
                    context.Result = new NotFoundObjectResult(new ProblemDetails
                    {
                        Detail = exception.Message
                    })
                    .AddContextInformation(context);
                    context.ExceptionHandled = true;
                    break;
                case DuplicationException exception:
                    context.Result = new BadRequestObjectResult(new ProblemDetails
                    {
                        Detail = exception.Message
                    })
                        .AddContextInformation(context);
                    context.ExceptionHandled = true;
                    break;
                case UnauthorizedException exception:
                    context.Result = new UnauthorizedObjectResult(new ProblemDetails
                    {
                        Detail = exception.Message
                    })
                        .AddContextInformation(context);
                    context.ExceptionHandled = true;
                    break;
                case ValidationsException exception:
                    context.Result = new BadRequestObjectResult(new ProblemDetails
                    {
                        Detail = exception.Message
                    })
                        .AddContextInformation(context);
                    context.ExceptionHandled = true;
                    break;

            }
        }
    }

    internal static class ProblemDetailsExtensions
    {
        public static IActionResult AddContextInformation(this ObjectResult objectResult, ExceptionContext context)
        {
            if (objectResult.Value is not ProblemDetails problemDetails)
            {
                return objectResult;
            }
            problemDetails.Extensions.Add("traceId", Activity.Current?.Id ?? context.HttpContext.TraceIdentifier);

            return objectResult;
        }
    }
}
