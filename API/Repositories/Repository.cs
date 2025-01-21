using System;
using System.Linq.Expressions;
using API.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
  protected readonly AppDbContext context;

  public Repository(AppDbContext context)
  {
    this.context = context;
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await context.Set<T>().AsNoTracking().ToListAsync();
  }

  public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
  {
    return await context.Set<T>().FirstOrDefaultAsync(predicate);
  }

  public T Create(T entity)
  {
    context.Set<T>().Add(entity);
    return entity;
  }

  public T Update(T entity)
  {
    context.Set<T>().Update(entity);
    return entity;
  }

  public T Delete(T entity)
  {
    context.Set<T>().Remove(entity);
    return entity;
  }
}
