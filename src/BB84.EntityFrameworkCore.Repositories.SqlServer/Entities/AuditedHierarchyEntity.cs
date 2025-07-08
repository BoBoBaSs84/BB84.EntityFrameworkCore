using BB84.EntityFrameworkCore.Entities.Abstractions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BB84.EntityFrameworkCore.Entities;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represents an abstract hierarchical entity that tracks auditing information, including the creator and editor.
/// </summary>
/// <remarks>
/// This class extends <see cref="HierarchyEntity"/> and provides additional auditing properties.
/// It is intended to be used as a base class for entities that require hierarchical structure and auditing
/// capabilities.
/// </remarks>
/// <typeparam name="TCreator">The type of the creator associated with the entity.</typeparam>
/// <typeparam name="TEdited">The type of the last editor associated with the entity.</typeparam>
public abstract class AuditedHierarchyEntity<TCreator, TEdited> : HierarchyEntity, IAuditedHierarchyEntity<TCreator, TEdited>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public TCreator Creator { get; set; } = default!;

	/// <inheritdoc/>
	public TEdited Editor { get; set; } = default!;
}

/// <summary>
/// Represents an abstract hierarchical entity that tracks auditing information, including the creator and editor.
/// The creator is of type <see cref="string"/> and the editor can be <see langword="null"/>.
/// </summary>
/// <inheritdoc cref="AuditedHierarchyEntity{TCreator, TEdited}"/>
public abstract class AuditedHierarchyEntity : AuditedHierarchyEntity<string, string?>, IAuditedHierarchyEntity
{ }
