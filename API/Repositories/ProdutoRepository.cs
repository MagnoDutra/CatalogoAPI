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

  public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco prodParams)
  {
    var produtos = GetAll().AsQueryable();

    if (prodParams.Preco.HasValue && !string.IsNullOrEmpty(prodParams.PrecoCriterio))
    {
      if (prodParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
        produtos = produtos.Where(p => p.Preco > prodParams.Preco.Value).OrderBy(p => p.Preco);
      else if (prodParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
        produtos = produtos.Where(p => p.Preco < prodParams.Preco.Value).OrderBy(p => p.Preco);
      else if (prodParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
        produtos = produtos.Where(p => p.Preco == prodParams.Preco.Value).OrderBy(p => p.Preco);
    }

    var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, prodParams.PageNumber, prodParams.PageSize);

    return produtosFiltrados;
  }

  public IEnumerable<Produto> GetProdutosPorCategoria(int categoriaID)
  {
    return GetAll().Where(prod => prod.CategoriaId == categoriaID);
  }
}
