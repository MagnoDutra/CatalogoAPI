using System;
using API.Models;

namespace API.Repositories;

public interface IProdutoRepository
{
  IQueryable<Produto> GetAll();
  Produto GetByID(int id);
  Produto Create(Produto produto);
  bool Update(Produto produto);
  bool Delete(int id);
}
