using API.Context;
using API.Models;
using API.Pagination;
using X.PagedList;

namespace API.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
  public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
  {
    var produtos = await GetAllAsync();
    var produtosOrdenados = produtos.OrderBy(on => on.Nome).AsQueryable();
    var paginaProdutosOrdenada = await produtosOrdenados.ToPagedListAsync(produtosParams.PageNumber, produtosParams.PageSize);

    return paginaProdutosOrdenada;
  }

  public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco prodParams)
  {
    var produtos = await GetAllAsync();

    if (prodParams.Preco.HasValue && !string.IsNullOrEmpty(prodParams.PrecoCriterio))
    {
      if (prodParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
        produtos = produtos.Where(p => p.Preco > prodParams.Preco.Value).OrderBy(p => p.Preco);
      else if (prodParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
        produtos = produtos.Where(p => p.Preco < prodParams.Preco.Value).OrderBy(p => p.Preco);
      else if (prodParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
        produtos = produtos.Where(p => p.Preco == prodParams.Preco.Value).OrderBy(p => p.Preco);
    }

    var produtosFiltrados = await produtos.ToPagedListAsync(prodParams.PageNumber, prodParams.PageSize);

    return produtosFiltrados;
  }

  public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int categoriaID)
  {
    var produtos = await GetAllAsync();
    return produtos.Where(prod => prod.CategoriaId == categoriaID);
  }
}
