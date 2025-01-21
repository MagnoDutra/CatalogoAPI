using API.Models;
using API.Pagination;

namespace API.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
  Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int categoriaID);
  Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
  Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco prodParams);
}
