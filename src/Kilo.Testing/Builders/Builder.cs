using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Moq;

namespace Kilo.Testing.Builders
{
    public abstract class Builder<TBuilder, TInstance>
        where TBuilder : Builder<TBuilder, TInstance>
        where TInstance : class
    {
        /// <summary>
        /// Gets or sets the builder instance
        /// </summary>
        protected TBuilder _this;

        /// <summary>
        /// Gets or sets the test instance.
        /// </summary>
        protected TInstance Instance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Builder{TBuilder, TInstance}"/> class.
        /// </summary>
        public Builder()
        {
            _this = (TBuilder)this;
        }

        /// <summary>
        /// Builds the target object
        /// </summary>
        public virtual TInstance Build()
        {
            TInstance result = this.Instance;
            this.Instance = null;
            return result;
        }

        /// <summary>
        /// Builds a mock object from the instance.
        /// </summary>
        /// <returns>A object which is a Mock<typeparamref name="TInstance"/></returns>
        public virtual Mock<TInstance> BuildMock()
        {
            var mock = new Mock<TInstance>();

            var sourceProperties = typeof(TInstance).GetProperties();
            var keyProperties = new List<PropertyInfo>();

            foreach (var property in sourceProperties)
            {
                var getMethod = property.GetGetMethod();
                if (getMethod == null || getMethod.IsVirtual == false)
                {
                    continue;
                }

                if (property.CanRead && property.CanWrite)
                {
                    dynamic value = property.GetValue(this.Instance, null);

                    // Make a member acces expression which can be used to set up the Mock property
                    var parameterExpression = Expression.Parameter(typeof(TInstance));
                    var memberAccessExpression = Expression.MakeMemberAccess(parameterExpression, property);
                    var lambdaExpression = Expression.Lambda(memberAccessExpression, parameterExpression);

                    var method = this.GetType().GetMethod("ConvertExpression", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(typeof(TInstance), property.PropertyType);
                    dynamic result = method.Invoke(this, new object[] { lambdaExpression });

                    if (value != null)
                    {
                        mock.SetupProperty(result, value);
                    }
                    else
                    {
                        mock.SetupProperty(result);
                    }
                }
            }

            this.Instance = null;

            return mock;
        }

        internal Expression<Func<T1, T2>> ConvertExpression<T1, T2>(Expression source)
        {
            return (Expression<Func<T1, T2>>)source;
        }

        /// <summary>
        /// Gets the name of a property access expression
        /// </summary>
        /// <typeparam name="T">The intance type</typeparam>
        /// <param name="exp">The expression</param>
        protected string GetName<T>(Expression<Func<T, object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}
