using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BB84.EntityFrameworkCore.Entities.Abstractions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represents a hierarchical entity that is fully audited, including user and time-based auditing.
/// </summary>
/// <typeparam name="TCreator">The type of the entity representing the creator.</typeparam>
/// <typeparam name="TEdited">The type of the entity representing the last editor.</typeparam>
public interface IFullAuditedHierarchyEntity<TCreator, TEdited> : IHierarchyEntity, IUserAudited<TCreator, TEdited>, ITimeAudited
		where TCreator : notnull
{ }

/// <summary>
/// Represents a hierarchical entity that is fully audited, including user and time-based auditing.
/// It uses <see cref="string"/> as the type for the creator and a <see cref="string"/>
/// for the editor that can be <see langword="null"/>.
/// </summary>
/// <inheritdoc cref="IFullAuditedEntity{TCreator, TEdited}"/>
public interface IFullAuditedHierarchyEntity : IFullAuditedHierarchyEntity<string, string?>
{ }
