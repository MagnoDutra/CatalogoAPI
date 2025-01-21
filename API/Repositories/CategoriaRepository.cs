using API.Context;
using API.Models;
using API.Pagination;
using X.PagedList;

namespace API.Repositories;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
  public async Task<IPagedList<Categoria>> GetCategoriaFilterNomeAsync(CategoriaFiltroNome categoriaFilterParam)
  {
    var categorias = await GetAllAsync();

    if (!string.IsNullOrEmpty(categoriaFilterParam.Nome))
    {
      categorias = categorias.Where(cat => cat.Nome.Contains(categoriaFilterParam.Nome, StringComparison.OrdinalIgnoreCase)).OrderBy(cat => cat.CategoriaId);
    }

    var categoriasFiltradas = await categorias.ToPagedListAsync(categoriaFilterParam.PageNumber, categoriaFilterParam.PageSize);

    return categoriasFiltradas;
  }

  public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriaParams)
  {
    var categorias = await GetAllAsync();

    var categoriasOrdenadas = categorias.OrderBy(cat => cat.CategoriaId).AsQueryable();

    var paginaCategoria = await categoriasOrdenadas.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);

    return paginaCategoria;
  }
}
