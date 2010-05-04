using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NHibernate.Linq.Visitors
{
	public class ConstantExpressionReducer : ExpressionVisitor
	{
		private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		protected override Expression VisitConstant(ConstantExpression c)
		{
			if (!c.Type.IsSubclassOf(typeof(Expression)))
				return base.VisitConstant(c);

			if (_Log.IsDebugEnabled)
				_Log.DebugFormat("Found reducible constant: {0}", c.ToString());

			var result = Visit(c.Value as Expression);

			if (_Log.IsDebugEnabled)
				_Log.DebugFormat("Reduced to: {0}", result.ToString());

			return result;
		}
	}
}
