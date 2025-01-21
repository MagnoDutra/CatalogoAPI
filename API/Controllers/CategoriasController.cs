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
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
    {
        var categorias = await uof.CategoriaRepository.GetAllAsync();

        if (categorias is null) return NotFound("Não existem categorias...");

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasAsync([FromQuery] CategoriasParameters paginationParams)
    {
        var categorias = await uof.CategoriaRepository.GetCategoriasAsync(paginationParams);

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
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFilter([FromQuery] CategoriaFiltroNome catFilterParams)
    {
        var categorias = await uof.CategoriaRepository.GetCategoriaFilterNomeAsync(catFilterParams);

        return ObterCategorias(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
    {
        var categoria = await uof.CategoriaRepository.GetAsync(cat => cat.CategoriaId == id);

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
    public async Task<ActionResult<CategoriaDTO>> PostCategoria(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
        {
            logger.LogWarning("Dados inválidos");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDto.ToCategoria();

        var novaCategoria = uof.CategoriaRepository.Create(categoria!);
        await uof.CommitAsync();

        var novaCategoriaDto = novaCategoria.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto!.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> PutCategoria(int id, CategoriaDTO categoriaDto)
    {
        if (categoriaDto.CategoriaId != id)
        {
            logger.LogWarning("Id da requisição difere do id da categoria");
            return BadRequest("Id da requisição difere do id da categoria");
        }

        var categoria = categoriaDto.ToCategoria();

        uof.CategoriaRepository.Update(categoria!);
        await uof.CommitAsync();

        var categoriaDtoAtualizada = categoria!.ToCategoriaDTO();

        return Ok(categoriaDtoAtualizada);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> DeleteCategoria(int id)
    {
        var categoria = await uof.CategoriaRepository.GetAsync(cat => cat.CategoriaId == id);

        if (categoria == null)
        {
            logger.LogWarning($"Categoria com id {id} não encontrada");
            return BadRequest($"Categoria com id {id} não encontrada");
        }

        var categoriaExcluida = uof.CategoriaRepository.Delete(categoria);
        await uof.CommitAsync();

        var categoriaDtoExcluida = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluida);
    }
}
