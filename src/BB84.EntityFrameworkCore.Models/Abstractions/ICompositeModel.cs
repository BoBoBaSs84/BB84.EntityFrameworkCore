using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The composite model interface.
/// </summary>
/// <inheritdoc cref="IConcurrency"/>
/// <inheritdoc cref="IAudited{TCreated, TModified}"/>
public interface ICompositeModel<TCreated, TModified> : IConcurrency, IAudited<TCreated, TModified>
{ }

/// <inheritdoc/>
public interface ICompositeModel : ICompositeModel<string, string?>
{ }