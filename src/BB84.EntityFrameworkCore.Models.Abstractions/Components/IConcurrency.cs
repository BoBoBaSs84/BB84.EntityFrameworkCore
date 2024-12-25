namespace BB84.EntityFrameworkCore.Models.Abstractions.Components;

/// <summary>
/// The interface for concurrency based components.
/// </summary>
public interface IConcurrency
{
	/// <summary>
	/// The current timestamp or row version of the data row.
	/// </summary>
	byte[] Timestamp { get; }
}
