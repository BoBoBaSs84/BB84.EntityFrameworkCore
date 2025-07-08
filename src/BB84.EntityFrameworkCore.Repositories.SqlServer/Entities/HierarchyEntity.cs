using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BB84.EntityFrameworkCore.Entities;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represents an entity within a hierarchical structure, identified by a unique <see cref="HierarchyId"/>.
/// </summary>
/// <remarks>
/// This abstract class serves as a base for entities that are part of a hierarchy.
/// It provides a unique identifier and a timestamp for concurrency control or version tracking.
/// </remarks>
public abstract class HierarchyEntity : IHierarchyEntity
{
	/// <inheritdoc/>
	public required HierarchyId Id { get; set; }

	/// <inheritdoc/>
	public required byte[] Timestamp { get; set; }
}
