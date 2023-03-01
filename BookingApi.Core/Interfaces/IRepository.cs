using BookingApi.Core.Abstractions;

namespace BookingApi.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity GetById(Guid id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DetachLocal(TEntity entity);
    }
}
