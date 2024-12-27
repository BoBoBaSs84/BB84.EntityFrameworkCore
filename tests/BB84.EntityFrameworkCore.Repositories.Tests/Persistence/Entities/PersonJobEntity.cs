using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class PersonJobEntity : AuditedCompositeEntity
{
	public Guid PersonId { get; set; }
	public Guid JobId { get; set; }

	public PersonEntity Person { get; set; } = default!;
	public JobEntity Job { get; set; } = default!;
}
