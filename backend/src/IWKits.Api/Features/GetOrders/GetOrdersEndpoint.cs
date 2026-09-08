using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IWKits.Api.Settings;
using IWKits.Api.Entities;
using IWKits.Api.Database;
using System.Reflection;
using System.Threading;
using MongoDB.Driver;
using System.Linq;
using System;

namespace IWKits.Api.Features.GetOrders;

public static class GetOrdersEndpoint
{
	public const string Endpoint = "/orders";

	private static readonly Dictionary<string, Expression<Func<OrderInfo, object>>>
		SortExpressions = CreateSortExpressions();

	public static void MapGetOrdersEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapGet(Endpoint, GetOrdersHandlerAsync)
			.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
			.Produces<GetOrdersRespond>(200)
			.WithName("GetOrders");
	}

	private static async Task<IResult> GetOrdersHandlerAsync
	(
		[FromServices] IOptions<ConstraintSettings> constraints,
		[FromServices] DataDatabaseContext dataDatabase,
		[AsParameters] GetOrdersQuery query,
		HttpContext httpContext, CancellationToken ct)
	{
		var options = query.CreateOptions(constraints);
		var filter = options.CreateFilterDefinition();

		List<OrderInfo> items = [];

		var totalCount = await dataDatabase.Orders.CountDocumentsAsync(filter, null, ct);

		var totalPages = (long) Math.Ceiling((double) totalCount / options.PageSize);

		if ( totalCount != 0 && options.Page <= totalPages )
		{
			var sorter = options.CreateSorterDefinition();

			var skipCount = (options.Page - 1) * options.PageSize;
			var takeCount = options.PageSize;

			items = await dataDatabase.Orders
				.Find(filter)
				.Sort(sorter)
				.Skip(skipCount)
				.Limit(takeCount)
				.ToListAsync(ct);
		}

		var respond = new GetOrdersRespond(items, totalCount, totalPages);
		return Results.Ok(respond);
	}

	private static FilterDefinition<OrderInfo> CreateFilterDefinition(this GetOrdersOptions options)
	{
		var filters = new List<FilterDefinition<OrderInfo>>();
		var builder = Builders<OrderInfo>.Filter;

		if ( options.FromDate is not null )
		{
			filters.Add( builder.Gte(x => x.Timestamp, options.FromDate) );
		}

		if ( options.ToDate is not null )
		{
			filters.Add( builder.Lte(x => x.Timestamp, options.ToDate) );
		}

		if ( options.MinTotalAmount is not null )
		{
			filters.Add( builder.Gte(x => x.TotalAmount, options.MinTotalAmount) );
		}

		if ( options.MaxTotalAmount is not null )
		{
			filters.Add( builder.Lte(x => x.TotalAmount, options.MaxTotalAmount) );
		}

		return (filters.Count != 0)
			? builder.And(filters)
			: builder.Empty;
	}


	private static SortDefinition<OrderInfo> CreateSorterDefinition(this GetOrdersOptions options)
	{
		var sorters = new List<SortDefinition<OrderInfo>>();
		var builder = Builders<OrderInfo>.Sort;

		if ( !SortExpressions.TryGetValue(options.SortBy, out var field) )
		{
			field = x => x.Timestamp;
		}

		sorters.Add(options.Descending
			? builder.Descending(field)
			: builder.Ascending (field)
		);

		sorters.Add( builder.Ascending(x => x.Id) );

		return builder.Combine(sorters);
	}

	private static Dictionary<string, Expression<Func<OrderInfo, object>>> CreateSortExpressions()
	{
		Dictionary<string, Expression<Func<OrderInfo, object>>> expressions = [];

		foreach ( var property in typeof(OrderInfo).GetProperties() )
		{
			var attribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
			if ( attribute is null ) continue;

			var parameter = Expression.Parameter(typeof(OrderInfo), "x");
			var eproperty = Expression.Property(parameter, property);
			var conversion = Expression.Convert(eproperty, typeof(object));

			var expression = Expression.Lambda<Func<OrderInfo, object>>(conversion, parameter);
			expressions[attribute.Name] = expression;
		}

		return expressions;
	}

	private static GetOrdersOptions CreateOptions(
		this GetOrdersQuery query, IOptions<ConstraintSettings> constrains)
	{
		var maxPageSize = constrains.Value.RespondMaxPageSize;

		return new GetOrdersOptions
		(
			MinTotalAmount: query.MinTotalAmount,
			MaxTotalAmount: query.MaxTotalAmount,

			FromDate: query.FromDate,
			ToDate  : query.ToDate,

			SortBy: query.SortBy ?? "timestamp",
			Descending: query.Descending ?? true,

			PageSize: Math.Max(1, Math.Min(maxPageSize, query.PageSize ?? 24)),
			Page: Math.Max(1, query.Page ?? 1)
		);
	}
}