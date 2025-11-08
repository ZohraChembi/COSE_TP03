using Microsoft.EntityFrameworkCore;
using OrderService.Api.Services;
using OrderService.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configuration de la base de données (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Ajouter les services

builder.Services.AddScoped<OrderService.Api.Services.OrderService>();
builder.Services.AddScoped<OrderService.Api.Services.CartService>();
// 🔹 Ajouter les contrôleurs
builder.Services.AddControllers();

// 🔹 Ajouter Swagger (avant builder.Build())
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 🔹 Mapper les contrôleurs
app.MapControllers();

app.Run();
