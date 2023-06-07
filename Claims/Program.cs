using System.Reflection;
using Claims.Extensions;
using Claims.Infrastructure.Configurations;
using Claims.Middleware;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.Configure<CosmosDbConfig>(configuration.GetSection("CosmosDb"));

builder.Services.RegisterSwagger();

builder.Services.AddCosmosDb(configuration);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.RegisterRepositories();

builder.Services.AddApplicationServices();

builder.Services.RegisterHttpClints(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.EnsureCosmosDbIsCreated();

app.Run();