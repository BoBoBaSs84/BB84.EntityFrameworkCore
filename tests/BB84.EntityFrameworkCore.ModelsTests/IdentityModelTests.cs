using BB84.EntityFrameworkCore.Models;
using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.ModelsTests;

[TestClass]
public sealed class IdentityModelTests
{
	[TestMethod]
	public void IdentityModelTest()
	{
		IIdentityModel model;

		model = new TestClass();

		Assert.IsNotNull(model);
		Assert.AreEqual(Guid.Empty, model.Id);
		Assert.IsNull(model.Timestamp);
	}

	private class TestClass : IdentityModel
	{ }
}
