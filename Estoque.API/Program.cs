using Estoque.Application.Services;
using Estoque.Domain.Interfaces;
using Estoque.Infrastructure.Context;
using Estoque.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AppConnectionString");

builder.Services.AddDbContext<EstoqueDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        x => x.MigrationsAssembly("Estoque.Infrastructure")
    );
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ProductAppService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();