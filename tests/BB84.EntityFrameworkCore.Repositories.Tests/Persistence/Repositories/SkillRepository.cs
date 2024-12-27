using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Repositories;

internal sealed class SkillRepository(IDbContext dbContext) : IdentityRepository<SkillEntity>(dbContext)
{ }
