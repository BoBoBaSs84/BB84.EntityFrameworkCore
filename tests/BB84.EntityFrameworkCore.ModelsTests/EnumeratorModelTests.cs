using BB84.EntityFrameworkCore.Models;
using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.ModelsTests;

[TestClass]
public sealed class EnumeratorModelTests
{
	[TestMethod]
	public void EnumeratorModelTest()
	{
		IEnumeratorModel model;

		model = new TestClass();

		Assert.IsNotNull(model);
		Assert.AreEqual(0, model.Id);
		Assert.IsNull(model.Timestamp);
		Assert.IsNull(model.Name);
		Assert.IsNull(model.Description);
		Assert.IsFalse(model.IsDeleted);
	}

	private class TestClass : EnumeratorModel
	{ }
}
