using System.IO;
using MediatR;

namespace IWKits.Core.Application.Modules.Orders.Models.Requests;

/// <summary>
/// Request to bulk import orders from a CSV stream with parallel tax processing.
/// </summary>
/// <param name="ContentStream">The open readable stream containing the CSV data.</param>
public sealed record ImportOrdersRequest(Stream ContentStream) : IRequest<ImportOrdersResponse>;
