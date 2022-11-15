using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.DataService.Models;
using System.Linq.Expressions;
using PhlegmaticOne.PagedList.Base;
using PhlegmaticOne.PagedList.Extensions;

namespace PhlegmaticOne.DataService.Implementation;

public class DbSetDataRepository<TEntity> : IDataRepository<TEntity> where TEntity : EntityBase
{
    private readonly DbSet<TEntity> _set;

    public DbSetDataRepository(DbSet<TEntity> dbSet) => _set = dbSet;

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var result = await _set.AddAsync(entity);
        return result.Entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity, Action<TEntity> actionOverExistingEntity)
    {
        actionOverExistingEntity(entity);
        _set.Update(entity);
        return Task.FromResult(entity);
    }

    public async Task<TEntity?> UpdateAsync(Guid id, Action<TEntity> actionOverExistingEntity)
    {
        var existing = await GetByIdOrDefaultAsync(id);

        if (existing is null)
        {
            return null;
        }

        actionOverExistingEntity(existing);
        _set.Update(existing);
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await GetByIdOrDefaultAsync(id);

        if (entity is null)
        {
            return false;
        }

        _set.Remove(entity);
        return true;
    }

    public Task<TEntity?> GetByIdOrDefaultAsync(Guid id,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> query = _set;

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return query.FirstOrDefaultAsync(x => x.Id == id);
    }

        
    public async Task<IList<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> query = _set;

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return await query.Select(selector).ToListAsync();
    }

    public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> query = _set;

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0,
        int pageSize = 20)
    {
        IQueryable<TEntity> query = _set;

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return query.ToPagedListAsync(pageIndex, pageSize);
    }

    public Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        IQueryable<TEntity> query = _set;

        if (include is not null)
        {
            query = include(query);
        }

        return query.FirstOrDefaultAsync(predicate);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null) =>
        predicate is null ? _set.CountAsync() : _set.CountAsync(predicate);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? predicate = null) =>
        predicate is null ? _set.AnyAsync() : _set.AnyAsync(predicate);

    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate) => 
        _set.AllAsync(predicate);
}