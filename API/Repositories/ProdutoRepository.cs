using System;
using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class ProdutoRepository(AppDbContext context) : IProdutoRepository
{
  public IQueryable<Produto> GetAll()
  {
    return context.Produtos!;
  }

  public Produto GetByID(int id)
  {
    return context.Produtos!.AsNoTracking().FirstOrDefault(prod => prod.ProdutoId == id)!;
  }

  public Produto Create(Produto produto)
  {
    ArgumentNullException.ThrowIfNull(produto);

    context.Produtos!.Add(produto);
    context.SaveChanges();

    return produto;
  }

  public bool Update(Produto produto)
  {
    ArgumentNullException.ThrowIfNull(produto);

    if (context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId))
    {
      context.Produtos.Update(produto);
      context.SaveChanges();
      return true;
    }
    return false;
  }

  public bool Delete(int id)
  {
    var produto = context.Produtos!.Find(id);

    ArgumentNullException.ThrowIfNull(produto);

    context.Produtos.Remove(produto);
    context.SaveChanges();

    return true;
  }

}
