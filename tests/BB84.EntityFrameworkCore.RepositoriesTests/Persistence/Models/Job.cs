using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class Job : AuditedModel
{
	public string Name { get; set; }
	public string Description { get; set; }

	public ICollection<PersonJob> PersonJobs { get; set; }
}
