using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore.Infrastructure;
using Scalar.AspNetCore;
using TrilhasApi.Data;
using TrilhasApi.DTOs;
using TrilhasApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'OracleConnection' não configurada. Defina-a com 'dotnet user-secrets set \"ConnectionStrings:OracleConnection\" \"...\"' " +
        "ou na variável de ambiente ConnectionStrings__OracleConnection.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString, oracle =>
        oracle.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19)));

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)))
    .ConfigureApiBehaviorOptions(options =>
        options.InvalidModelStateResponseFactory = context =>
        {
            var erros = context.ModelState
                .Where(entrada => entrada.Value is { Errors.Count: > 0 })
                .ToList();

            // Falhas de leitura do JSON chegam com chave vazia ou iniciada por "$" e mensagem técnica em inglês.
            var corpoIlegivel = erros.Any(entrada => entrada.Key.Length == 0 || entrada.Key.StartsWith('$'));

            IDictionary<string, string[]> detalhes = corpoIlegivel
                ? new Dictionary<string, string[]>
                {
                    ["corpo"] = new[] { "JSON inválido ou valor em formato incorreto." }
                }
                : erros.ToDictionary(
                    entrada => entrada.Key,
                    entrada => entrada.Value!.Errors.Select(erro => erro.ErrorMessage).ToArray());

            return new BadRequestObjectResult(new ErroResponse("Dados inválidos.", detalhes));
        });

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference(options =>
        options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
