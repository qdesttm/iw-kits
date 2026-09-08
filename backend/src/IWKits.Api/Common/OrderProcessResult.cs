using System.Diagnostics.CodeAnalysis;
using IWKits.Api.Entities;

namespace IWKits.Api.Services;

public sealed record OrderProcessResult
{
	[MemberNotNullWhen(false, nameof(OrderInfo))]
	[MemberNotNullWhen(true, nameof(ErrorMessage))]
	public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

	public string? ErrorMessage { get; init; }
	public OrderInfo? OrderInfo { get; init; }

	public static OrderProcessResult Success(OrderInfo order) => new() { OrderInfo = order };

	public static OrderProcessResult Failure(string errorMsg) => new() { ErrorMessage = errorMsg };
}