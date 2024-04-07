using BB84.EntityFrameworkCore.Models;
using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.ModelsTests;

[TestClass]
public sealed class AuditedCompositeModelTests
{
	[TestMethod]
	public void AuditedCompositeModelTest()
	{
		IAuditedCompositeModel model;

		model = new TestClass();

		Assert.IsNotNull(model);
		Assert.IsNull(model.Timestamp);
		Assert.IsNull(model.CreatedBy);
		Assert.IsNull(model.ModifiedBy);
	}

	private sealed class TestClass : AuditedCompositeModel
	{ }
}
