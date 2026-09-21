using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Application.Models;
using IWKits.Core.Application.Modules.Orders.Models;
using IWKits.Core.Application.Modules.Orders.Models.Requests;
using IWKits.Core.Application.Services;
using IWKits.Core.Common;
using IWKits.Core.Common.Exceptions;
using IWKits.Core.Common.Models;
using IWKits.Core.Data;
using IWKits.Core.Data.Entities;
using MediatR;
using MongoDB.Driver;

namespace IWKits.Core.Application.Modules.Orders.Handlers;

internal sealed class ImportOrdersHandler(
	IOrderTaxProcessor taxProcessor,
	MongoDbContext dbContext) :
	IRequestHandler<ImportOrdersRequest, ImportOrdersResponse>
{
	private const int ImportChunkSize = 128;

	public async Task<ImportOrdersResponse> Handle(
		ImportOrdersRequest request,
		CancellationToken cancellationToken)
	{
		var insertOptions = new InsertManyOptions { IsOrdered = false };

		var errors = new List<ErrorInfo>();
		int importedTotal = 0;

		using var reader = new StreamReader(request.ContentStream);
		using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

		var chunks = csv.GetRecordsAsync<RawOrderRecord>(cancellationToken)
			.Chunk(ImportChunkSize);

		var parallelOptions = CreateParallelOptions(cancellationToken);

		await Parallel.ForEachAsync(chunks, parallelOptions, async (chunk, ct) =>
		{
			var toInsert = new List<OrderRecordEntity>(chunk.Length);

			var tasks = chunk.Select(async rawOrder =>
			{
				try
				{
					var orderRecord = await taxProcessor.ProcessAsync(rawOrder, ct);
					var recordEntity = orderRecord.ToOrderRecordEntity();

					lock (toInsert)
					{
						toInsert.Add(recordEntity);
					}
				}
				catch (ServiceException ex)
				{
					lock (errors)
					{
						errors.Add(ex.Info);
					}
				}
				catch (Exception ex)
				{
					var systemError = ErrorInfoFactory.CreateOperationFailed(ex.Message);

					lock (errors)
					{
						errors.Add(systemError);
					}
				}
			});

			await Task.WhenAll(tasks);

			if (toInsert.Count > 0)
			{
				await dbContext.OrderRecords.InsertManyAsync(toInsert, insertOptions, ct);
				Interlocked.Add(ref importedTotal, toInsert.Count);
			}
		});

		return new ImportOrdersResponse(importedTotal, errors);
	}

	private static ParallelOptions CreateParallelOptions(CancellationToken cancellationToken)
		=> new() { CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 };
}