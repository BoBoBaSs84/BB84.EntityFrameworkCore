﻿// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class PersonJobEntity : AuditedCompositeEntity
{
	public Guid PersonId { get; set; }
	public Guid JobId { get; set; }

	public PersonEntity Person { get; set; } = default!;
	public JobEntity Job { get; set; } = default!;
}
