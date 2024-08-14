using System;
using System.Linq.Expressions;
using System.Reflection;
using Ems.Core.Helpers;
using Ems.Domain.Interfaces;
using Ems.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ems.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _entities;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public virtual void Add(TEntity entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        _entities.AddRange(entities);
        _context.SaveChanges();
    }

    public virtual void Update(TEntity entity)
    {
        _context.Update(entity);
        _context.SaveChanges();
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        _entities.UpdateRange(entities);
    }

    public virtual void Remove(TEntity entity)
    {
        _context.Remove(entity);
        _context.SaveChanges();
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        _entities.RemoveRange(entities);
    }

    public virtual int Count()
    {
        return _entities.Count();
    }

    public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.Where(predicate);
    }

    public virtual Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.SingleOrDefaultAsync(predicate);
    }

    public virtual Task<bool> GetFirsOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.AnyAsync(predicate);
    }

    public virtual TEntity Get(int id)
    {
        return _entities.Find(id);
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return _entities.AsNoTracking();
    }


    public async Task<PagedList<TEntity>> GetAllAsync(string sortColumn, string sortOrder, string searchTerm, int page,
        int pageSize)
    {
        IQueryable<TEntity> entityQuery = _entities.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(searchTerm)) entityQuery = ApplySearchTermFilter<TEntity>(entityQuery, searchTerm);

        entityQuery = ApplySorting<TEntity>(entityQuery, sortColumn, sortOrder);

        var entities = entityQuery
            .AsSingleQuery();

        var pagedEntities = await PagedList<TEntity>.CreateAsync(entities, page, pageSize);

        return pagedEntities;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }



    private static Expression<Func<TEntity, object>> GetSortProperty(string sortColumn)
    {
        switch (sortColumn?.ToLower())
        {
            case "name":
                return entity => entity.GetType().GetProperty("Name").GetValue(entity, null);
            default:
                return entity => entity.GetType().GetProperty("Id").GetValue(entity, null);
        }
    }

    private IQueryable<TEntity> ApplySearchTermFilter<T>(IQueryable<TEntity> query, string searchTerm)
    {
        var entityType = typeof(TEntity);
        var parameter = Expression.Parameter(entityType, "e");
        var properties = entityType.GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .ToList();

        if (properties.Any())
        {
            Expression combinedExpression = null;
            var searchTermLower = searchTerm.ToLower();

            foreach (var property in properties)
            {
                var propertyExpression = Expression.Property(parameter, property);
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var toLowerCall = Expression.Call(propertyExpression, toLowerMethod);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchTermExpression = Expression.Constant(searchTermLower);

                var containsCall = Expression.Call(toLowerCall, containsMethod, searchTermExpression);

                if (combinedExpression == null)
                    combinedExpression = containsCall;
                else
                    combinedExpression = Expression.Or(combinedExpression, containsCall);
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);
            return query.Where(lambda);
        }

        return query;
    }

    private IQueryable<TEntity> ApplySorting<T>(IQueryable<TEntity> query, string sortColumn, string sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortColumn) || sortOrder == null) return query;

        var entityType = typeof(TEntity);
        var property = entityType.GetProperty(sortColumn,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null) return query;

        var parameter = Expression.Parameter(entityType, "e");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        if (sortOrder.ToLower() == "desc")
            return Queryable.OrderByDescending(query, (dynamic)orderByExpression);
        return Queryable.OrderBy(query, (dynamic)orderByExpression);
    }

}