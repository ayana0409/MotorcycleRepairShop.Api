using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MotorcycleRepairShop.Share.Exceptions;
using System.Web.Http.Results;

namespace MotorcycleRepairShop.Application.Configurations
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is NotFoundException)
            {
                context.Result = new NotFoundObjectResult(new { context.Exception.Message });
                context.ExceptionHandled = true;
            }
            else if (context.Exception is ArgumentException)
            {
                context.Result = new BadRequestObjectResult(new { context.Exception.Message });
                context.ExceptionHandled = true;
            }
        }
    }
}
