using System;

namespace API.DTOs;

public class ProdutoDTOUpdateResponse
{
  public int ProdutoId { get; set; }

  public required string Nome { get; set; }

  public required string Descricao { get; set; }

  public required decimal Preco { get; set; }

  public required string ImagemUrl { get; set; }

  public float Estoque { get; set; }

  public DateTime DataCadastro { get; set; }

  public int CategoriaId { get; set; }

}
