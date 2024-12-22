using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The interface for full audited entities.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IUserAudited{TCreated, TModified}"/>
/// <inheritdoc cref="ITimeAudited"/>
public interface IFullAuditedModel<TKey, TCreated, TModified> : IIdentityModel<TKey>, IUserAudited<TCreated, TModified>, ITimeAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IFullAuditedModel{TKey, TCreated, TModified}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IFullAuditedModel<TKey> : IFullAuditedModel<TKey, string, string?>, IUserAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IFullAuditedModel{TKey, TCreated, TModified}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IFullAuditedModel<TCreated, TModified> : IFullAuditedModel<Guid, TCreated, TModified>, IIdentityModel
{ }

/// <inheritdoc cref="IFullAuditedModel{TKey, TCreated, TModified}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IFullAuditedModel : IFullAuditedModel<Guid, string, string?>, IIdentityModel, IUserAudited
{ }
