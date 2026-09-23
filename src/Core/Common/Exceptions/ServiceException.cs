using System;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Common.Exceptions;

/// <summary>
/// Represents a base exception for service-level errors that contains structured error information.
/// </summary>
/// <param name="info">The detailed structured information about the error.</param>
public abstract class ServiceException(ErrorInfo info) : Exception(info.Message)
{
	/// <summary>
	/// Gets the structured error information containing the code and description.
	/// </summary>
	public ErrorInfo Info { get; } = info;
}