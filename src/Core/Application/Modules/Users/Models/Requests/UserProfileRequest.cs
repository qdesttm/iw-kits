using System;
using MediatR;

namespace IWKits.Core.Application.Modules.Users.Models.Requests;

/// <summary>
/// Request to retrieve the profile for a specific user.
/// </summary>
/// <param name="UserId">The unique identifier of the user.</param>
public sealed record UserProfileRequest(Guid UserId) : IRequest<UserProfileResponse>;