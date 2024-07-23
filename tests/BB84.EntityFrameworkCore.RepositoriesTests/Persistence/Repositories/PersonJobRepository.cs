using BB84.EntityFrameworkCore.Repositories;
using BB84.EntityFrameworkCore.RepositoriesTests.Abstractions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

internal sealed class PersonJobRepository(ITestDbContext testContext) : GenericRepository<PersonJob>(testContext)
{ }
