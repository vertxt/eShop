using eShop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAllAsync()
        {
            return _entities.AsQueryable<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is not null)
            {
                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}