using System;
using IWKits.Core.Common.Messages;

namespace IWKits.Core.Application.Modules.Orders.Models;

/// <summary>
/// Response containing the created order identifier.
/// </summary>
/// <param name="Data">The unique identifier of the created order record.</param>
public sealed record AddOrderResponse(Guid Data) : Response<Guid>(Data);
