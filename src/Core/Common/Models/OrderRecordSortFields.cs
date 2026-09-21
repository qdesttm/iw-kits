namespace IWKits.Core.Common.Models;

/// <summary>
/// Specifies the available fields for sorting order records.
/// </summary>
public enum OrderRecordSortFields
{
	/// <summary>
	/// Sort by the order subtotal amount before taxes.
	/// </summary>
	Subtotal,

	/// <summary>
	/// Sort by the composite tax rate percentage.
	/// </summary>
	CompositeTaxRate,

	/// <summary>
	/// Sort by the calculated tax amount value.
	/// </summary>
	TaxAmount,

	/// <summary>
	/// Sort by the final total amount including taxes.
	/// </summary>
	TotalAmount,

	/// <summary>
	/// Sort by the date and time when the record was created.
	/// </summary>
	Timestamp
}