using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class JobTypeEntity : CompositeEntity
{
	public string? Color { get; set; }
}
