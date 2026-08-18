using CommonAppUtils.Middleware;
using Polly;
using Polly.Extensions.Http;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

/*
 var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() // Handles 5xx and 408
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) // Add 404
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)));

builder.Services.AddHttpClient("ExternalApiClient").AddPolicyHandler(retryPolicy);
*/

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() // Handles 5xx and 408
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) // Explicitly handle 404
    .CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 3, // Trip after 3 consecutive 404s
        durationOfBreak: TimeSpan.FromSeconds(30) // Stay open for 30 seconds
    );
builder.Services.AddHttpClient("ExternalApiClient").AddPolicyHandler(circuitBreakerPolicy);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<RequestValidator>();
app.MapControllers();

app.Run();
