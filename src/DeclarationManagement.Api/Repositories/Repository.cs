using DeclarationManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DeclarationManagement.Api.Repositories;

/// <summary>
/// 基础仓储实现：封装 EF Core 通用异步数据库访问。
/// </summary>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// _dbContext 字段。
    /// </summary>
    private readonly AppDbContext _dbContext;
    /// <summary>
    /// _dbSet 字段。
    /// </summary>
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    /// <summary>
    /// 获取数据。
    /// </summary>
    public async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    /// <summary>
    /// 获取数据。
    /// </summary>
    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// AddAsync 方法。
    /// </summary>
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// AddRangeAsync 方法。
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    /// <summary>
    /// 更新数据。
    /// </summary>
    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Remove 方法。
    /// </summary>
    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// SaveChangesAsync 方法。
    /// </summary>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
