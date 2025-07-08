using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BB84.EntityFrameworkCore.Entities.Abstractions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represents an entity contract within a hierarchy, providing access to its unique
/// hierarchical identifier.
/// </summary>
public interface IHierarchyEntity : IConcurrency
{
	/// <summary>
	/// Gets or sets the unique identifier for the entity within its hierarchy.
	/// </summary>
	HierarchyId Id { get; set; }
}
