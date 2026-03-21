using PetClinic.Notifications.Api.HttpClients;
using PetClinic.Notifications.Api.Messaging;
using PetClinic.Notifications.Api.Services;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Email
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();

// HTTP clients for cross-service lookups
builder.Services.AddHttpClient<PetsApiClient>(client =>
{
    var baseUrl = builder.Configuration["Services:PetsApi"]
        ?? throw new InvalidOperationException("Configuration key 'Services:PetsApi' is required.");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<OwnersApiClient>(client =>
{
    var baseUrl = builder.Configuration["Services:OwnersApi"]
        ?? throw new InvalidOperationException("Configuration key 'Services:OwnersApi' is required.");
    client.BaseAddress = new Uri(baseUrl);
});

// Health checks
builder.Services.AddHealthChecks();

// Quartz (in-memory job store — switch to AdoJobStore with Npgsql for production persistence)
builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddMessaging(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "PetClinic Notifications API");
app.MapHealthChecks("/health");

app.Run();
