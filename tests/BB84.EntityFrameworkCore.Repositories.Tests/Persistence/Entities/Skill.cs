using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class Skill : FullAuditedEntity
{
	public required string Name { get; set; }
	public required string Description { get; set; }
	public bool IsCritical { get; set; }

	public ICollection<Person>? Persons { get; set; }
	public ICollection<Job>? Jobs { get; set; }
}
