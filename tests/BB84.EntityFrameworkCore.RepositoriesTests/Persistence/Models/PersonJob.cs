using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class PersonJob : AuditedCompositeModel
{
	public Guid PersonId { get; set; }
	public Guid JobId { get; set; }

	public Person Person { get; set; } = default!;
	public Job Job { get; set; } = default!;
}
