// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines a generic repository interface for performing CRUD operations and querying
/// entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <remarks>
/// <para>
/// The composition of <see cref="IReadRepository{TEntity}"/> and
/// <see cref="IWriteRepository{TEntity}"/>, and the interface a repository implementation
/// declares. It adds no members of its own.
/// </para>
/// <para>
/// Depend on one of the halves instead wherever a component only reads or only writes; the
/// dependency then states in its type what it is allowed to do.
/// </para>
/// </remarks>
/// <typeparam name="TEntity">
/// The type of the entity for which the repository provides data access functionality.
/// </typeparam>
public interface IGenericRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>
	where TEntity : class
{ }
