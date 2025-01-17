using System;
using API.Context;

namespace API.Repositories;

public class UnitOfWork : IUnitOfWork
{
  private IProdutoRepository? produtoRepo;
  private ICategoriaRepository? categoriaRepo;
  public AppDbContext context;

  public IProdutoRepository ProdutoRepository
  {
    get
    {
      return produtoRepo = produtoRepo ?? new ProdutoRepository(context);
    }
  }

  public ICategoriaRepository CategoriaRepository
  {
    get
    {
      return categoriaRepo ??= new CategoriaRepository(context);
    }
  }

  public UnitOfWork(AppDbContext context)
  {
    this.context = context;
  }

  public void Commit()
  {
    context.SaveChanges();
  }

  public void Dispose()
  {
    context.Dispose();
  }
}
