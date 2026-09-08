using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using IWKits.Api.Entities;

namespace IWKits.Api.Services;

public sealed record TaxApplyResult
{
	[MemberNotNullWhen(true, nameof(ErrorMessage))]
	public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
	public decimal CompositeTaxRate { get; init; } = 0.0m;

	public decimal TotalAmount { get; init; } = 0.0m;
	public decimal TaxAmount   { get; init; } = 0.0m;

	public TaxBreakdown Breakdown              { get; init; } = new();
	public List<TaxJurisdiction> Jurisdictions { get; init; } = [];

	public string ErrorMessage { get; init; } = string.Empty;

	public static TaxApplyResult Failure(string errorMsg) => new() { ErrorMessage = errorMsg };
}