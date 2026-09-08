using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
using IWKits.Api.Entities;
using IWKits.Api.Services;
using IWKits.Api.Database;
using IWKits.Api.Settings;
using System.Threading;
using MongoDB.Driver;
using System.Linq;
using System.IO;
using CsvHelper;

namespace IWKits.Api.Features.ImportOrders;

public static class ImportOrdersEndpoint
{
	public const string Endpoint = "orders/import";

	public static void MapImportOrdersEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, ImportOrdersHandlerAsync).WithName("ImportOrders")
			.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
			.Accepts<IFormFile>("multipart/form-data")
			.Produces<ImportOrdersRespond>(200)
			.Produces(400)
			.DisableAntiforgery();
	}

	private static async Task<IResult> ImportOrdersHandlerAsync
	(
		[FromServices] IOptions<ConstraintSettings> constraints,
		[FromServices] IOrderProcessService orderProcess,
		[FromServices] DataDatabaseContext dataDatabase,
		HttpContext httpContext, IFormFile file, CancellationToken ct)
	{
		if ( file is null || file.Length == 0 )
		{
			return Results.BadRequest("File is empty or missing.");
		}

		int importChunkSize = constraints.Value.ImportChunkSize;

		var insertOptions = new InsertManyOptions() { IsOrdered = false };

		var errors = new List<string>();
		int importedTotal = 0;

		using var reader = new StreamReader( file.OpenReadStream() );
		using var csv    = new CsvReader(reader, CultureInfo.InvariantCulture);

		var chunks = csv.GetRecordsAsync<RawOrderInfo>(ct).Chunk(importChunkSize);
		var options = new ParallelOptions() { CancellationToken = ct, MaxDegreeOfParallelism = 2 };

		await Parallel.ForEachAsync(chunks, options, async (chunk, token) =>
		{
			var tasks = chunk.Select(orderProcess.ProcessAsync);
			var toInsert = new List<OrderInfo>(importChunkSize);

			foreach (var result in await Task.WhenAll(tasks))
			{
				if ( result.HasError )
				{
					errors.Add(result.ErrorMessage);
				}
				else
				{
					toInsert.Add(result.OrderInfo);
				}
			}

			if ( toInsert.Count > 0 )
			{
				await dataDatabase.Orders.InsertManyAsync(toInsert, insertOptions, token);
				Interlocked.Add(ref importedTotal, toInsert.Count);
			}
		});

		var respond = new ImportOrdersRespond(importedTotal, errors);
		return Results.Ok(respond);
	}
}