using System;
using API.DTOs;
using API.Models;

namespace API.Extensions;

public static class CategoriaDTOMappingExtensions
{
  public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
  {
    if (categoria is null) return null;

    var categoriaDto = new CategoriaDTO
    {
      CategoriaId = categoria.CategoriaId,
      Nome = categoria.Nome,
      ImagemUrl = categoria.ImagemUrl
    };

    return categoriaDto;
  }

  public static Categoria? ToCategoria(this CategoriaDTO dto)
  {
    if (dto is null) return null;

    var categoria = new Categoria()
    {
      CategoriaId = dto.CategoriaId,
      Nome = dto.Nome,
      ImagemUrl = dto.ImagemUrl
    };

    return categoria;
  }

  public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
  {
    if (categorias is null || !categorias.Any()) return new List<CategoriaDTO>();

    return categorias.Select(categoria => new CategoriaDTO
    {
      CategoriaId = categoria.CategoriaId,
      Nome = categoria.Nome,
      ImagemUrl = categoria.ImagemUrl
    }).ToList();

  }
}
