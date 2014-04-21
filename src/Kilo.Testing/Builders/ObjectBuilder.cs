using System;

namespace Kilo.Testing.Builders
{
    public class ObjectBuilder<TInstance> : ObjectBuilder<ObjectBuilder<TInstance>, TInstance>
        where TInstance : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilder{TInstance}"/> class.
        /// </summary>
        public ObjectBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilder{TInstance}"/> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public ObjectBuilder(TInstance instance)
            : base(instance)
        {
        }
    }

    public class ObjectBuilder<TBuilder, TInstance> : Builder<TBuilder, TInstance>
        where TBuilder : ObjectBuilder<TBuilder, TInstance>
        where TInstance : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilder{TBuilder, TInstance}"/> class.
        /// </summary>
        public ObjectBuilder()
        {
            this.Instance = new TInstance();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilder{TBuilder, TInstance}"/> class.
        /// </summary>
        /// <param name="instance">The instance to use</param>
        public ObjectBuilder(TInstance instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            this.Instance = instance;
        }
    }
}
