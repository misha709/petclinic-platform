using Microsoft.EntityFrameworkCore;
using PetClinic.Visits.Application;
using PetClinic.Visits.Contracts;
using PetClinic.Visits.Domain.Enums;
using PetClinic.Visits.Infrastructure;
using PetClinic.Visits.Infrastructure.Messaging;
using PetClinic.Visits.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.Configure<AwsMessagingOptions>(builder.Configuration.GetSection("AwsMessaging")); // TODO move to infrastructure layer
builder.Services.AddVisitsInfrastructure(builder.Configuration);
builder.Services.AddScoped<IVisitService, VisitService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VisitsDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogCritical(ex, "Database migration failed. Shutting down.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapGet("/", () => "PetClinic Visits API");

app.MapGet("/visits/{id:guid}", async (Guid id, IVisitService service, CancellationToken ct) =>
{
    var visit = await service.GetByIdAsync(id, ct);
    return visit is null ? Results.NotFound() : Results.Ok(visit);
})
.WithName("GetVisit");

app.MapGet("/visits", async (
    Guid? petId,
    Guid? vetId,
    DateOnly? date,
    string? status,
    IVisitService service,
    CancellationToken ct) =>
{
    VisitStatus? visitStatus = null;
    if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<VisitStatus>(status, ignoreCase: true, out var parsed))
        visitStatus = parsed;

    var visits = await service.SearchAsync(petId, vetId, date, visitStatus, ct);
    return Results.Ok(visits);
})
.WithName("GetVisits");

app.MapPost("/visits", async (CreateVisitRequest request, IVisitService service, CancellationToken ct) =>
{
    try
    {
        var visit = await service.CreateAsync(request, ct);
        return Results.Created($"/visits/{visit.Id}", visit);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateVisit");

app.MapPut("/visits/{id:guid}", async (Guid id, UpdateVisitRequest request, IVisitService service, CancellationToken ct) =>
{
    try
    {
        var visit = await service.UpdateAsync(id, request, ct);
        return visit is null ? Results.NotFound() : Results.Ok(visit);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("UpdateVisit");

app.MapPost("/visits/{id:guid}/cancel", async (Guid id, CancelVisitRequest request, IVisitService service, CancellationToken ct) =>
{
    try
    {
        var visit = await service.CancelAsync(id, request, ct);
        return visit is null ? Results.NotFound() : Results.Ok(visit);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
})
.WithName("CancelVisit");

app.MapPost("/visits/{id:guid}/complete", async (Guid id, IVisitService service, CancellationToken ct) =>
{
    try
    {
        var visit = await service.CompleteAsync(id, ct);
        return visit is null ? Results.NotFound() : Results.Ok(visit);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
})
.WithName("CompleteVisit");

app.MapDelete("/visits/{id:guid}", async (Guid id, IVisitService service, CancellationToken ct) =>
{
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteVisit");

app.Run();
