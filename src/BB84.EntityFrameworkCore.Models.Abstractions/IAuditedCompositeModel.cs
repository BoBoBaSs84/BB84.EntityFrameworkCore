using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The interface for the audited composite models.
/// </summary>
/// <inheritdoc cref="IConcurrency"/>
/// <inheritdoc cref="IUserAudited{TCreator, TEditor}"/>
public interface IAuditedCompositeModel<TCreator, TEditor> : IConcurrency, IUserAudited<TCreator, TEditor>
{ }

/// <inheritdoc cref="IAuditedCompositeModel{TCreator, TEditor}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedCompositeModel : IAuditedCompositeModel<string, string?>
{ }
