using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BB84.EntityFrameworkCore.Entities.Abstractions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represents an entity within a hierarchy, providing access to its unique hierarchical identifier.
/// </summary>
/// <remarks>
/// This interface is intended to be implemented by types that require hierarchical organization.
/// The <see cref="Id"/> property uniquely identifies the entity's position within the hierarchy.
/// </remarks>
public interface IHierarchyEntity : IConcurrency
{
	/// <summary>
	/// Gets or sets the unique identifier representing the hierarchy level of the current entity.
	/// </summary>
	HierarchyId Id { get; set; }
}
