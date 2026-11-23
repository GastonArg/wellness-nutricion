using Microsoft.EntityFrameworkCore;
using WellnessNutricion.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Registramos EF Core y configuramos PostgreSQL.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Swagger para documentar y probar nuestra API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoint de prueba generado por ASP.NET Core.
var summaries = new[]
{
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// Endpoint temporal para comprobar la conexión con PostgreSQL.
app.MapGet("/db-test", async (ApplicationDbContext db) =>
{
    var conectado = await db.Database.CanConnectAsync();

    return conectado
        ? "Conexión con PostgreSQL OK 🐘"
        : "No se pudo conectar con PostgreSQL";
});

app.MapControllers();
app.Run();


record WeatherForecast(
    DateOnly Date,
    int TemperatureC,
    string? Summary)
{
    public int TemperatureF =>
        32 + (int)(TemperatureC / 0.5556);
}