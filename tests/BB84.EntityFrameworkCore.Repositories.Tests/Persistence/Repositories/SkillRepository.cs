// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Repositories;

internal sealed class SkillRepository(IDbContext dbContext) : IdentityRepository<SkillEntity>(dbContext)
{ }
