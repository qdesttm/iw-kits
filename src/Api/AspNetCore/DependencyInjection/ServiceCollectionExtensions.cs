using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using IWKits.Api.AspNetCore.Infrastructure.Errors;
using IWKits.Core.Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IWKits.Api.AspNetCore.DependencyInjection;

/// <summary>
/// Provides extension methods to register and configure core infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds core infrastructure, including security, versioning, serialization, and error handling.
	/// </summary>
	/// <param name="services">The service collection instance.</param>
	/// <returns>The modified service collection for chaining.</returns>
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		services.AddMemoryCache();

		services.AddSwaggerGen(options =>
		{
			options.DescribeAllParametersInCamelCase();
		});

		services.AddExceptionHandler<ServiceExceptionHandler>();
		services.AddProblemDetails();

		services.AddMediatR(options =>
			options.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

		services.AddValidatorsFromAssemblyContaining<ServiceExceptionHandler>(includeInternalTypes: true);

		services.AddVersioning();
		services.AddJwtAuthentication();

		services.ConfigureNetworkHeaders();
		services.ConfigureJsonOptions();

		return services;
	}

	/// <summary>
	/// Configures API versioning using URL segments.
	/// </summary>
	/// <param name="services">The service collection instance.</param>
	/// <returns>The modified service collection for chaining.</returns>
	private static IServiceCollection AddVersioning(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();

		var builder = services.AddApiVersioning(options =>
		{
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.ApiVersionReader = new UrlSegmentApiVersionReader();
		});

		builder.AddApiExplorer(options =>
		{
			options.GroupNameFormat = "'v'VVV";
			options.SubstituteApiVersionInUrl = true;
		});

		return services;
	}

	/// <summary>
	/// Configures JWT-based authentication and token validation settings.
	/// </summary>
	/// <param name="services">The service collection instance.</param>
	/// <returns>The modified service collection for chaining.</returns>
	private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer();

		services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
			.Configure<IOptions<SecurityTokensOptions>>((options, securityOptions) =>
			{
				var jwtKeyBytes = Encoding.UTF8.GetBytes(securityOptions.Value.JwtKey);

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = securityOptions.Value.JwtIssuer,

					ValidateAudience = true,
					ValidAudience = securityOptions.Value.JwtAudience,

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes),

					ValidateLifetime = true
				};
			});

		services.AddAuthorization();

		return services;
	}

	/// <summary>
	/// Configures x-forwarded network headers for reverse proxy compatibility.
	/// </summary>
	/// <param name="services">The service collection instance.</param>
	/// <returns>The modified service collection for chaining.</returns>
	private static IServiceCollection ConfigureNetworkHeaders(this IServiceCollection services)
	{
		services.Configure<ForwardedHeadersOptions>(options =>
		{
			options.ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
		});

		return services;
	}

	/// <summary>
	/// Configures HTTP JSON serialization rules and policies for Minimal API and Swagger metadata.
	/// </summary>
	/// <param name="services">The service collection instance.</param>
	/// <returns>The modified service collection for chaining.</returns>
	private static IServiceCollection ConfigureJsonOptions(this IServiceCollection services)
	{
		services.ConfigureHttpJsonOptions(options =>
		{
			options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
			options.SerializerOptions.WriteIndented = false;
			options.SerializerOptions.PropertyNameCaseInsensitive = true;
			options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			options.SerializerOptions.AllowOutOfOrderMetadataProperties = true;
			options.SerializerOptions.Converters.Add(
				new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
		});

		services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
		{
			options.JsonSerializerOptions.Converters.Add(
				new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
		});

		return services;
	}
}