using API.Context;
using API.Models;
using API.Pagination;

namespace API.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
  public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
  {
    return GetAll().OrderBy(on => on.Nome).Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize).Take(produtosParams.PageSize).ToList();
  }

  public IEnumerable<Produto> GetProdutosPorCategoria(int categoriaID)
  {
    return GetAll().Where(prod => prod.CategoriaId == categoriaID);
  }
}
