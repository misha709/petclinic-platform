using Microsoft.EntityFrameworkCore;
using PetClinic.Vets.Application;
using PetClinic.Vets.Contracts;
using PetClinic.Vets.Infrastructure;
using PetClinic.Vets.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddVetsInfrastructure(builder.Configuration);
builder.Services.AddScoped<IVetService, VetService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VetsDbContext>();
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

app.MapGet("/", () => "PetClinic Vets API");

app.MapGet("/vets/{id:guid}", async (Guid id, IVetService service, CancellationToken ct) =>
{
    var vet = await service.GetByIdAsync(id, ct);
    return vet is null ? Results.NotFound() : Results.Ok(vet);
})
.WithName("GetVet");

app.MapGet("/vets", async (string? query, IVetService service, CancellationToken ct) =>
{
    var vets = await service.SearchAsync(query, ct);
    return Results.Ok(vets);
})
.WithName("GetVets");

app.MapPost("/vets", async (CreateVetRequest request, IVetService service, CancellationToken ct) =>
{
    var vet = await service.CreateAsync(request, ct);
    return Results.Created($"/vets/{vet.Id}", vet);
})
.WithName("CreateVet");

app.MapPut("/vets/{id:guid}", async (Guid id, UpdateVetRequest request, IVetService service, CancellationToken ct) =>
{
    var vet = await service.UpdateAsync(id, request, ct);
    return vet is null ? Results.NotFound() : Results.Ok(vet);
})
.WithName("UpdateVet");

app.MapDelete("/vets/{id:guid}", async (Guid id, IVetService service, CancellationToken ct) =>
{
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteVet");

app.MapPut("/vets/{id:guid}/specializations", async (Guid id, AssignSpecializationsRequest request, IVetService service, CancellationToken ct) =>
{
    var vet = await service.AssignSpecializationsAsync(id, request, ct);
    return vet is null ? Results.NotFound() : Results.Ok(vet);
})
.WithName("AssignVetSpecializations");

app.MapGet("/specializations/{id:int}", async (int id, ISpecializationService service, CancellationToken ct) =>
{
    var specialization = await service.GetByIdAsync(id, ct);
    return specialization is null ? Results.NotFound() : Results.Ok(specialization);
})
.WithName("GetSpecialization");

app.MapGet("/specializations", async (string? query, ISpecializationService service, CancellationToken ct) =>
{
    var specializations = string.IsNullOrWhiteSpace(query) 
        ? await service.GetAllAsync(ct)
        : await service.SearchAsync(query, ct);
    return Results.Ok(specializations);
})
.WithName("GetSpecializations");

app.MapPost("/specializations", async (CreateSpecializationRequest request, ISpecializationService service, CancellationToken ct) =>
{
    var specialization = await service.CreateAsync(request, ct);
    return Results.Created($"/specializations/{specialization.Id}", specialization);
})
.WithName("CreateSpecialization");

app.MapPut("/specializations/{id:int}", async (int id, UpdateSpecializationRequest request, ISpecializationService service, CancellationToken ct) =>
{
    var specialization = await service.UpdateAsync(id, request, ct);
    return specialization is null ? Results.NotFound() : Results.Ok(specialization);
})
.WithName("UpdateSpecialization");

app.MapDelete("/specializations/{id:int}", async (int id, ISpecializationService service, CancellationToken ct) =>
{
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteSpecialization");

app.Run();
