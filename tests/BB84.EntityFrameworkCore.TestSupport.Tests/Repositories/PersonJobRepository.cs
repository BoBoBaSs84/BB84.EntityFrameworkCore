// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories;
using BB84.EntityFrameworkCore.TestSupport.Abstractions;
using BB84.EntityFrameworkCore.TestSupport.Entities;

namespace BB84.EntityFrameworkCore.TestSupport.Repositories;

public sealed class PersonJobRepository(ITestDbContext testContext) : GenericRepository<PersonJobEntity>(testContext)
{ }
