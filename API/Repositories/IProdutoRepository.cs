using System;
using API.Models;

namespace API.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
  IEnumerable<Produto> GetProdutosPorCategoria(int categoriaID);
}
