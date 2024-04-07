namespace BB84.EntityFrameworkCore.Models.Abstractions.Components;

/// <summary>
/// The concurrency interface.
/// </summary>
public interface IConcurrency
{
	/// <summary>
	/// The current timestamp or row version.
	/// </summary>
	byte[] Timestamp { get; }
}
