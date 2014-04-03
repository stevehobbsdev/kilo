
namespace Kilo.Data
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits the operations which are currently in the unit of work
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks this unit of work.
        /// </summary>
        void Rollback();
    }
}
