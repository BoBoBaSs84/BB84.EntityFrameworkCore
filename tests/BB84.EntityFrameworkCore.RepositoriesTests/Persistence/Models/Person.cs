using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class Person : AuditedModel
{
	public string FirstName { get; set; }
	public string? MiddleName { get; set; }
	public string LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public decimal Salary { get; set; }
	public string? Settings { get; set; }

	public PersonType Type { get; set; }
	public ICollection<PersonJob>? PersonJobs { get; set; }
}
