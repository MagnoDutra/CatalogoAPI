using API.Context;
using API.Models;
using API.Pagination;

namespace API.Repositories;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
  public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParams)
  {
    var categorias = GetAll().OrderBy(cat => cat.CategoriaId).AsQueryable();
    var paginaCategoria = PagedList<Categoria>.ToPagedList(categorias, categoriaParams.PageNumber, categoriaParams.PageSize);

    return paginaCategoria;
  }
}
