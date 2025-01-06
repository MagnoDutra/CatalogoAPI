using API.Context;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetProdutos()
    {
        var produtos = context.Produtos.ToList();
        return produtos;
    }

    [HttpGet("{id:int}")]
    public string GetProduto(int id)
    {
        return $"Get {id} produto route ";
    }

    [HttpPost]
    public string PostProduto()
    {
        return "Post route";
    }

    [HttpPut("{id:int}")]
    public string PutProduto(int id)
    {
        return "Put route";
    }

    [HttpDelete("{id:int}")]
    public string DeleteProduto(int id)
    {
        return "Delete route";
    }

}
