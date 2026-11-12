using RSSFeedReader.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SubscriptionStore>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:5213", "https://localhost:7025")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.MapGet(
    "/api/subscriptions",
    (SubscriptionStore store) => store.GetAll());

app.MapPost(
    "/api/subscriptions",
    IResult (CreateSubscriptionRequest? request, SubscriptionStore store) =>
    {
        if (request is null || !store.TryAdd(request.Url))
        {
            return Results.BadRequest();
        }

        return Results.NoContent();
    });

app.Run();

public partial class Program { }

public sealed record CreateSubscriptionRequest(string? Url);
