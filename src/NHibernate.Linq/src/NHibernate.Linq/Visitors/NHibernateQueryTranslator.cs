using System;
using NHibernate.Linq.Expressions;
using LinqExpression = System.Linq.Expressions.Expression;

namespace NHibernate.Linq.Visitors
{
	/// <summary>
	/// Translates a Linq Expression into an NHibernate ICriteria object.
	/// </summary>
	public class NHibernateQueryTranslator : NHibernateExpressionVisitor
	{
		private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly ISession session;
		private readonly string entityName;
		private ICriteria rootCriteria;
		private QueryOptions options;

		public NHibernateQueryTranslator(ISession session)
		{
			this.session = session;
		}

		public NHibernateQueryTranslator(ISession session, string entityName)
		{
			this.session = session;
			this.entityName = entityName;
		}

		public virtual object Translate(LinqExpression expression, QueryOptions queryOptions)
		{
			this.rootCriteria = null;
			this.options = queryOptions;

			return TranslateInternal(expression);
		}

		public virtual object Translate(LinqExpression expression, ICriteria rootCriteria)
		{
			this.rootCriteria = rootCriteria;
			this.options = null;

			return TranslateInternal(expression);
		}

		private object TranslateInternal(LinqExpression expression)
		{
			try
			{
				Visit(expression); //ensure criteria

				var visitor = new RootVisitor(rootCriteria, session, true);
				visitor.Visit(expression);

				if (_Log.IsDebugEnabled)
					_Log.DebugFormat("Translated Criteria: {0}", rootCriteria.ToString());
				
				return visitor.Results;
			}
			catch (Exception ex)
			{
				var tmp = rootCriteria != null ? rootCriteria.ToString() : "{null}";
				_Log.ErrorFormat("Linq translation failed, current criteria: {0}", tmp);
				throw;
			}
		}

		protected override LinqExpression VisitQuerySource(QuerySourceExpression expr)
		{
			if (rootCriteria == null)
			{
				if (!string.IsNullOrEmpty(this.entityName))
					rootCriteria = session.CreateCriteria(entityName, expr.Alias);
				else
					rootCriteria = session.CreateCriteria(expr.ElementType, expr.Alias);
				options.Execute(rootCriteria);
			}
			return expr;
		}
	}
}