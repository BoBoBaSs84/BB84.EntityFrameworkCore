using BB84.EntityFrameworkCore.Repositories;
using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

internal sealed class SkillRepository(IDbContext dbContext) : IdentityRepository<Skill>(dbContext)
{ }
