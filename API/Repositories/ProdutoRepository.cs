using API.Context;
using API.Models;
using API.Pagination;

namespace API.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
  public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
  {
    var produtos = GetAll().OrderBy(on => on.Nome).AsQueryable();
    var paginaProdutosOrdenada = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);

    return paginaProdutosOrdenada;
  }

  public IEnumerable<Produto> GetProdutosPorCategoria(int categoriaID)
  {
    return GetAll().Where(prod => prod.CategoriaId == categoriaID);
  }
}
