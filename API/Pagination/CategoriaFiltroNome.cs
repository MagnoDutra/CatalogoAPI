using System;

namespace API.Pagination;

public class CategoriaFiltroNome : QueryStringParameters
{
  public string? Nome { get; set; }
}
