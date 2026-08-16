namespace APIGateway.Middleware
{
    public class RequestInterceptor(RequestDelegate next){

	    public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["Referrer"] = "api-gateway";

            await next(context);
        }
    
    }
}
