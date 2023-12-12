using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace doob.Reflectensions.AspNetCore
{
    public class ResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is Result result)
            {
                if (result.IsSuccess)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                    objectResult.Value = result.GetValue();
                }
                else
                {
                    switch (result.Error)
                    {
                        case NotFoundError nfe:
                            {
                                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                                objectResult.Value = null;
                                break;
                            }
                        case ExceptionError ee:
                            {
                                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                objectResult.Value = ee;
                                break;
                            }
                        default:
                            {
                                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                objectResult.Value = result.Error?.Message;
                                break;
                            }
                    }
                }

            }
        }
    }
}
