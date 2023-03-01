using BookingApi.Core.Abstractions;
using BookingApi.Core.Interfaces;
using BookingApi.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Infrastructure.Data.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext _context;

        public RepositoryBase(ApplicationContext applicationContext)
        {
            _context = applicationContext;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public void DetachLocal(T entity)
        {
            var local = _context.Set<T>().Local.Where(x => x.Id == entity.Id).FirstOrDefault();
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
