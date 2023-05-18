using Microsoft.EntityFrameworkCore;
using Projekt.Interfaces;
using Projekt.Models;
using System.Linq.Expressions;

namespace Projekt.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().Where(predicate);

            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

    }
}
