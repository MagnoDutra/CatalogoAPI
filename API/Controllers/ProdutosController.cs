using API.Context;
using API.DTOs;
using API.Models;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(IUnitOfWork uof, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutos()
    {
        var produtos = uof.ProdutoRepository.GetAll();

        if (produtos is null)
        {
            return NotFound();
        }

        var produtosDto = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> GetProduto(int id)
    {
        var produto = uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null) return NotFound("Produto não encontrado.");

        var produtosDto = mapper.Map<ProdutoDTO>(produto);

        return Ok(produtosDto);
    }

    [HttpGet("categoria/{id:int}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosByCat(int id)
    {
        var produtos = uof.ProdutoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
        {
            return NotFound();
        }

        var produtosDto = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> PostProduto(ProdutoDTO produtoDto)
    {
        if (produtoDto is null) return BadRequest();

        var produto = mapper.Map<Produto>(produtoDto);

        var novoProduto = uof.ProdutoRepository.Create(produto);
        uof.Commit();

        var novoProdutoDto = mapper.Map<ProdutoDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> PutProduto(int id, ProdutoDTO dto)
    {
        if (dto.ProdutoId != id) return BadRequest();

        var produto = mapper.Map<Produto>(dto);

        var produtoAtualizado = uof.ProdutoRepository.Update(produto);
        uof.Commit();

        var dtoAtualizado = mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(dtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> DeleteProduto(int id)
    {
        var produto = uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null) return NotFound("Produto não localizado...");

        uof.ProdutoRepository.Delete(produto);
        uof.Commit();

        var dto = mapper.Map<ProdutoDTO>(produto);

        return Ok(dto);
    }

}
