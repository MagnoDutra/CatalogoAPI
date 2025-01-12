using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class ApiLoggingFilter(ILogger<ApiLoggingFilter> logger) : IActionFilter
{
  public void OnActionExecuted(ActionExecutedContext context)
  {
    logger.LogInformation("Executando -> OnActionExecuted");
    logger.LogInformation("##############################################");
    logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
    logger.LogInformation($"Status code : {context.HttpContext.Response.StatusCode}");
    logger.LogInformation("##############################################");
  }

  public void OnActionExecuting(ActionExecutingContext context)
  {
    logger.LogInformation("Executando -> OnActionExecuting");
    logger.LogInformation("##############################################");
    logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
    logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
    logger.LogInformation("##############################################");
  }
}
