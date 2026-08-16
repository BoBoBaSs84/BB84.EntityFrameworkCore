// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines a repository interface for managing entities of type <see cref="IIdentityEntity{TKey}"/>
/// with a primary key of type <typeparamref name="TKey"/>.
/// </summary>
/// <remarks>
/// <para>
/// The composition of <see cref="IReadIdentityRepository{TEntity, TKey}"/> and
/// <see cref="IWriteIdentityRepository{TEntity, TKey}"/>. It adds no members of its own.
/// </para>
/// <para>
/// Depend on one of the halves instead wherever a component only reads or only writes.
/// </para>
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
public interface IIdentityRepository<TEntity, TKey>
	: IGenericRepository<TEntity>, IReadIdentityRepository<TEntity, TKey>, IWriteIdentityRepository<TEntity, TKey>
	where TEntity : class, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IIdentityRepository{TEntity, TKey}"/>
/// <remarks>
/// The primary key is of type <see cref="Guid"/>.
/// </remarks>
public interface IIdentityRepository<TEntity>
	: IIdentityRepository<TEntity, Guid>, IReadIdentityRepository<TEntity>, IWriteIdentityRepository<TEntity>
	where TEntity : class, IIdentityEntity
{ }
