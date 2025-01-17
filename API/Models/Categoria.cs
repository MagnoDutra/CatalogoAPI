using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class Categoria
{
  public int CategoriaId { get; set; }

  [Required]
  [StringLength(80)]
  public required string Nome { get; set; }


  [Required]
  [StringLength(300)]
  public required string ImagemUrl { get; set; }

  [JsonIgnore]
  public ICollection<Produto>? Produtos { get; set; } = new Collection<Produto>();
}
