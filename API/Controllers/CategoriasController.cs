using API.Context;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> GetCategorias()
    {
        var categorias = context.Categorias.ToList();

        if (categorias is null) return NotFound();

        return categorias;
    }


    [HttpGet("{id:int}")]
    public ActionResult<Categoria> GetCategoria(int id)
    {
        var categoria = context.Categorias.FirstOrDefault(cat => cat.CategoriaId == id);

        if (categoria is null) return NotFound();

        return categoria;
    }


}
