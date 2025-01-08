using API.Context;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosAsync()
    {
        var produtos = await context.Produtos.AsNoTracking().ToListAsync();

        if (produtos is null)
        {
            return NotFound();
        }

        return produtos;
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> GetProdutoAsync(int id)
    {
        var produto = await context.Produtos.AsNoTracking().FirstOrDefaultAsync(produto => produto.ProdutoId == id);

        if (produto is null) return NotFound("Produto não encontrado.");

        return produto;
    }

    [HttpPost]
    public ActionResult PostProduto(Produto produto)
    {
        if (produto is null) return BadRequest();

        context.Produtos.Add(produto);
        context.SaveChanges();

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult PutProduto(int id, Produto produto)
    {
        if (produto.ProdutoId != id) return BadRequest();

        context.Entry(produto).State = EntityState.Modified;
        context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduto(int id)
    {
        var produto = context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        if (produto is null) return NotFound("Produto não localizado...");

        context.Produtos.Remove(produto);
        context.SaveChanges();

        return Ok(produto);
    }

}
