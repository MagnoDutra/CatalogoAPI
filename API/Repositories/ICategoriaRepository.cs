using System;
using API.Models;
using API.Pagination;
using X.PagedList;

namespace API.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
  public Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriaParams);
  public Task<IPagedList<Categoria>> GetCategoriaFilterNomeAsync(CategoriaFiltroNome categoriaFilterParam);
}
