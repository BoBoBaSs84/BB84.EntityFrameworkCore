using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class Trait : AuditedModel
{
	public string Description { get; set; }
	public int Severety { get; set; }

	public ICollection<PersonTrait>? PersonTraits { get; set; }
}
