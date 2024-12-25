using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The interface for the audited models.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IUserAudited{TCreator, TEdited}"/>
public interface IAuditedModel<TKey, TCreator, TEdited> : IIdentityModel<TKey>, IUserAudited<TCreator, TEdited> where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IAuditedModel{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedModel<TKey> : IAuditedModel<TKey, string, string?>, IUserAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IAuditedModel{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IAuditedModel<TCreator, TEdited> : IAuditedModel<Guid, TCreator, TEdited>, IIdentityModel
{ }

/// <inheritdoc cref="IAuditedModel{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedModel : IAuditedModel<Guid, string, string?>, IIdentityModel, IUserAudited
{ }
