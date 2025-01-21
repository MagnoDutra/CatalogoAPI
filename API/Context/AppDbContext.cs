using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
{
  public DbSet<Categoria>? Categorias { get; set; }
  public DbSet<Produto>? Produtos { get; set; }
}
