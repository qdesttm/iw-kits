using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using IWKits.Api.Settings;
using IWKits.Api.Database;
using IWKits.Api.Services;
using FluentValidation;
using MongoDB.Driver;
using System.Text;

namespace IWKits.Api;

public static class Program
{
	private static async Task Main()
	{
		var builder = WebApplication.CreateBuilder();

		builder.AddApplicationServices();
		var application = builder.Build();

		application.MapApplicationEndpoints();
		application.UseApplicationPipelines();

		await application.RunAsync();
	}

	private static void AddApplicationServices(this WebApplicationBuilder builder)
	{
		var cfg = builder.Configuration;
		var env = builder.Environment;
		var srv = builder.Services;

		srv.AddControllers();
		srv.AddAuthorization();
		srv.AddEndpointsApiExplorer();
		srv.AddMemoryCache();
		srv.AddSwaggerGen();

		builder.Services.AddValidatorsFromAssemblyContaining
			<Features.AuthLogin.AuthLoginRequestValidator>();
		builder.Services.AddValidatorsFromAssemblyContaining
			<Features.AuthRefresh.AuthRefreshRequestValidator>();
		builder.Services.AddValidatorsFromAssemblyContaining
			<Features.AuthRegister.AuthRegisterRequestValidator>();
		builder.Services.AddValidatorsFromAssemblyContaining
			<Features.CreateOrder.CreateOrderRequestValidator>();

		builder.Services
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer();

		builder.Services
			.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
			.Configure<IOptions<SecuritySettings>>((options, securitySettings) =>
			{
				var jwtKey = Utils.GetRequiredEnv("JWT_KEY");
				var settings = securitySettings.Value;

				var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = settings.JwtIssuer,

					ValidateAudience = true,
					ValidAudience = settings.JwtAudience,

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = signingKey
				};
			});

		srv.AddOptions<MongoDBSettings>()
			.Bind( cfg.GetSection(MongoDBSettings.SectionName) )
			.ValidateDataAnnotations()
			.ValidateOnStart();

		srv.AddOptions<ConstraintSettings>()
			.Bind( cfg.GetSection(ConstraintSettings.SectionName) )
			.ValidateDataAnnotations()
			.ValidateOnStart();

		srv.AddOptions<ServiceSettings>()
			.Bind( cfg.GetSection(ServiceSettings.SectionName) )
			.ValidateDataAnnotations()
			.ValidateOnStart();

		srv.AddOptions<SecuritySettings>()
			.Bind( cfg.GetSection(SecuritySettings.SectionName) )
			.ValidateDataAnnotations()
			.ValidateOnStart();

		srv.AddOptions<SessionSettings>()
			.Bind( cfg.GetSection(SessionSettings.SectionName) )
			.ValidateDataAnnotations()
			.ValidateOnStart();

		srv.AddSingleton<IMongoClient>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

			string username = Utils.GetRequiredEnv("DB_CONNECTION_USERNAME");
			string password = Utils.GetRequiredEnv("DB_CONNECTION_PASSWORD");

			string hostname = Utils.GetRequiredEnv("DB_CONNECTION_HOSTNAME");
			string hostport = Utils.GetRequiredEnv("DB_CONNECTION_HOSTPORT");

			var parameters = $"?authSource={settings.AuthSource}&maxPoolSize={settings.MaxPoolSize}";

			return new MongoClient(
				$"mongodb://{username}:{password}@{hostname}:{hostport}/{parameters}"
			);
		});

		srv.AddSingleton<AuthDatabaseContext>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

			var client = sp.GetRequiredService<IMongoClient>();
			return new(client, settings.Databases.Auth);
		});

		srv.AddSingleton<CoreDatabaseContext>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

			var client = sp.GetRequiredService<IMongoClient>();
			return new(client, settings.Databases.Core);
		});

		srv.AddSingleton<DataDatabaseContext>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

			var client = sp.GetRequiredService<IMongoClient>();
			return new(client, settings.Databases.Data);
		});

		srv.AddSingleton<IOrderProcessService, OrderProcessService>();
		srv.AddSingleton<IGeoLocationService, GeoLocationService>();

		srv.AddSingleton<ITaxApplierService>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<ServiceSettings>>().Value;

			return ( settings.TaxApplier == "fake" ) ? new TaxApplierFakeService()
				: new TaxApplierService();
		});

		srv.AddSingleton<ISecurityService>((sp) =>
		{
			var securitySettings = sp.GetRequiredService<IOptions<SecuritySettings>>().Value;
			var sessionSettings = sp.GetRequiredService<IOptions<SessionSettings>>().Value;
			var jwtKey = Utils.GetRequiredEnv("JWT_KEY");

			return new SecurityService(securitySettings, sessionSettings, jwtKey);
		});

		srv.AddSingleton<ISessionService>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<SessionSettings>>().Value;
			var securityService = sp.GetRequiredService<ISecurityService>();
			var authDatabase = sp.GetRequiredService<AuthDatabaseContext>();

			return new SessionService(securityService, settings, authDatabase);
		});

		srv.AddHostedService<GeoLocationCacheWarmupService>();
	}

	private static void MapApplicationEndpoints(this WebApplication application)
	{
		var apiV1 = application.MapGroup("api/v1");

		Features.AuthRegister.AuthRegisterEndpoint.MapAuthRegisterEndpoint(apiV1);
		Features.AuthRefresh.AuthRefreshEndpoint.MapAuthRefreshEndpoint(apiV1);
		Features.AuthLogin.AuthLoginEndpoint.MapAuthLoginEndpoint(apiV1);

		Features.CreateOrder.CreateOrderEndpoint.MapCreateOrderEndpoint(apiV1);
		Features.GetOrders.GetOrdersEndpoint.MapGetOrdersEndpoint(apiV1);
		Features.ImportOrders.ImportOrdersEndpoint.MapImportOrdersEndpoint(apiV1);
	}

	private static void UseApplicationPipelines(this WebApplication application)
	{
		var environment = application.Environment;

		if ( environment.IsDevelopment() )
		{
			application.UseSwaggerUI();
			application.UseSwagger();
		}

		application.UseHttpsRedirection();
		application.UseAuthorization();
	}
}