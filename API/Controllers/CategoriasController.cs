using API.DTOs;
using API.Extensions;
using API.Models;
using API.Pagination;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategorias()
    {
        var categorias = uof.CategoriaRepository.GetAllAsync();

        if (categorias is null) return NotFound("Não existem categorias...");

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategorias([FromQuery] CategoriasParameters paginationParams)
    {
        var categorias = uof.CategoriaRepository.GetCategorias(paginationParams);

        return ObterCategorias(categorias);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("filtro/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFilter([FromQuery] CategoriaFiltroNome catFilterParams)
    {
        var categorias = uof.CategoriaRepository.GetCategoriaFilterNome(catFilterParams);

        return ObterCategorias(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> GetCategoria(int id)
    {
        var categoria = uof.CategoriaRepository.GetAsync(cat => cat.CategoriaId == id);

        if (categoria is null)
        {
            logger.LogWarning($"Categoria com id {id} não encontrada");
            return NotFound($"Categoria com id {id} não encontrada");
        }

        var categoriaDto = categoria.ToCategoriaDTO();

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

        var categoria = categoriaDto.ToCategoria();

        var novaCategoria = uof.CategoriaRepository.Create(categoria!);
        uof.Commit();

        var novaCategoriaDto = novaCategoria.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto!.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> PutCategoria(int id, CategoriaDTO categoriaDto)
    {
        if (categoriaDto.CategoriaId != id)
        {
            logger.LogWarning("Id da requisição difere do id da categoria");
            return BadRequest("Id da requisição difere do id da categoria");
        }

        var categoria = categoriaDto.ToCategoria();

        uof.CategoriaRepository.Update(categoria!);
        uof.Commit();

        var categoriaDtoAtualizada = categoria!.ToCategoriaDTO();

        return Ok(categoriaDtoAtualizada);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> DeleteCategoria(int id)
    {
        var categoria = uof.CategoriaRepository.GetAsync(cat => cat.CategoriaId == id);

        if (categoria == null)
        {
            logger.LogWarning($"Categoria com id {id} não encontrada");
            return BadRequest($"Categoria com id {id} não encontrada");
        }

        var categoriaExcluida = uof.CategoriaRepository.Delete(categoria);
        uof.Commit();

        var categoriaDtoExcluida = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluida);
    }
}
