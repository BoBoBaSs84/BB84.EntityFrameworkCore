using BB84.EntityFrameworkCore.Repositories;
using BB84.EntityFrameworkCore.RepositoriesTests.Abstractions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

internal sealed class JobRepository(ITestDbContext testContext) : IdentityRepository<Job>(testContext)
{ }
