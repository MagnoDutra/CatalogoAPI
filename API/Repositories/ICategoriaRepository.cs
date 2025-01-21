using System;
using API.Models;
using API.Pagination;

namespace API.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
  public Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriaParams);
  public Task<PagedList<Categoria>> GetCategoriaFilterNomeAsync(CategoriaFiltroNome categoriaFilterParam);
}
