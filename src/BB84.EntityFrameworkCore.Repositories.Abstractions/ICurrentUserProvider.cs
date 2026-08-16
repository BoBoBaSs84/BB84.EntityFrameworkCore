// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines a contract for supplying the identity to record in the audit columns of an entity.
/// </summary>
/// <remarks>
/// The identity source is application specific, which is why the library asks for it rather
/// than deciding it. A console application or service typically reports the operating system
/// user, while a web application resolves the identity from the current request.
/// </remarks>
/// <typeparam name="TUser">The type representing the user.</typeparam>
public interface ICurrentUserProvider<out TUser>
{
	/// <summary>
	/// Returns the user to record for the operation currently being saved.
	/// </summary>
	/// <returns>The current user.</returns>
	TUser GetCurrentUser();
}

/// <inheritdoc cref="ICurrentUserProvider{TUser}"/>
/// <remarks>
/// The convenience alias for the default <see cref="string"/> based audit columns.
/// </remarks>
public interface ICurrentUserProvider : ICurrentUserProvider<string>
{ }
