using System;
using API.Models;
using API.Pagination;

namespace API.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
  public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParams);
  public PagedList<Categoria> GetCategoriaFilterNome(CategoriaFiltroNome categoriaFilterParam);
}
