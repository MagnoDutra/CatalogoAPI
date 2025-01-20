using API.Models;
using API.Pagination;

namespace API.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
  IEnumerable<Produto> GetProdutosPorCategoria(int categoriaID);
  PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
}
