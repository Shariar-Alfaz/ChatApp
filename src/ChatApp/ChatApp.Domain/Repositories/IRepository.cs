using ChatApp.Infrastructure.Entity.Base;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ChatApp.Domain.Repositories
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IComparable
    {
        void Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        void Edit(TEntity entityToUpdate);
        Task EditAsync(TEntity entityToUpdate);
        IList<TEntity> GetAll();
        Task<IList<TEntity>> GetAllAsync();
        TEntity GetById(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
        int GetCount(Expression<Func<TEntity, bool>> filter = null);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);
        void Remove(Expression<Func<TEntity, bool>> filter);
        void Remove(TEntity entityToDelete);
        void Remove(TKey id);
        Task RemoveAsync(Expression<Func<TEntity, bool>> filter);
        Task RemoveAsync(TEntity entityToDelete);
        Task RemoveAsync(TKey id);
        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetByIncludingAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> navigationPropertyPath);
        Task<TEntity> FindAsync(TKey id);
        IList<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isTrackingOff = false
        );
        (IList<TEntity> data, int total, int totalDisplay) Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 1,
            int pageSize = 10,
            bool isTrackingOff = false
        );
        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isTrackingOff = false
        );
        Task<(IList<TEntity> data, int total, int totalDisplay)> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 1,
            int pageSize = 10,
            bool isTrackingOff = false
        );
        Task<IList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
        );
        Task<IEnumerable<TResult>> GetAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default
        )
            where TResult : class;
        IList<TEntity> GetDynamic(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isTrackingOff = false
        );
        (IList<TEntity> data, int total, int totalDisplay) GetDynamic(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 1,
            int pageSize = 10,
            bool isTrackingOff = false
        );
        Task<IList<TEntity>> GetDynamicAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isTrackingOff = false
        );
        Task<(IList<TEntity> data, int total, int totalDisplay)> GetDynamicAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 1,
            int pageSize = 10,
            bool isTrackingOff = false
        );
        Task<TResult> SingleOrDefaultAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true
        );
    }
}
