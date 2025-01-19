using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs;

public class ProdutoDTO
{
  public int ProdutoId { get; set; }

  [Required]
  [StringLength(80)]
  public required string Nome { get; set; }

  [Required]
  [StringLength(300)]
  public required string Descricao { get; set; }

  [Required]
  public required decimal Preco { get; set; }

  [Required]
  [StringLength(300)]
  public required string ImagemUrl { get; set; }

  public int CategoriaId { get; set; }
}
