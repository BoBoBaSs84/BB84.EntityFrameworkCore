using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Entities;

public sealed class PersonJob : AuditedCompositeEntity
{
	public Guid PersonId { get; set; }
	public Guid JobId { get; set; }

	public Person Person { get; set; } = default!;
	public Job Job { get; set; } = default!;
}
