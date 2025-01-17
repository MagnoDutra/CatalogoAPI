using API.Context;
using API.Models;

namespace API.Repositories;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{

}
