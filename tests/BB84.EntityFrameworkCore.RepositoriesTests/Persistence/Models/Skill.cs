using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class Skill : FullAuditedModel
{
	public required string Name { get; set; }
	public required string Description { get; set; }
	public bool IsCritical { get; set; }

	public ICollection<Person>? Persons { get; set; }
	public ICollection<Job>? Jobs { get; set; }
}
