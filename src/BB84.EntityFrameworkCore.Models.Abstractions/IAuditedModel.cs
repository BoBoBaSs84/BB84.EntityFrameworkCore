using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The audited model base interface.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IAudited{TCreated, TModified}"/>
public interface IAuditedModel<TKey, TCreated, TModified> : IIdentityModel<TKey>, IAudited<TCreated, TModified> where TKey : IEquatable<TKey>
{ }

/// <inheritdoc/>
/// <remarks>
/// The primary key is of type <see cref="Guid"/>.
/// </remarks>
public interface IAuditedModel<TCreated, TModified> : IAuditedModel<Guid, TCreated, TModified>, IIdentityModel
{ }

/// <inheritdoc/>
public interface IAuditedModel : IAuditedModel<string, string?>
{ }
