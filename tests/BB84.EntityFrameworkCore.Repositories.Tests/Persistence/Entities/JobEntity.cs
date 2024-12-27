using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class JobEntity : AuditedEntity
{
	public required string Name { get; set; }
	public required string Description { get; set; }

	public ICollection<PersonJobEntity>? PersonJobs { get; set; }
	public ICollection<SkillEntity>? Requirements { get; set; }
}
