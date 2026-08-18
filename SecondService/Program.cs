using CommonAppUtils.Middleware;
using Microsoft.Extensions.Caching.Hybrid;
using SecondService.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<RabbitmqMessage>();

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "testapi";

});

// Configure HybridCache (Combines IMemoryCache + Redis automatically)
builder.Services.AddHybridCache(options => {
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(10), // Total L2 Expiration :: in Redis
        LocalCacheExpiration = TimeSpan.FromMinutes(5) // L1 Expiration :: In memory
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestValidator>();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/minimal/Course", async (HybridCache cache) => {
    var cacheKey = "1";
    var courses = await cache.GetOrCreateAsync(
        cacheKey,
        async (CancellationToken ct) => {
            Console.WriteLine("fetching from DB");
            await Task.Delay(10000, ct);
            return new Course("1", "DotnetFS", 6, "C#, SQL, EF, .NETCore, Microservices");
        }
    );

    return Results.Ok(courses);
});

app.Run();

record Course(string id, string name, int duration, string modules);
