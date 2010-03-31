using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;
using NHibernate.Linq.Tests.Entities;
namespace NHibernate.Linq.Tests
{

	[TestFixture]
	public class NHLQ67 :BaseTest
	{
		protected override ISession CreateSession()
		{
			return GlobalSetup.CreateSession();
		}

		[Test]
		public void CanUseExpressionOnWhereClause()
		{
			var name = "Dr Dobbs";
			Expression<Func<Patient, bool>> query = x => x.Physician.Name == name;

			var result = session.Linq<Patient>().Where(query).ToList();
			Assert.That(result.Count, Is.GreaterThan(0));
		}

		[Test]
		public void CanUseExpressionOnNestedClause()
		{
			Expression<Func<PatientRecord, bool>> nestedQuery = x => x.BirthDate <= DateTime.Now;
			Expression<Func<Patient, bool>> query = x => ((IQueryable<PatientRecord>)x.PatientRecords).Any(nestedQuery);

			var result = session.Linq<Patient>().Where(query).ToList();
			Assert.That(result.Count, Is.GreaterThan(0));
		}

		[Test]
		public void CanUseExpressionOnNestedClauseWithMemberAccess()
		{
			var now = DateTime.Now;
			Expression<Func<PatientRecord, bool>> nestedQuery = x => x.BirthDate <= now;
			Expression<Func<Patient, bool>> query = x => ((IQueryable<PatientRecord>)x.PatientRecords).Any(nestedQuery);

			var result = session.Linq<Patient>().Where(query).ToList();
			Assert.That(result.Count, Is.GreaterThan(0));
		}
	}
}
