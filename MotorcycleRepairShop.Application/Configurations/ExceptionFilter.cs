using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MotorcycleRepairShop.Share.Exceptions;

namespace MotorcycleRepairShop.Application.Configurations
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is NotFoundException)
            {
                context.Result = new BadRequestObjectResult(new { context.Exception.Message });
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new StatusCodeResult(500);
                context.ExceptionHandled = true;
            }
        }
    }
}
