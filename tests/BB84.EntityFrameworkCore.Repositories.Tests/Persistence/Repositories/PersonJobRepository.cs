using BB84.EntityFrameworkCore.Repositories;
using BB84.EntityFrameworkCore.RepositoriesTests.Abstractions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Entities;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

internal sealed class PersonJobRepository(ITestDbContext testContext) : GenericRepository<PersonJob>(testContext)
{ }
