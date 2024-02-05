using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace WA_LogningAttribute.Attributes
{
    public class ActionLoggerAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            ILogger? logger = GetLogger(context, context.Controller.GetType());
            var httpMethod = context.HttpContext.Request.Method;
            var routeUrl = context.HttpContext.Request.Path;
            var controllerName = context.Controller.GetType().Name;

            if (logger == null)
            {
                return;
            }

            logger?.LogInformation($"[{httpMethod}][{controllerName}][{routeUrl}] Executed");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ILogger? logger = GetLogger(context, context.Controller.GetType());
            var parameters = context.ActionArguments;
            var httpMethod = context.HttpContext.Request.Method;
            var routeUrl = context.HttpContext.Request.Path;
            var controllerName = context.Controller.GetType().Name;

            if (logger == null)
            {
                return;
            }

            string baseMessage = $"[{httpMethod}][{controllerName}][{routeUrl}] Executing";

            switch (httpMethod)
            {
                case "POST":
                    // Get the type of body content
                    var bodyType = context.ActionDescriptor.Parameters
                        .FirstOrDefault(p => p.BindingInfo?.BindingSource?.DisplayName == "Body")
                        ?.Name;

                    // Get the value of bodytype
                    var bodyContent = bodyType != null ? JsonSerializer.Serialize(parameters[bodyType]) : null;

                    if (bodyContent != null)
                    {
                        logger.LogInformation($"{baseMessage} with body content: {bodyContent}");
                        break;
                    }

                    logger.LogInformation($"{baseMessage} without body content");
                    break;
                case "GET":
                    if (parameters.Count > 0)
                    {
                        logger.LogInformation($"{baseMessage} with parameters: {string.Join(", ", parameters)}");
                        break;
                    }

                    logger.LogInformation($"{baseMessage} without parameters");
                    break;
                default:
                    logger?.LogInformation($"{baseMessage} with parameters: {string.Join(", ", parameters)}");
                    break;
            }
        }

        private ILogger<T>? GetLogger<T>(FilterContext context, T type)
        {
            return (ILogger<T>?)context.HttpContext.RequestServices.GetService(typeof(ILogger<T>));
        }
    }
}
