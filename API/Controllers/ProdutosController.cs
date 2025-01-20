using API.Context;
using API.DTOs;
using API.Models;
using API.Pagination;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(IUnitOfWork uof, IMapper mapper) : ControllerBase
{
    //Exercicio de paginação
    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameter)
    {
        var produtos = uof.ProdutoRepository.GetProdutos(produtosParameter);

        var produtosDto = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

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

    [HttpPatch("{id:int}")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchDoc)
    {
        if (patchDoc is null || id <= 0)
            return BadRequest();

        var produto = uof.ProdutoRepository.Get(prod => prod.ProdutoId == id);

        if (produto is null)
            return NotFound();

        var produtoUpdateRequest = mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchDoc.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        mapper.Map(produtoUpdateRequest, produto);

        uof.ProdutoRepository.Update(produto);
        uof.Commit();

        return Ok(mapper.Map<ProdutoDTOUpdateResponse>(produto));
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