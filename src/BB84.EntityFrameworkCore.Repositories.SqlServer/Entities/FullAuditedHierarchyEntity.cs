using BB84.EntityFrameworkCore.Entities.Abstractions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BB84.EntityFrameworkCore.Entities;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represents an entity that is part of a hierarchical structure and includes full auditing information.
/// </summary>
/// <remarks>
/// This class extends <see cref="AuditedHierarchyEntity{TCreator, TEdited}"/> by adding properties for
/// tracking the creation and modification timestamps. It is designed for scenarios where entities require
/// both hierarchical relationships and detailed audit tracking.
/// </remarks>
/// <typeparam name="TCreator">The type of the entity representing the creator.</typeparam>
/// <typeparam name="TEdited">The type of the entity representing the last editor.</typeparam>
public abstract class FullAuditedHierarchyEntity<TCreator, TEdited> : AuditedHierarchyEntity<TCreator, TEdited>, IFullAuditedHierarchyEntity<TCreator, TEdited>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public DateTime Created { get; set; }

	/// <inheritdoc/>
	public DateTime? Edited { get; set; }
}

/// <summary>
/// Represents an entity that is part of a hierarchical structure and includes full auditing information.
/// It uses <see cref="string"/> as the type for the creator and a <see cref="string"/>
/// for the editor that can be <see langword="null"/>.
/// </summary>
/// <inheritdoc cref="FullAuditedHierarchyEntity{TCreator, TEdited}"/>
public abstract class FullAuditedHierarchyEntity : FullAuditedHierarchyEntity<string, string?>
{ }
