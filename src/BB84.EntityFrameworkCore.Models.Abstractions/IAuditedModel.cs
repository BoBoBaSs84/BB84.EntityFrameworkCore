using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The interface for the audited models.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IUserAudited{TCreated, TModified}"/>
public interface IAuditedModel<TKey, TCreated, TModified> : IIdentityModel<TKey>, IUserAudited<TCreated, TModified> where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IAuditedModel{TKey, TCreated, TModified}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedModel<TKey> : IAuditedModel<TKey, string, string?>, IUserAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IAuditedModel{TKey, TCreated, TModified}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IAuditedModel<TCreated, TModified> : IAuditedModel<Guid, TCreated, TModified>, IIdentityModel
{ }

/// <inheritdoc cref="IAuditedModel{TKey, TCreated, TModified}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedModel : IAuditedModel<Guid, string, string?>, IIdentityModel, IUserAudited
{ }
