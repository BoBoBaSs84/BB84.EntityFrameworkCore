using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// The interface for the audited composite models.
/// </summary>
/// <inheritdoc cref="IConcurrency"/>
/// <inheritdoc cref="IUserAudited{TCreator, TEditor}"/>
public interface IAuditedCompositeEntity<TCreator, TEditor> : IConcurrency, IUserAudited<TCreator, TEditor>
{ }

/// <inheritdoc cref="IAuditedCompositeEntity{TCreator, TEditor}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedCompositeEntity : IAuditedCompositeEntity<string, string?>
{ }
