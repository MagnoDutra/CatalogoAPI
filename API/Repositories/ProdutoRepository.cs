using API.Context;
using API.Models;

namespace API.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
  public IEnumerable<Produto> GetProdutosPorCategoria(int categoriaID)
  {
    return GetAll().Where(prod => prod.CategoriaId == categoriaID);
  }
}
