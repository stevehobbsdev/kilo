using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;


namespace Kilo.Data.Caching
{
//    public class CachedRepository<T> : IRepository<T>
//    {
//        IRepository<T> _repository;
//        ICacheProvider _cache;
//        string _cacheKey;
//        Expression<Func<T, bool>> _cacheExpression = null;
//        UnitOfWork<T> _ouw;

//        /// <summary>
//        /// Gets the provider being cached.
//        /// </summary>
//        public IRepository<T> Provider
//        {
//            get { return _repository; }
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="CachedRepository&lt;T&gt;"/> class.
//        /// </summary>
//        /// <param name="repository">The repository.</param>
//        public CachedRepository(IRepository<T> repository)
//            : this(repository, new DefaultCacheProvider(), null)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="CachedRepository&lt;T&gt;"/> class.
//        /// </summary>
//        /// <param name="repository">The repository.</param>
//        /// <param name="cache">The cache.</param>
//        /// <param name="cacheExpression">The cache expression.</param>
//        public CachedRepository(IRepository<T> repository, ICacheProvider cache, Expression<Func<T, bool>> cacheExpression)
//        {
//            _repository = repository;
//            _cache = cache;
//            _cacheKey = typeof(T).Name + "_All";
//            _cacheExpression = cacheExpression;

//            _ouw = new UnitOfWork<T>(_repository);
//        }

//        /// <summary>
//        /// Gets a single entity with the specified key.
//        /// </summary>
//        /// <param name="key">The key.</param>
//        public T GetSingle(object key)
//        {
//            var data = GetCachedData();

//            if (data.ContainsKey(key))
//            {
//#if DEBUG
//                Trace.WriteLine("Returning object id " + key.ToString() + " from cache " + _cacheKey);
//#endif
//                return data[key];
//            }
//            else
//                return default(T);
//        }

//        /// <summary>
//        /// Gets all of the entities.
//        /// </summary>
//        public IQueryable<T> All()
//        {
//            var cached = GetCachedData();

//#if DEBUG
//            Trace.WriteLine("Returning all from cache " + _cacheKey);
//#endif

//            return cached.Values.AsQueryable();
//        }

//        /// <summary>
//        /// Alls the specified predicate.
//        /// </summary>
//        /// <param name="predicate">The predicate.</param>
//        public IQueryable<T> All(Expression<Func<T, bool>> predicate)
//        {
//            var cached = GetCachedData();

//#if DEBUG
//            Trace.WriteLine("Returning all from cache with predicate " + _cacheKey);
//#endif

//            return cached.Values.AsQueryable().Where(predicate);
//        }

//        /// <summary>
//        /// Inserts the specified entity.
//        /// </summary>
//        /// <param name="entity">The entity.</param>
//        public void Insert(T entity)
//        {
//            _ouw.Insert(entity);
//        }

//        /// <summary>
//        /// Updates the specified entity.
//        /// </summary>
//        /// <param name="entity">The entity.</param>
//        public void Update(T entity)
//        {
//            _ouw.Update(entity);
//        }

//        /// <summary>
//        /// Deletes the specified entity.
//        /// </summary>
//        /// <param name="entity">The entity.</param>
//        public void Delete(T entity)
//        {
//            _ouw.Delete(entity);
//        }

//        /// <summary>
//        /// Attaches the specified entity.
//        /// </summary>
//        /// <param name="entity">The entity.</param>
//        /// <param name="state">The state.</param>
//        public void Attach(T entity, State state = State.Unchanged)
//        {
//            _ouw.Attach(entity, state);		
//        }

//        /// <summary>
//        /// Detaches the specified entity.
//        /// </summary>
//        /// <param name="entity">The entity.</param>
//        public void Detach(T entity)
//        {
//            Provider.Detach(entity);
//        }

//        /// <summary>
//        /// Saves the changes made to the repository.
//        /// </summary>
//        public void SaveChanges()
//        {
//#if DEBUG
//            Trace.WriteLine("Saving changes from provider");
//#endif
//            Provider.SaveChanges();

//            var cachedData = GetCachedData();

//            foreach (var item in _ouw.Inserts)
//            {
//                Provider.Detach(item);
//                cachedData[this.GetObjectKey(item)] = item;
//            }

//            foreach (var item in _ouw.Updates)
//            {
//                Provider.Detach(item);
//                cachedData[this.GetObjectKey(item)] = item;
//            }

//            foreach (var item in _ouw.Deletes)
//            {
//                Provider.Detach(item);
//                var key = this.GetObjectKey(item);

//                if (cachedData.ContainsKey(key))
//                    cachedData.Remove(key);
//            }

//#if DEBUG
//            Trace.WriteLine("Updating cache");
//#endif

//            SetCache(cachedData);


//#if DEBUG
//            Trace.WriteLine("Clearing work items");
//#endif
//            _ouw.Clear();
//        }

//        /// <summary>
//        /// Gets the object key.
//        /// </summary>
//        /// <param name="entity">The entity.</param>
//        public object GetObjectKey(T entity)
//        {
//            return this.Provider.GetObjectKey(entity);
//        }

//        /// <summary>
//        /// Gets the cached data.
//        /// </summary>
//        private Dictionary<object, T> GetCachedData()
//        {
//            var data = _cache.Get(_cacheKey) as Dictionary<object, T>;

//            if (data == null)
//            {
//#if DEBUG
//                Trace.WriteLine("Cache not populated");
//#endif
//                data = HydrateCache();
//            }

//            return data;
//        }

//        /// <summary>
//        /// Hydrates the cache with data from the associated provider.
//        /// </summary>
//        private Dictionary<object, T> HydrateCache()
//        {
//            Dictionary<object, T> data = null;

//            if (_cacheExpression != null)
//            {
//#if TRACE
//                Trace.WriteLine("Creating data index on primary key");
//#endif
//                data = Provider.All(_cacheExpression).ToDictionary(r => this.GetObjectKey(r));
//            }
//            else
//                data = Provider.All().ToDictionary(r => this.GetObjectKey(r));

//#if DEBUG
//            Trace.WriteLine("Rehydrating cache with " + data.Count.ToString() + " objects from provider");
//#endif

//            foreach (var item in data)
//            {
//                Provider.Detach(item.Value);
//            }

//            SetCache(data);

//            return data;
//        }

//        /// <summary>
//        /// Sets the cache with the specified data
//        /// </summary>
//        /// <param name="data">The data.</param>
//        private void SetCache(Dictionary<object, T> data)
//        {
//            _cache.Set(_cacheKey, data, DateTime.Now.AddMinutes(5));
//        }
//    }
}
