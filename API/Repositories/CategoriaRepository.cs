using API.Context;
using API.Models;
using API.Pagination;

namespace API.Repositories;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
  public async Task<PagedList<Categoria>> GetCategoriaFilterNomeAsync(CategoriaFiltroNome categoriaFilterParam)
  {
    var categorias = await GetAllAsync();

    if (!string.IsNullOrEmpty(categoriaFilterParam.Nome))
    {
      categorias = categorias.Where(cat => cat.Nome.Contains(categoriaFilterParam.Nome, StringComparison.OrdinalIgnoreCase)).OrderBy(cat => cat.CategoriaId);
    }

    var categoriaPaginada = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriaFilterParam.PageNumber, categoriaFilterParam.PageSize);

    return categoriaPaginada;
  }

  public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriaParams)
  {
    var categorias = await GetAllAsync();

    var categoriasOrdenadas = categorias.OrderBy(cat => cat.CategoriaId).AsQueryable();

    var paginaCategoria = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriaParams.PageNumber, categoriaParams.PageSize);

    return paginaCategoria;
  }


}
