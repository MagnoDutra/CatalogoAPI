using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using API.Validations;

namespace API.Models;

[Table("Produtos")]
public class Produto : IValidatableObject
{
  [Key]
  public int ProdutoId { get; set; }

  [Required]
  [StringLength(80)]
  [Teste]
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

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (!string.IsNullOrEmpty(this.Nome))
    {
      var primeiraLetra = this.Nome[0].ToString();
      if (primeiraLetra != primeiraLetra.ToUpper())
      {
        yield return new ValidationResult("A primeira letra deve ser maiuscula", new[] { nameof(this.Nome) });
      }
    }
  }
}
