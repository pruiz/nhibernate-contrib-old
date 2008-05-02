using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class SizeValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			SizeValidator v = new SizeValidator();
			v.Initialize(new SizeAttribute());
			Assert.IsTrue(v.IsValid(new int[0]));
			v.Initialize(new SizeAttribute(1,3));
			Assert.IsTrue(v.IsValid(new int[1]));
			Assert.IsTrue(v.IsValid(new int[3]));
			Assert.IsTrue(v.IsValid(null));

			Assert.IsFalse(v.IsValid(new int[0]));
			Assert.IsFalse(v.IsValid(new int[4]));
			Assert.IsFalse(v.IsValid("46546546"));
			Assert.IsFalse(v.IsValid(123456));
		}
	}
}