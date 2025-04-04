// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

public sealed class SkillEntity : FullAuditedEntity
{
	public required string Name { get; set; }
	public required string Description { get; set; }
	public bool IsCritical { get; set; }

	public ICollection<PersonEntity>? Persons { get; set; }
	public ICollection<JobEntity>? Jobs { get; set; }
}
