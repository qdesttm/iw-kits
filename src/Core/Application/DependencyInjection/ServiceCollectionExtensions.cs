using IWKits.Core.Application.Services;
using IWKits.Core.Application.Services.Hosted;
using Microsoft.Extensions.DependencyInjection;

namespace IWKits.Core.Application.DependencyInjection;

/// <summary>
/// Extends <see cref="IServiceCollection"/> with application services.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds application services to the specified <see cref="IServiceCollection"/>.
	/// </summary>
	/// <param name="services">The service collection to add the application services to.</param>
	/// <returns>The original <see cref="IServiceCollection"/> instance for chaining.</returns>
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(options =>
			options.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

		services.AddSingleton<IPasswordCryptoService, PasswordCryptoService>();
		services.AddSingleton<ISecurityTokensService, SecurityTokensService>();
		services.AddSingleton<IUserSessionService, UserSessionService>();

		services.AddSingleton<IGeoLocationResolver, GeoLocationResolver>();
		services.AddSingleton<IServiceAreaLocator, ServiceAreaLocator>();

		services.AddSingleton<IOrderTaxProcessor, OrderTaxProcessor>();
		services.AddSingleton<ITaxCalculatorService, TaxCalculatorService>();
		services.AddSingleton<ITaxRateProvider, TaxRateProvider>();

		services.AddHostedService<ServiceAreaCacheRefresher>();

		return services;
	}
}