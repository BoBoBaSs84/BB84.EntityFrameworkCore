using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class Job : AuditedModel
{
	public required string Name { get; set; }
	public required string Description { get; set; }

	public ICollection<PersonJob>? PersonJobs { get; set; }
}
