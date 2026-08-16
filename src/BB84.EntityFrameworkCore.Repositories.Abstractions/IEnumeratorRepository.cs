// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines a repository interface for managing entities of type <see cref="IEnumeratorEntity{TKey}"/>
/// with a primary key of type <typeparamref name="TKey"/>.
/// </summary>
/// <remarks>
/// <para>
/// The composition of <see cref="IReadEnumeratorRepository{TEntity, TKey}"/> and the write half
/// inherited from <see cref="IIdentityRepository{TEntity, TKey}"/>. It adds no members of its
/// own.
/// </para>
/// <para>
/// Depend on <see cref="IReadEnumeratorRepository{TEntity, TKey}"/> instead wherever a
/// component only reads.
/// </para>
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
public interface IEnumeratorRepository<TEntity, TKey>
	: IIdentityRepository<TEntity, TKey>, IReadEnumeratorRepository<TEntity, TKey>
	where TEntity : class, IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IEnumeratorRepository{TEntity, TKey}"/>
/// <remarks>
/// The primary key is of type <see cref="int"/>.
/// </remarks>
public interface IEnumeratorRepository<TEntity>
	: IEnumeratorRepository<TEntity, int>, IReadEnumeratorRepository<TEntity>
	where TEntity : class, IEnumeratorEntity
{ }
