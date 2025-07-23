using NewsWebsite.DAL.Context;
using NewsWebsite.DAL.Contracts;
using NewsWebsite.DAL.Etities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsDbContext _newsContext;
        private readonly ConcurrentDictionary<string, Object> _repositories;
        //private readonly Dictionary<string, Object> _repositories;
        public UnitOfWork(NewsDbContext storeContext)
        {
            _newsContext = storeContext;
            _repositories = new();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
            => (IGenericRepository<TEntity>) _repositories
            .GetOrAdd(typeof(TEntity).Name, _ => new GenericRepository<TEntity>(_newsContext));
        //{
        //    var name = typeof(TEntity).Name;
        //    if (_repositories.ContainsKey(name))
        //        return (IGenericRepository<TEntity, TKey>) _repositories[name];
        //    var repo = new GenericRepository<TEntity, TKey>(_storeContext);
        //    _repositories.Add(name, repo);
        //    return new GenericRepository<TEntity, TKey>(_storeContext);
        //}

        public async Task<int> SaveChangesAsync() => await _newsContext.SaveChangesAsync();
    }
}
