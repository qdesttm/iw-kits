using System;

namespace IWKits.Core.Common.Models;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Username"></param>
/// <param name="PasswordHash"></param>
/// <param name="Role"></param>
public sealed record User(Guid Id, string Username, string PasswordHash, string Role);