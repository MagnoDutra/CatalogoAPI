using API.DTOs;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategorias()
    {
        var categorias = uof.CategoriaRepository.GetAll();

        if (categorias is null) return NotFound("Não existem categorias...");

        var categoriasDto = new List<CategoriaDTO>();
        foreach (var categoria in categorias)
        {
            var categoriaDto = new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };

            categoriasDto.Add(categoriaDto);
        }

        return Ok(categoriasDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> GetCategoria(int id)
    {
        var categoria = uof.CategoriaRepository.Get(cat => cat.CategoriaId == id);

        if (categoria is null)
        {
            logger.LogWarning($"Categoria com id {id} não encontrada");
            return NotFound($"Categoria com id {id} não encontrada");
        }

        var categoriaDto = new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };

        return Ok(categoriaDto);
    }

    // [HttpGet("produtos")]
    // public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    // {
    //     var categorias = context.Categorias.Include(cat => cat.Produtos).ToList();

    //     if (categorias is null) return NotFound();

    //     return Ok(categorias);
    // }

    [HttpPost]
    public ActionResult<CategoriaDTO> PostCategoria(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
        {
            logger.LogWarning("Dados inválidos");
            return BadRequest("Dados inválidos");
        }

        var categoria = new Categoria()
        {
            CategoriaId = categoriaDto.CategoriaId,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl
        };

        var novaCategoria = uof.CategoriaRepository.Create(categoria);
        uof.Commit();

        var novaCategoriaDto = new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> PutCategoria(int id, CategoriaDTO categoriaDto)
    {
        if (categoriaDto.CategoriaId != id)
        {
            logger.LogWarning("Id da requisição difere do id da categoria");
            return BadRequest("Id da requisição difere do id da categoria");
        }

        var categoria = new Categoria()
        {
            CategoriaId = categoriaDto.CategoriaId,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl
        };

        uof.CategoriaRepository.Update(categoria);
        uof.Commit();

        var categoriaDtoAtualizada = new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };
        return Ok(categoriaDtoAtualizada);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> DeleteCategoria(int id)
    {
        var categoria = uof.CategoriaRepository.Get(cat => cat.CategoriaId == id);

        if (categoria == null)
        {
            logger.LogWarning($"Categoria com id {id} não encontrada");
            return BadRequest($"Categoria com id {id} não encontrada");
        }

        var categoriaExcluida = uof.CategoriaRepository.Delete(categoria);
        uof.Commit();

        var categoriaDtoExcluida = new CategoriaDTO()
        {
            CategoriaId = categoriaExcluida.CategoriaId,
            Nome = categoriaExcluida.Nome,
            ImagemUrl = categoriaExcluida.ImagemUrl
        };

        return Ok(categoriaExcluida);
    }
}
