using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The enumerator model interface.
/// </summary>
/// <inheritdoc/>
public interface IEnumeratorModel : IIdentityModel<int>, IEnumerator, ISoftDeletable
{ }
