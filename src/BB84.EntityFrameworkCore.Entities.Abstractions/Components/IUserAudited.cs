// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Defines a contract for entities that track auditing information, including
/// the creator and last editor of the entity.
/// </summary>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEditor">The type representing the last editor of the entity.</typeparam>
public interface IUserAudited<TCreator, TEditor>
	where TCreator : notnull
{
	/// <summary>
	/// Gets or sets the creator of the entity.
	/// </summary>
	TCreator Creator { get; set; }

	/// <summary>
	/// Gets or sets the last editor of the entity.
	/// </summary>
	TEditor Editor { get; set; }
}

/// <inheritdoc cref="IUserAudited{TCreator, TEditor}"/>
public interface IUserAudited : IUserAudited<string, string?>
{ }
