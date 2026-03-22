using Microsoft.EntityFrameworkCore;
using PetClinic.Owners.Application;
using PetClinic.Owners.Contracts;
using PetClinic.Owners.Infrastructure;
using PetClinic.Owners.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOwnersInfrastructure(builder.Configuration);
builder.Services.AddScoped<IOwnerService, OwnerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OwnersDbContext>();
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

app.MapGet("/", () => "PetClinic Owners API");

app.MapGet("/owners/{id:guid}", async (Guid id, IOwnerService service, CancellationToken ct) =>
{
    var owner = await service.GetByIdAsync(id, ct);
    return owner is null ? Results.NotFound() : Results.Ok(owner);
})
.WithName("GetOwner");

app.MapGet("/owners", async (string? query, IOwnerService service, CancellationToken ct) =>
{
    var owners = await service.SearchAsync(query, ct);
    return Results.Ok(owners);
})
.WithName("SearchOwners");

app.MapPost("/owners", async (CreateOwnerRequest request, IOwnerService service, CancellationToken ct) =>
{
    var owner = await service.CreateAsync(request, ct);
    return Results.Created($"/owners/{owner.Id}", owner);
})
.WithName("CreateOwner");

app.MapPut("/owners/{id:guid}", async (Guid id, UpdateOwnerRequest request, IOwnerService service, CancellationToken ct) =>
{
    var owner = await service.UpdateAsync(id, request, ct);
    return owner is null ? Results.NotFound() : Results.Ok(owner);
})
.WithName("UpdateOwner");

app.MapDelete("/owners/{id:guid}", async (Guid id, IOwnerService service, CancellationToken ct) =>
{
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteOwner");

app.Run();
