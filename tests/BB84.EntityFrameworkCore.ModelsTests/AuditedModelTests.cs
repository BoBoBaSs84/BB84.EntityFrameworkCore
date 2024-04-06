using BB84.EntityFrameworkCore.Models;
using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.ModelsTests;

[TestClass]
public sealed class AuditedModelTests
{
	[TestMethod]
	public void AuditedModelTest()
	{
		IAuditedModel model;

		model = new TestClass();

		Assert.IsNotNull(model);
		Assert.AreEqual(Guid.Empty, model.Id);
		Assert.IsNull(model.Timestamp);
		Assert.IsNull(model.CreatedBy);
		Assert.IsNull(model.ModifiedBy);
	}

	private class TestClass : AuditedModel
	{ }
}
