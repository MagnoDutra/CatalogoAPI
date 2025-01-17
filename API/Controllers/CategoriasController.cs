using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> GetCategorias()
    {

        var categorias = repository.GetCategorias();
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetCategoria(int id)
    {
        var categoria = repository.GetCategoriaById(id);

        if (categoria is null)
        {
            logger.LogWarning($"Categoria com id {id} não encontrada");
            return NotFound($"Categoria com id {id} não encontrada");
        }

        return Ok(categoria);
    }

    // [HttpGet("produtos")]
    // public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    // {
    //     var categorias = context.Categorias.Include(cat => cat.Produtos).ToList();

    //     if (categorias is null) return NotFound();

    //     return Ok(categorias);
    // }

    [HttpPost]
    public ActionResult PostCategoria(Categoria categoria)
    {
        if (categoria is null)
        {
            logger.LogWarning("Dados inválidos");
            return BadRequest("Dados inválidos");
        }

        var novaCategoria = repository.Create(categoria);

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoria.CategoriaId }, novaCategoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult PutCategoria(int id, Categoria categoria)
    {
        if (categoria.CategoriaId != id)
        {
            logger.LogWarning("Id da requisição difere do id da categoria");
            return BadRequest("Id da requisição difere do id da categoria");
        }

        repository.Update(categoria);
        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> DeleteCategoria(int id)
    {
        var categoria = repository.GetCategoriaById(id);

        if (categoria == null)
        {
            logger.LogWarning($"Categoria com id {id} não encontrada");
            return BadRequest($"Categoria com id {id} não encontrada");
        }

        var categoriaExcluida = repository.Delete(id);
        return Ok(categoriaExcluida);
    }
}
