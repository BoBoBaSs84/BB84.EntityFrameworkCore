using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class JobType : CompositeModel
{
	public string? Color {  get; set; }
}
