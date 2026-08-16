// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines the write half of a repository for entities of type
/// <see cref="IIdentityEntity{TKey}"/> with a primary key of type <typeparamref name="TKey"/>.
/// </summary>
/// <remarks>
/// Extends <see cref="IWriteRepository{TEntity}"/> with key based bulk operations. Every method
/// declared here executes immediately and bypasses the change tracker, so no interceptor runs
/// for any of them.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
public interface IWriteIdentityRepository<TEntity, TKey> : IWriteRepository<TEntity>
	where TEntity : class, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Deletes the database row for the <typeparamref name="TEntity"/> instance which matches
	/// the <paramref name="id"/> from the database.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </para>
	/// <para>
	/// <b>This deletes permanently, even for soft deletable entity types.</b> Because no save
	/// operation takes place, save changes interceptors never run, and the soft deletable
	/// interceptor therefore cannot turn the deletion into a flag update. The rows are gone.
	/// </para>
	/// </remarks>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int Delete(TKey id);

	/// <summary>
	/// Deletes all database rows for the <typeparamref name="TEntity"/> instances which matches
	/// the <paramref name="ids"/> from the database.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </para>
	/// <para>
	/// <b>This deletes permanently, even for soft deletable entity types.</b> Because no save
	/// operation takes place, save changes interceptors never run, and the soft deletable
	/// interceptor therefore cannot turn the deletion into a flag update. The rows are gone.
	/// </para>
	/// </remarks>
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int Delete(IEnumerable<TKey> ids);

	/// <inheritdoc cref="Delete(TKey)"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> DeleteAsync(TKey id, CancellationToken cancellationToken = default);

	/// <inheritdoc cref="Delete(IEnumerable{TKey})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates the entity identified by the specified identifier with the provided property changes.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>
	/// <param name="id">The unique identifier of the entity to update.</param>
	/// <param name="setPropertyCalls">A lambda expression specifying the properties to update and their new values.</param>
	/// <returns>
	/// The number of entities updated. Typically, this will be 1 if the update is successful,
	/// or 0 if no entity matches the specified <paramref name="id"/>.
	/// </returns>
	int Update(
		TKey id,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls);

	/// <summary>
	/// Updates the entities identified by the specified identifiers with the provided property changes.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>
	/// <param name="ids">The unique identifiers of the entities to update.</param>
	/// <param name="setPropertyCalls">A lambda expression specifying the properties to update and their new values.</param>
	/// <returns>
	/// The number of entities updated. Typically, this will be 1 if the update is successful,
	/// or 0 if no entity matches the specified <paramref name="ids"/>.
	/// </returns>
	int Update(
		IEnumerable<TKey> ids,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls);

	/// <inheritdoc cref="Update(TKey, Action{UpdateSettersBuilder{TEntity}})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> UpdateAsync(
		TKey id,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="Update(IEnumerable{TKey}, Action{UpdateSettersBuilder{TEntity}})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> UpdateAsync(
		IEnumerable<TKey> ids,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
		CancellationToken cancellationToken = default);
}

/// <inheritdoc cref="IWriteIdentityRepository{TEntity, TKey}"/>
/// <remarks>
/// The primary key is of type <see cref="Guid"/>.
/// </remarks>
public interface IWriteIdentityRepository<TEntity> : IWriteIdentityRepository<TEntity, Guid>
	where TEntity : class, IIdentityEntity
{ }
