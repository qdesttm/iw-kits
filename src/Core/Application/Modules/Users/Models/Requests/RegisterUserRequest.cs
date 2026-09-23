using MediatR;

namespace IWKits.Core.Application.Modules.Users.Models.Requests;

/// <summary>
/// Represents a request to register a new user account within the system.
/// </summary>
/// <param name="Username">The unique username requested by the user.</param>
/// <param name="Password">The plain-text password to be securely hashed and stored.</param>
public sealed record RegisterUserRequest(string Username, string Password) : IRequest;