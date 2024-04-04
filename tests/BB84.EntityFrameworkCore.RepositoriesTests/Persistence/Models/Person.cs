using BB84.EntityFrameworkCore.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

public sealed class Person : IdentityModel
{
	public string FirstName { get; set; }
	public string? MiddleName { get; set; }
	public string LastName { get; set; }

	public Salutation? Salutation { get; set; }
	public ICollection<PersonTrait>? PersonTraits {  get; set; }
}
