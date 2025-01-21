using API.Models;
using API.Pagination;
using X.PagedList;

namespace API.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
  Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int categoriaID);
  Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
  Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco prodParams);
}
