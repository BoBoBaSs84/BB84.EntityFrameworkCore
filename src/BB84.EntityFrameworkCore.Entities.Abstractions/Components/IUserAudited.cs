// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Defines an interface for tracking audit information about the creator and last editor of a data row.
/// </summary>
/// <typeparam name="TCreator">The type representing the creator of the data row.</typeparam>
/// <typeparam name="TEditor">The type representing the last editor of the data row.</typeparam>
public interface IUserAudited<TCreator, TEditor>
{
	/// <summary>
	/// Gets or sets the creator of the current data row.
	/// </summary>
	TCreator Creator { get; set; }

	/// <summary>
	/// Gets or sets the last editor of the current data row.
	/// </summary>
	TEditor Editor { get; set; }
}

/// <inheritdoc cref="IUserAudited{TCreator, TEditor}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IUserAudited : IUserAudited<string, string?>
{ }
