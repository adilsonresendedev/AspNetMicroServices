using Discount.API.Repositories;
using Discount.API.Repositories.Interfaces;
using Discount.API.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MigrateDatabase<Program>();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
