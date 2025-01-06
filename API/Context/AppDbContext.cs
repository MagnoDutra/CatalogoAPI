using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Categoria>? Categorias { get; set; }
  public DbSet<Produto>? Produtos { get; set; }

}
