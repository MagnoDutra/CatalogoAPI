using System;
using API.Models;

namespace API.Repositories;

public interface ICategoriaRepository
{
  IEnumerable<Categoria> GetCategorias();
  Categoria GetCategoriaById(int id);
  Categoria Create(Categoria categoria);
  Categoria Update(Categoria categoria);
  Categoria Delete(int id);
}
