using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class PersonTrait : CompositeModel
{
	public Guid PersonId {  get; set; }
	public Guid TraitId { get; set; }

	public ICollection<Person>? Persons { get; set; }
	public ICollection<Trait>? Traits { get; set; }
}
