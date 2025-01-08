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
    public ActionResult<IEnumerable<Produto>> GetProdutos()
    {
        var produtos = context.Produtos.AsNoTracking().ToList();

        if (produtos is null)
        {
            return NotFound();
        }

        return produtos;
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> GetProduto(int id)
    {
        var produto = context.Produtos.FirstOrDefault(produto => produto.ProdutoId == id);

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
