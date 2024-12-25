using BB84.EntityFrameworkCore.Models;
using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.ModelsTests;

[TestClass]
public sealed class EnumeratorModelTests
{
	[TestMethod]
	public void EnumeratorModelTest()
	{
		IEnumeratorModel? model;
		int id = int.MaxValue;
		string name = "Name";
		string description = "Description";
		bool isDeleted = true;

		model = new TestClass()
		{
			Id = id,
			Name = name,
			Description = description,
			IsDeleted = isDeleted
		};

		Assert.IsNotNull(model);
		Assert.AreEqual(id, model.Id);
		Assert.IsNull(model.Timestamp);
		Assert.AreEqual(name, model.Name);
		Assert.AreEqual(description, model.Description);
		Assert.AreEqual(isDeleted, model.IsDeleted);
	}

	private sealed class TestClass : EnumeratorModel
	{ }
}
