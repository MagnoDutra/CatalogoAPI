using API.Context;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(IProdutoRepository produtoRepository) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetProdutos()
    {
        var produtos = produtoRepository.GetAll();

        if (produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Produto> GetProduto(int id)
    {
        var produto = produtoRepository.GetByID(id);

        if (produto is null) return NotFound("Produto não encontrado.");

        return produto;
    }

    [HttpPost]
    public ActionResult PostProduto(Produto produto)
    {
        if (produto is null) return BadRequest();

        var novoProduto = produtoRepository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult PutProduto(int id, Produto produto)
    {
        if (produto.ProdutoId != id) return BadRequest();

        bool atualizado = produtoRepository.Update(produto);

        if (atualizado)
            return Ok(produto);
        else
            return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduto(int id)
    {
        var produto = produtoRepository.GetByID(id);

        if (produto is null) return NotFound("Produto não localizado...");

        bool produtoExcluido = produtoRepository.Delete(id);

        if (produtoExcluido)
            return Ok(produto);
        else
            return StatusCode(500, $"Falha ao deletar o produto com id = {id}");
    }

}
