using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWKits.Core.Application.Mappers;
using IWKits.Core.Application.Modules.Orders.Models;
using IWKits.Core.Application.Modules.Orders.Models.Requests;
using IWKits.Core.Common.Models;
using IWKits.Core.Data;
using IWKits.Core.Data.Entities;
using MediatR;
using MongoDB.Driver;
using SortDirection = IWKits.Core.Common.Models.SortDirection;

namespace IWKits.Core.Application.Modules.Orders.Handlers;

internal sealed class ListOrdersHandler(
	MongoDbContext dbContext) :
	IRequestHandler<ListOrdersRequest, ListOrdersResponse>
{
	private const int DefaultPageSize = 24;
	private const int MaxPageSize = 128;

	public async Task<ListOrdersResponse> Handle(
		ListOrdersRequest request,
		CancellationToken cancellationToken)
	{
		var filter = CreateFilter(request);

		var totalCount = await dbContext.OrderRecords.CountDocumentsAsync(filter, null, cancellationToken);

		int pageSize = Math.Max(1, Math.Min(MaxPageSize, request.Size ?? DefaultPageSize));
		int page = Math.Max(1, request.Page ?? 1);

		if (totalCount == 0)
			return new ListOrdersResponse([], page, pageSize, 0);

		long totalPages = (long) Math.Ceiling((double) totalCount / pageSize);

		if (page > totalPages)
			return new ListOrdersResponse([], page, pageSize, (int) totalCount);

		var sortBy = request.SortBy ?? OrderRecordSortFields.Timestamp;
		var direction = request.SortDirection ?? SortDirection.Descending;

		var sorter = CreateSorter(sortBy, direction);
		int skipCount = (page - 1) * pageSize;

		var entities = await dbContext.OrderRecords
			.Find(filter)
			.Sort(sorter)
			.Skip(skipCount)
			.Limit(pageSize)
			.ToListAsync(cancellationToken);

		var domainItems = entities
			.Select(OrderRecordMapper.ToOrderRecord)
			.ToList();

		return new ListOrdersResponse(domainItems, page, pageSize, (int) totalCount);
	}

	private static FilterDefinition<OrderRecordEntity> CreateFilter(ListOrdersRequest request)
	{
		var builder = Builders<OrderRecordEntity>.Filter;
		var filters = new List<FilterDefinition<OrderRecordEntity>>();

		if (request.After is not null)
		{
			filters.Add(builder.Gte(x => x.Timestamp, request.After));
		}

		if (request.Before is not null)
		{
			filters.Add(builder.Lte(x => x.Timestamp, request.Before));
		}

		if (request.MinTotalAmount is not null)
		{
			filters.Add(builder.Gte(x => x.TotalAmount, request.MinTotalAmount));
		}

		if (request.MaxTotalAmount is not null)
		{
			filters.Add(builder.Lte(x => x.TotalAmount, request.MaxTotalAmount));
		}

		return filters.Count > 0 ? builder.And(filters) : builder.Empty;
	}

	private static SortDefinition<OrderRecordEntity> CreateSorter(OrderRecordSortFields field, SortDirection? direction)
	{
		direction ??= SortDirection.Descending;

		var isDesc = direction == SortDirection.Descending;
		var builder = Builders<OrderRecordEntity>.Sort;

		var sortDefinition = field switch
		{
			OrderRecordSortFields.Subtotal => isDesc
				? builder.Descending(x => x.Subtotal)
				: builder.Ascending(x => x.Subtotal),

			OrderRecordSortFields.CompositeTaxRate => isDesc
				? builder.Descending(x => x.CompositeTaxRate)
				: builder.Ascending(x => x.CompositeTaxRate),

			OrderRecordSortFields.TaxAmount => isDesc
				? builder.Descending(x => x.TaxAmount)
				: builder.Ascending(x => x.TaxAmount),

			OrderRecordSortFields.TotalAmount => isDesc
				? builder.Descending(x => x.TotalAmount)
				: builder.Ascending(x => x.TotalAmount),

			OrderRecordSortFields.Timestamp or _ => isDesc
				? builder.Descending(x => x.Timestamp)
				: builder.Ascending(x => x.Timestamp)
		};

		return builder.Combine(sortDefinition, builder.Ascending(x => x.Id));
	}
}