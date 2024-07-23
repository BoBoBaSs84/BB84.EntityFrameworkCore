using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Abstractions;

public interface ITestDbContext : IDbContext
{
	DbSet<Person> Persons { get; set; }
	DbSet<PersonType> PersonType { get; set; }
}
