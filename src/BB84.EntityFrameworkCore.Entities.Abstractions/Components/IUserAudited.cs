// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// The interface for user audited components.
/// </summary>
/// <typeparam name="TCreator">The type of the creator column.</typeparam>
/// <typeparam name="TEditor">The type of the editor column.</typeparam>
public interface IUserAudited<TCreator, TEditor>
{
	/// <summary>
	/// The initial creator of the data row.
	/// </summary>
	TCreator Creator { get; set; }

	/// <summary>
	/// The last editor of the data row.
	/// </summary>
	TEditor Editor { get; set; }
}

/// <inheritdoc cref="IUserAudited{TCreator, TEditor}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IUserAudited : IUserAudited<string, string?>
{ }
