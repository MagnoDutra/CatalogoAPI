using API.Context;
using API.Filters;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController(AppDbContext context, IConfiguration config) : ControllerBase
{
    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
    {
        try
        {
            var categorias = await context.Categorias.AsNoTracking().ToListAsync();
            return Ok(categorias);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias");
        }
    }



    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetCategoria(int id)
    {
        var categoria = context.Categorias.FirstOrDefault(cat => cat.CategoriaId == id);

        if (categoria is null) return NotFound();

        return Ok(categoria);
    }

    // GET
    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        var categorias = context.Categorias.Include(cat => cat.Produtos).ToList();

        if (categorias is null) return NotFound();

        return Ok(categorias);
    }


    [HttpPost]
    public ActionResult PostCategoria(Categoria categoria)
    {
        if (categoria is null) return BadRequest();

        context.Categorias.Add(categoria);
        context.SaveChanges();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult PutCategoria(int id, Categoria categoria)
    {
        if (categoria.CategoriaId != id) return BadRequest();

        context.Entry(categoria).State = EntityState.Modified;
        context.SaveChanges();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> DeleteCategoria(int id)
    {
        var categoria = context.Categorias.FirstOrDefault(cat => cat.CategoriaId == id);

        if (categoria == null) return BadRequest();

        context.Categorias.Remove(categoria);
        context.SaveChanges();

        return Ok(categoria);
    }

    [HttpGet("{nome}")]
    public string GetString(IMeuServico meuServico, string nome)
    {
        return meuServico.Saudacao(nome);
    }
}
