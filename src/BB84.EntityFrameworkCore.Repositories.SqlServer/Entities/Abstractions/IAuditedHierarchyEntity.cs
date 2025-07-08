using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BB84.EntityFrameworkCore.Entities.Abstractions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represents a hierarchical entity that is audited with information about its creator and last editor.
/// </summary>
/// <typeparam name="TCreator">The type of the creator associated with the entity.</typeparam>
/// <typeparam name="TEdited">The type of the last editor associated with the entity.</typeparam>
public interface IAuditedHierarchyEntity<TCreator, TEdited> : IHierarchyEntity, IUserAudited<TCreator, TEdited>
	where TCreator : notnull
{ }

/// <summary>
/// Represents a hierarchical entity that is audited with information about its creator and last editor.
/// It uses <see cref="string"/> as the type for the creator and a <see cref="string"/>
/// for the editor that can be <see langword="null"/>.
/// </summary>
public interface IAuditedHierarchyEntity : IAuditedHierarchyEntity<string, string?>
{ }
