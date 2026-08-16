using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAppUtils.Middleware
{
    public class RequestValidator(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var referer = context.Request.Headers["Referrer"].FirstOrDefault();
            if (string.IsNullOrEmpty(referer))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                await context.Response.WriteAsync("You are not authorized to call this Service Directly");
                return;
            }
            else
            {
                await next(context);    
            }
        }
    }
}
