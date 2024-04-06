using BB84.EntityFrameworkCore.Repositories;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

internal sealed class PersonRepository(DbContext dbContext) : IdentityRepository<Person>(dbContext)
{ }
