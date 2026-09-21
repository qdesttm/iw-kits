using FluentValidation;
using IWKits.Api.AspNetCore.Hosting;
using IWKits.Api.AspNetCore.Infrastructure;
using IWKits.Core.Application.DependencyInjection;
using IWKits.Core.Application.Options;
using IWKits.Core.Data.DependencyInjection;
using IWKits.Core.Mongodb.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var services = builder.Services;

services.Configure<BackgroundServicesOptions>(configuration.GetSection(BackgroundServicesOptions.SectionName));
services.Configure<SecurityTokensOptions>(configuration.GetSection(SecurityTokensOptions.SectionName));
services.Configure<MongodbOptions>(configuration.GetSection(MongodbOptions.SectionName));
services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SectionName));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddExceptionHandler<ServiceExceptionHandler>();
services.AddProblemDetails();

services.AddMediatR(options =>
    options.RegisterServicesFromAssembly(typeof(Program).Assembly));

services.AddValidatorsFromAssemblyContaining<Program>();

services.AddAuthentication().AddJwtBearer();
services.AddAuthorization();

services.AddDbContext();
services.AddApplication();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

await app.RunAsync();