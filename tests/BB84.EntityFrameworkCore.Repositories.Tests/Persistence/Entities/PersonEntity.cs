// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class PersonEntity : AuditedEntity
{
	public required string FirstName { get; set; }
	public string? MiddleName { get; set; }
	public required string LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public decimal Salary { get; set; }
	public string? Settings { get; set; }

	public PersonTypeEntity Type { get; set; } = default!;
	public ICollection<PersonJobEntity>? PersonJobs { get; set; }
	public ICollection<SkillEntity>? Skills { get; set; }
}
