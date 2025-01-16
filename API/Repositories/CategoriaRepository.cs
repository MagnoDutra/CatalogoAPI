using System;
using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class CategoriaRepository(AppDbContext context) : ICategoriaRepository
{
  public IEnumerable<Categoria> GetCategorias()
  {
    return context.Categorias.AsNoTracking().ToList();
  }

  public Categoria GetCategoriaById(int id)
  {
    return context.Categorias.AsNoTracking().FirstOrDefault(cat => cat.CategoriaId == id);
  }

  public Categoria Create(Categoria categoria)
  {
    if (categoria is null)
      throw new ArgumentNullException(nameof(categoria));

    context.Categorias.Add(categoria);
    context.SaveChanges();
    return categoria;
  }

  public Categoria Update(Categoria categoria)
  {
    if (categoria is null)
      throw new ArgumentNullException(nameof(categoria));

    context.Entry(categoria).State = EntityState.Modified;
    context.SaveChanges();
    return categoria;
  }

  public Categoria Delete(int id)
  {
    var categoria = context.Categorias.Find(id);

    if (categoria is null)
      throw new ArgumentNullException(nameof(categoria));

    context.Categorias.Remove(categoria);
    context.SaveChanges();
    return categoria;
  }
}
