using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class Job : AuditedEntity
{
	public required string Name { get; set; }
	public required string Description { get; set; }

	public ICollection<PersonJob>? PersonJobs { get; set; }
	public ICollection<Skill>? Requirements { get; set; }
}
