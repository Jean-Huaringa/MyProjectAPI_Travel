using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; // Ruta al formulario de inicio de sesión
        options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta para accesos denegados
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tiempo de expiración de la cookie
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyProjectTravelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConectionString"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
