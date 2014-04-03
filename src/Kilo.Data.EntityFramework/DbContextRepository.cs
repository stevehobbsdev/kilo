using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Kilo.Expressions;

namespace Kilo.Data.EntityFramework
{
    public class DbContextRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        TContext _context;
        IDbSet<TEntity> _set;

        public DbContextRepository()
        {
            _context = Activator.CreateInstance(typeof(TContext)) as TContext;
            _set = _context.Set<TEntity>();
        }

        public DbContextRepository(TContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public TEntity GetSingle(object key)
        {
            return _set.Find(key);
        }

        public IQueryable<TEntity> All()
        {
            return _set;
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public IQueryable<TEntity> AllWithIncludes(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _set;
            var exprParser = new ExpressionParser<TEntity, object>();

            foreach (var includeExpression in includes)
            {
                string path = exprParser.GetPropertyPathFromExpression(includeExpression);
                query = query.Include(path);
            }

            return query;
        }

        public IQueryable<TEntity> AllWithIncludes(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _set.Where(predicate);
            var exprParser = new ExpressionParser<TEntity, object>();

            foreach (var includeExpression in includes)
            {
                string path = exprParser.GetPropertyPathFromExpression(includeExpression);
                query = query.Include(path);
            }

            return query;
        }

        public void Insert(TEntity entity)
        {
            _set.Add(entity);
        }

        public void Update(TEntity entity)
        {
        }

        public void Delete(TEntity entity)
        {
            _set.Remove(entity);
        }

        public void Attach(TEntity entity, State state = State.Unchanged)
        {
            _set.Attach(entity);

            var entry = _context.Entry<TEntity>(entity);

            switch (state)
            {
                case State.Modified:
                    entry.State = EntityState.Modified;
                    break;

                case State.New:
                    entry.State = EntityState.Added;
                    break;
            }
        }

        public void Detach(TEntity entity)
        {
            var entry = _context.Entry<TEntity>(entity);

            if (entry != null)
                entry.State = System.Data.Entity.EntityState.Detached;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #region Not applicable 
        public object GetObjectKey(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
