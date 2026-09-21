namespace IWKits.Core.Common.Models;

/// <summary>
/// Represents a specific legal tax jurisdiction applied to an order within the application domain.
/// </summary>
/// <param name="Name">The distinctive name of the tax jurisdiction.</param>
/// <param name="Type">The administrative level type of the jurisdiction.</param>
/// <param name="Rate">The specific tax rate enforced by this jurisdiction.</param>
public sealed record OrderTaxJurisdiction(string Name, JurisdictionType Type, decimal Rate);