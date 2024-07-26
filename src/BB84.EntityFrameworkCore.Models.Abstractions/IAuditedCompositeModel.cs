using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The audited composite model interface.
/// </summary>
/// <inheritdoc cref="IConcurrency"/>
/// <inheritdoc cref="IAudited{TCreated, TModified}"/>
public interface IAuditedCompositeModel<TCreated, TModified> : IConcurrency, IAudited<TCreated, TModified>
{ }

/// <inheritdoc/>
public interface IAuditedCompositeModel : IAuditedCompositeModel<string, string?>
{ }
