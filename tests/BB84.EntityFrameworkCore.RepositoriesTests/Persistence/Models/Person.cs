using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class Person : AuditedModel
{
	public required string FirstName { get; set; }
	public string? MiddleName { get; set; }
	public required string LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public decimal Salary { get; set; }
	public string? Settings { get; set; }

	public PersonType Type { get; set; } = default!;
	public ICollection<PersonJob>? PersonJobs { get; set; }
}
