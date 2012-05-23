using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Kilo.Expressions
{
    public class ExpressionParser<T, TResult>
    {
        /// <summary>
        /// Takes an expression and finds the property path
        /// </summary>
        public string GetPropertyPathFromExpression(Expression<Func<T, TResult>> item)
        {
            LambdaExpression lExpr = item as LambdaExpression;

            if (lExpr == null)
                throw new ApplicationException("Only lambda expressions are supported at this time");

            MemberExpression mExpr = lExpr.Body as MemberExpression;

            return ParseMemberExpression(mExpr);
        }

        /// <summary>
        /// Parses a single member expression recursively and returns a string representation of the expression path.
        /// </summary>
        /// <param name="mExpr">The member expression to parse.</param>
        private string ParseMemberExpression(MemberExpression mExpr)
        {
            StringBuilder pathBuilder = new StringBuilder();

            if (mExpr.Expression is MemberExpression)
            {
                pathBuilder.Append(ParseMemberExpression(mExpr.Expression as MemberExpression)).Append(".");
            }

            PropertyInfo pInfo = mExpr.Member as PropertyInfo;
            string propertyName = pInfo.Name;

            pathBuilder.Append(propertyName);

            return pathBuilder.ToString();
        }
    }
}
