using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
  public void OnException(ExceptionContext context)
  {
    logger.LogError(context.Exception, "Ocorreu uma exceção não tratada.");

    context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua solicitação.")
    {
      StatusCode = StatusCodes.Status500InternalServerError,
    };
  }
}
