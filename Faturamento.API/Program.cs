using Faturamento.Application.Interfaces;
using Faturamento.Domain.Interfaces;
using Faturamento.Infrastructure.Data;
using Faturamento.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AcessoAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("AppConnectionString");

builder.Services.AddDbContext<FaturamentoContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        x => x.MigrationsAssembly("Faturamento.Infrastructure")
    );
});

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

builder.Services.AddScoped<IInvoiceAppService, InvoiceAppService>();

builder.Services.AddHttpClient("EstoqueAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7265/");
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddScoped<IInvoiceAppService, InvoiceAppService>();

builder.Services.AddHttpClient<EstoqueHttpClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7265/");
});

var app = builder.Build();

app.UseCors("AcessoAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();