using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models;

[Table("Produtos")]
public class Produto
{
  [Key]
  public int ProdutoId { get; set; }

  [Required]
  [StringLength(80)]
  public required string Nome { get; set; }

  [Required]
  [StringLength(300)]
  public required string Descricao { get; set; }

  [Required]
  [Column(TypeName = "decimal(10,2)")]
  public required decimal Preco { get; set; }

  [Required]
  [StringLength(300)]
  public required string ImagemUrl { get; set; }

  public float Estoque { get; set; }
  public DateTime DataCadastro { get; set; }

  public int CategoriaId { get; set; }

  [JsonIgnore]
  public Categoria? Categoria { get; set; }

}
