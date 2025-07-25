var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/gyms", () => new[] { "Gym A", "Gym B", "Gym C" })
    .WithName("GetGyms");

app.Run();
