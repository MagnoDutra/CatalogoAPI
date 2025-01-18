using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CategoriaDTO
{
  public int CategoriaId { get; set; }

  [Required]
  [StringLength(80)]
  public required string Nome { get; set; }


  [Required]
  [StringLength(300)]
  public required string ImagemUrl { get; set; }
}
