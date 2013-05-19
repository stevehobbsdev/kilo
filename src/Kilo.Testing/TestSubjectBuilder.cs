using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using Kilo.Testing.Builders;
using Moq;

namespace Kilo.Testing
{
    public class TestSubjectBuilder<T> : TestSubjectBuilder<TestSubjectBuilder<T>, T>
        where T : class
    {
    }

    public class TestSubjectBuilder<TBuilder, T> : Builder<TBuilder, T>
        where TBuilder : TestSubjectBuilder<TBuilder, T>
        where T : class
    {
        private IServiceContainer _container = new ServiceContainer();

        /// <summary>
        /// Gets the service container.
        /// </summary>
        public IServiceContainer ServiceContainer
        {
            get { return this._container; }
        }

        /// <summary>
        /// Binds the type TInput to the specified instance.
        /// </summary>
        /// <typeparam name="TInput">The binding type</typeparam>
        /// <param name="instance">The instance to bind to the type</param>
        public TBuilder Bind<TInput>(TInput instance)
        {
            if (instance == null)
                throw new ArgumentNullException("An instance must be provided");

            _container.AddService(typeof(TInput), instance);

            return _this;
        }

        /// <summary>
        /// Builds the provider instance
        /// </summary>
        public override T Build()
        {
            Type instanceType = typeof(T);

            var instance = CreateInstance(instanceType, instanceType);

            return instance as T;
        }

        /// <summary>
        /// Builds a mocked version of the specified type
        /// </summary>
        public override Mock<T> BuildMock()
        {
            var instance = CreateInstance(typeof(Mock<T>), typeof(T)) as Mock<T>;

            object mockInstance = instance.Object;

            return instance;
        }

        /// <summary>
        /// Builds a mocked version of the specified type, optionally specifying whether or not the base method should be called.
        /// </summary>
        public Mock<T> BuildMock(bool callbase)
        {
            var mock = this.BuildMock();
            mock.CallBase = callbase;

            return mock;
        }

        /// <summary>
        /// Creates the required type instance.
        /// </summary>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="subjectType">Type of the subject.</param>
        protected virtual object CreateInstance(Type instanceType, Type subjectType)
        {
            // Assume one constructor
            ConstructorInfo[] constructors = subjectType.GetConstructors();
            ConstructorInfo constructor = null;

            if (constructors.Length != 1)
            {
                throw new InvalidOperationException("This builder only supports construction of types which have exactly one constructor");
            }

            constructor = constructors[0];

            var parameters = constructor.GetParameters();
            var arguments = new List<object>();

            // Run through each argument on the constructor and create mocks for each one. For each, where there is a binding
            // in _container, use that instead.
            foreach (var param in parameters)
            {
                Type paramType = param.ParameterType;
                object instance = GetInstanceToInject(this._container, paramType);

                arguments.Add(instance);
            }

            object result = Activator.CreateInstance(instanceType, arguments.ToArray());

            return result;
        }

        /// <summary>
        /// Takes the type and returns an instance which is of that type. The instance will either be a Mock<> or
        /// will comee from the types which have been bound to this builder.
        /// </summary>
        /// <param name="paramType">The type to create</param>
        private static object GetInstanceToInject(IServiceContainer container, Type objectType)
        {
            object instance = null;

            instance = container.GetService(objectType);

            if (instance == null)
            {
                var mockType = typeof(Mock<>).MakeGenericType(objectType);

                object mock = Activator.CreateInstance(mockType);
                var objectProperty = GetLowestProperty(mockType, "Object");

                instance = objectProperty.GetMethod.Invoke(mock, null);
            }

            return instance;
        }

        // Thanks to Jon Skeet O_o
        private static PropertyInfo GetLowestProperty(Type type, string name)
        {
            while (type != null)
            {
                var property = type.GetProperty(name, BindingFlags.DeclaredOnly |
                                                      BindingFlags.Public |
                                                      BindingFlags.Instance);
                if (property != null)
                {
                    return property;
                }

                type = type.BaseType;
            }
            return null;
        }
    }
}
