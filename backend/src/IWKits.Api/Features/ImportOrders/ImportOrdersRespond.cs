using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace IWKits.Api.Features.ImportOrders;

public sealed record ImportOrdersRespond
(
	[property: JsonPropertyName("imported_total")]
	int ImportedTotal,

	[property: JsonPropertyName("errors")]
	List<string> Errors
);