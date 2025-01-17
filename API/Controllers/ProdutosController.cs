using API.Context;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(IUnitOfWork uof) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetProdutos()
    {
        var produtos = uof.ProdutoRepository.GetAll();

        if (produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> GetProduto(int id)
    {
        var produto = uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null) return NotFound("Produto não encontrado.");

        return produto;
    }

    [HttpGet("categoria/{id:int}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosByCat(int id)
    {
        var produtos = uof.ProdutoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);
    }

    [HttpPost]
    public ActionResult PostProduto(Produto produto)
    {
        if (produto is null) return BadRequest();

        var novoProduto = uof.ProdutoRepository.Create(produto);
        uof.Commit();

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult PutProduto(int id, Produto produto)
    {
        if (produto.ProdutoId != id) return BadRequest();

        uof.ProdutoRepository.Update(produto);
        uof.Commit();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduto(int id)
    {
        var produto = uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null) return NotFound("Produto não localizado...");

        uof.ProdutoRepository.Delete(produto);
        uof.Commit();

        return Ok(produto);
    }

}
