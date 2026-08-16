// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Abstractions;

namespace BB84.EntityFrameworkCore.Repositories.Interceptors;

/// <summary>
/// A current user provider that reports the operating system user running the process,
/// in the form <c>MachineName\UserName</c>.
/// </summary>
/// <remarks>
/// Suitable for desktop applications, services and background jobs, where the process
/// identity is the identity worth auditing. A web application should implement
/// <see cref="ICurrentUserProvider"/> over the current request instead, because the process
/// identity is the same for every user.
/// </remarks>
public sealed class EnvironmentUserProvider : ICurrentUserProvider
{
	/// <inheritdoc/>
	public string GetCurrentUser()
		=> $"{Environment.MachineName}\\{Environment.UserName}";
}
