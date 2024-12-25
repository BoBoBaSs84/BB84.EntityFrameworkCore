using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The interface for full audited entities.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IUserAudited{TCreator, TEdited}"/>
/// <inheritdoc cref="ITimeAudited"/>
public interface IFullAuditedModel<TKey, TCreator, TEdited> : IIdentityModel<TKey>, IUserAudited<TCreator, TEdited>, ITimeAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IFullAuditedModel{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IFullAuditedModel<TKey> : IFullAuditedModel<TKey, string, string?>, IUserAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IFullAuditedModel{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IFullAuditedModel<TCreator, TEdited> : IFullAuditedModel<Guid, TCreator, TEdited>, IIdentityModel
{ }

/// <inheritdoc cref="IFullAuditedModel{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IFullAuditedModel : IFullAuditedModel<Guid, string, string?>, IIdentityModel, IUserAudited
{ }
