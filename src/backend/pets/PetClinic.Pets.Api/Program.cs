using PetClinic.Pets.Application;
using PetClinic.Pets.Contracts;
using PetClinic.Pets.Infrastructure;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapGet("/", () => "PetClinic Pets API");

app.MapGet("/pets/{id:guid}", async (Guid id, IPetService service, CancellationToken ct) =>
{
    var pet = await service.GetByIdAsync(id, ct);
    return pet is null ? Results.NotFound() : Results.Ok(pet);
})
.WithName("GetPet");

app.MapGet("/pets", async (Guid? ownerId, IPetService service, CancellationToken ct) =>
{
    if (ownerId is null)
        return Results.BadRequest("ownerId query parameter is required.");
    var pets = await service.GetByOwnerIdAsync(ownerId.Value, ct);
    return Results.Ok(pets);
})
.WithName("GetPetsByOwner");

app.MapGet("/pets/search", async (string? query, IPetService service, CancellationToken ct) =>
{
    var pets = await service.SearchAsync(query, ct);
    return Results.Ok(pets);
})
.WithName("SearchPets")
.WithOpenApi();

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
.WithName("DeletePet")
.WithOpenApi();

app.Run();
