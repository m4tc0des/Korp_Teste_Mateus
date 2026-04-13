using Microsoft.EntityFrameworkCore;
using Faturamento.Infrastructure.Data;
using Faturamento.Infrastructure.Repositories;
using Faturamento.Domain.Interfaces;
using Faturamento.Application.Interfaces;
using Faturamento.Application.Services;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();