using Microsoft.EntityFrameworkCore;
using PetClinic.Pets.Application;
using PetClinic.Pets.Contracts;
using PetClinic.Pets.Infrastructure;
using PetClinic.Pets.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddPetsInfrastructure(builder.Configuration);
builder.Services.AddScoped<IPetService, PetService>();

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PetsDbContext>();
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

app.MapHealthChecks("/health");
app.MapGet("/", () => "PetClinic Pets API [RD]");

app.MapGet("/pets/{id:guid}", async (Guid id, IPetService service, CancellationToken ct) =>
{
    var pet = await service.GetByIdAsync(id, ct);
    return pet is null ? Results.NotFound() : Results.Ok(pet);
})
.WithName("GetPet");

app.MapGet("/pets", async (string? query, Guid? ownerId, IPetService service, CancellationToken ct) =>
{
    var pets = await service.SearchAsync(query, ownerId, ct);
    return Results.Ok(pets);
})
.WithName("GetPets");

app.MapPost("/pets", async (CreatePetRequest request, IPetService service, CancellationToken ct) =>
{
    var pet = await service.CreateAsync(request, ct);
    return Results.Created($"/pets/{pet.Id}", pet);
})
.WithName("CreatePet");

app.MapPut("/pets/{id:guid}", async (Guid id, UpdatePetRequest request, IPetService service, CancellationToken ct) =>
{
    var pet = await service.UpdateAsync(id, request, ct);
    return pet is null ? Results.NotFound() : Results.Ok(pet);
})
.WithName("UpdatePet");

app.MapDelete("/pets/{id:guid}", async (Guid id, IPetService service, CancellationToken ct) =>
{
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeletePet");

app.Run();
