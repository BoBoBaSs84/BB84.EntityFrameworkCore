using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class SkillEntity : FullAuditedEntity
{
	public required string Name { get; set; }
	public required string Description { get; set; }
	public bool IsCritical { get; set; }

	public ICollection<PersonEntity>? Persons { get; set; }
	public ICollection<JobEntity>? Jobs { get; set; }
}
