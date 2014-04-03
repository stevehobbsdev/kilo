namespace Kilo.Data
{
    public interface IEntityMapper<TEntity, TDto>
    {
        /// <summary>
        /// Maps to an entity from a DTO
        /// </summary>
        TEntity MapToEntity(TDto target);

        /// <summary>
        /// Maps from an entity to a DTO
        /// </summary>
        TDto MapFromEntity(TEntity entity);
    }
}
