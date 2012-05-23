using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kilo.Data
{
    //public class UnitOfWork<T>
    //{
    //    List<T> _inserts, _deletes, _updates;
    //    IRepository<T> _provider;

    //    /// <summary>
    //    /// Gets the inserts.
    //    /// </summary>
    //    public IEnumerable<T> Inserts { get { return _inserts; } }

    //    /// <summary>
    //    /// Gets the updates.
    //    /// </summary>
    //    public IEnumerable<T> Updates { get { return _updates; } }

    //    /// <summary>
    //    /// Gets the deletes.
    //    /// </summary>
    //    public IEnumerable<T> Deletes { get { return _deletes; } }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="UnitOfWork&lt;T&gt;"/> class.
    //    /// </summary>
    //    /// <param name="provider">The provider.</param>
    //    public UnitOfWork(IRepository<T> provider)
    //    {
    //        _inserts = new List<T>();
    //        _deletes = new List<T>();
    //        _updates = new List<T>();

    //        _provider = provider;
    //    }

    //    /// <summary>
    //    /// Inserts the specified entity.
    //    /// </summary>
    //    /// <param name="entity">The entity.</param>
    //    public void Insert(T entity)
    //    {
    //        _inserts.Add(entity);
    //        _provider.Insert(entity);
    //    }

    //    /// <summary>
    //    /// Updates the specified entity.
    //    /// </summary>
    //    /// <param name="entity">The entity.</param>
    //    public void Update(T entity)
    //    {
    //        _updates.Add(entity);
    //        _provider.Update(entity);
    //    }

    //    /// <summary>
    //    /// Deletes the specified entity.
    //    /// </summary>
    //    /// <param name="entity">The entity.</param>
    //    public void Delete(T entity)
    //    {
    //        _deletes.Add(entity);
    //        _provider.Delete(entity);
    //    }

    //    /// <summary>
    //    /// Attaches the specified entity.
    //    /// </summary>
    //    /// <param name="entity">The entity.</param>
    //    /// <param name="state">The state.</param>
    //    public void Attach(T entity, State state)
    //    {
    //        _provider.Attach(entity, state);

    //        switch(state)
    //        {
    //            case State.Modified:
    //                _updates.Add(entity);
    //                break;

    //            case State.New:
    //                _inserts.Add(entity);
    //                break;
    //        }
    //    }

    //    /// <summary>
    //    /// Clears this instance.
    //    /// </summary>
    //    public void Clear()
    //    {
    //        _inserts.Clear();
    //        _updates.Clear();
    //        _deletes.Clear();
    //    }

    //}
}
