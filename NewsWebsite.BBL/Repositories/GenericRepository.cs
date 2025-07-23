using Microsoft.EntityFrameworkCore;
using NewsWebsite.BBL.Specifications;
using NewsWebsite.DAL.Context;
using NewsWebsite.DAL.Contracts;
using NewsWebsite.DAL.Etities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly NewsDbContext _newsContext;

        public GenericRepository(NewsDbContext storeContext)
        {
            _newsContext = storeContext;
        }

        public async Task AddAsync(TEntity entity) => await _newsContext.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync(ISpecification<TEntity> specifications)
            => await SpecificationEvaluator.CreateQuery(_newsContext.Set<TEntity>(), specifications).CountAsync();

        public void DeleteAsync(TEntity entity) => _newsContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
            => trackChanges ? await _newsContext.Set<TEntity>().ToListAsync()
            : await _newsContext.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specifications)
            => await SpecificationEvaluator.CreateQuery(_newsContext.Set<TEntity>(), specifications).ToListAsync();

        public async Task<TEntity?> GetAsync(int id) => await _newsContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetAsync(ISpecification<TEntity> specifications)
            => await SpecificationEvaluator.CreateQuery(_newsContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();

        public void UpdateAsync(TEntity entity) => _newsContext.Set<TEntity>().Update(entity);
    }
}
