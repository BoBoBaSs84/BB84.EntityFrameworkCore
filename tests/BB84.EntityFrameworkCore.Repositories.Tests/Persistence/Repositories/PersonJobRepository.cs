﻿using BB84.EntityFrameworkCore.Repositories.Tests.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Repositories;

internal sealed class PersonJobRepository(ITestDbContext testContext) : GenericRepository<PersonJobEntity>(testContext)
{ }
