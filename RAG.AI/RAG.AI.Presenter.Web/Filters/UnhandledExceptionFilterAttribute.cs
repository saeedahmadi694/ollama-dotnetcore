using RAG.AI.Infrastructure.Exceptions.BaseExceptions;
using BookHouse.Core.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace RAG.AI.Presenter.Web.Filters;

public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<UnhandledExceptionFilterAttribute> _logger;

    public UnhandledExceptionFilterAttribute(ILogger<UnhandledExceptionFilterAttribute> logger)
    {
        _logger = logger;
    }

    private static int _exceptionTracker = 1;


    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        var actionDescriptor =
            (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
        Type controllerType = actionDescriptor.ControllerTypeInfo;

        var controller = typeof(Controller);

        if (controllerType.IsSubclassOf(controller))
        {
            IActionResult result;
            if (context.Exception is ValidationException exception)
            {
                var errorBuilder = new StringBuilder();
                //errorBuilder.AppendLine("Command Validation Error");
                foreach (var error in exception.Errors)
                {
                    errorBuilder.AppendLine(error.ErrorMessage);
                }

                result = new ObjectResult(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "A validation error occurred !",
                    Detail = errorBuilder.ToString(),
                    Type = this.GetGenericTypeName()
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            else if (context.Exception is BaseException businessException)
            {
                string exceptionTracker = DateTime.Today.Day.ToString() + _exceptionTracker;
                _exceptionTracker++;

                result = ConvertBaseExceptionToHttpObjectResult(businessException);
                _logger.LogWarning(context.Exception, $"BusinessException {exceptionTracker}");
            }
            else
            {
                string exceptionTracker = DateTime.Today.Day.ToString() + _exceptionTracker;
                _exceptionTracker++;
                _logger.LogError(context.Exception, $"Exception {exceptionTracker}");
                result = new ObjectResult(new ProblemDetails()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal server error",
                    Detail = context.Exception.Message,
                    Type = context.Exception.GetGenericTypeName()
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            context.Result = result;
        }
        else if (controllerType.IsSubclassOf(controller))
        {
            IActionResult result;
            string exceptionTracker = DateTime.Today.Day.ToString() + _exceptionTracker;
            _exceptionTracker++;
            _logger.LogError(context.Exception, $"Exception {exceptionTracker}");
            result = new ObjectResult(new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error",
                Detail = context.Exception.Message,
                Type = context.Exception.GetGenericTypeName()
            });
            context.Result = result;
        }

        await base.OnExceptionAsync(context);
    }

    private ObjectResult ConvertBaseExceptionToHttpObjectResult(BaseException baseException)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        switch (baseException)
        {
            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                break;
            case InvalidInputException:
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case OperationNotAllowedException:
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case DuplicateException:
                statusCode = StatusCodes.Status409Conflict;
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        return new ObjectResult(new ProblemDetails()
        {
            Status = statusCode,
            Title = "A business error occurred !",
            Detail = baseException.Message,
            Type = baseException.GetGenericTypeName()
        })
        {
            StatusCode = statusCode
        };
    }
}


