using IWKits.Core.Common.Messages;
using IWKits.Core.Common.Models;

namespace IWKits.Core.Application.Modules.Users.Models;

/// <summary>
/// API response envelope containing user profile data.
/// </summary>
/// <param name="Data">The secure user profile profile data.</param>
public sealed record UserProfileResponse(UserProfile Data) : Response<UserProfile>(Data);