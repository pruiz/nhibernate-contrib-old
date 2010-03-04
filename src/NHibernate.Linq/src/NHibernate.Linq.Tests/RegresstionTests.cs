using System.Linq;
using NHibernate.Linq.Tests.Entities;
using NHibernate.Transform;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class RegresstionTests : BaseTest
	{
		protected override string ConnectionStringName
		{
			get
			{
				return "Test";
			}
		}

		/// <summary>
		/// http://aspzone.com/tech/nhibernate-linq-troubles/
		/// </summary>
		[Test]
		public void HierarchicalQueries()
		{
			var children = from s in session.Linq<Role>()
						   where s.ParentRole != null
						   select s;
			Assert.AreEqual(0, children.Count());

			var roots = from s in session.Linq<Role>()
						where s.ParentRole == null
						select s;
			Assert.AreEqual(2, roots.Count());
		}


		[Test]
		public void DegenerateSelectInMethodChainShouldNotReplaceResultTransformer()
		{
			var queryable = session.Linq<Animal>();
			queryable.Expand("Children");
			queryable.QueryOptions.RegisterCustomAction
				(c => c.SetResultTransformer(new DistinctRootEntityResultTransformer()));

			var animals = queryable.Where(a => a.Id == 1).Select(a => a).ToList();
			Assert.AreEqual(1, animals.Count);
		}

		[Test]
		public void DegenerateSelectInLinqShouldNotReplaceResultTransformer()
		{
			var queryable = session.Linq<Animal>();
			queryable.Expand("Children");
			queryable.QueryOptions.RegisterCustomAction
				(c => c.SetResultTransformer(new DistinctRootEntityResultTransformer()));

			var animals = (from a in queryable select a).ToList();
			Assert.AreEqual(6, animals.Count);
		}
	}
}