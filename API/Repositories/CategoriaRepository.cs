using API.Context;
using API.Models;
using API.Pagination;

namespace API.Repositories;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
  public PagedList<Categoria> GetCategoriaFilterNome(CategoriaFiltroNome categoriaFilterParam)
  {
    var categorias = GetAll().AsQueryable();

    if (!string.IsNullOrEmpty(categoriaFilterParam.Nome))
    {
      categorias = categorias.Where(cat => cat.Nome.Contains(categoriaFilterParam.Nome, StringComparison.OrdinalIgnoreCase)).OrderBy(cat => cat.CategoriaId);
    }

    var categoriaPaginada = PagedList<Categoria>.ToPagedList(categorias, categoriaFilterParam.PageNumber, categoriaFilterParam.PageSize);

    return categoriaPaginada;
  }

  public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParams)
  {
    var categorias = GetAll().OrderBy(cat => cat.CategoriaId).AsQueryable();
    var paginaCategoria = PagedList<Categoria>.ToPagedList(categorias, categoriaParams.PageNumber, categoriaParams.PageSize);

    return paginaCategoria;
  }


}
