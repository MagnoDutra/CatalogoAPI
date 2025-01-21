using API.Context;
using API.DTOs;
using API.Models;
using API.Pagination;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController(IUnitOfWork uof, IMapper mapper) : ControllerBase
{
    //Exercicio de paginação
    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameter)
    {
        var produtos = await uof.ProdutoRepository.GetProdutosAsync(produtosParameter);

        return ObterProdutos(produtos);
    }

    //Exercicio Filtro
    [HttpGet("filter/preco/pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFiltroPreco)
    {
        var produtos = await uof.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFiltroPreco);

        return ObterProdutos(produtos);
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDto = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos()
    {
        var produtos = await uof.ProdutoRepository.GetAllAsync();

        if (produtos is null)
        {
            return NotFound();
        }

        var produtosDto = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<ProdutoDTO>> GetProduto(int id)
    {
        var produto = await uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

        if (produto is null) return NotFound("Produto não encontrado.");

        var produtosDto = mapper.Map<ProdutoDTO>(produto);

        return Ok(produtosDto);
    }

    [HttpGet("categoria/{id:int}")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosByCat(int id)
    {
        var produtos = await uof.ProdutoRepository.GetProdutosPorCategoriaAsync(id);

        if (produtos is null)
        {
            return NotFound();
        }

        var produtosDto = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDTO>> PostProduto(ProdutoDTO produtoDto)
    {
        if (produtoDto is null) return BadRequest();

        var produto = mapper.Map<Produto>(produtoDto);

        var novoProduto = uof.ProdutoRepository.Create(produto);
        await uof.CommitAsync();

        var novoProdutoDto = mapper.Map<ProdutoDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchDoc)
    {
        if (patchDoc is null || id <= 0)
            return BadRequest();

        var produto = await uof.ProdutoRepository.GetAsync(prod => prod.ProdutoId == id);

        if (produto is null)
            return NotFound();

        var produtoUpdateRequest = mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchDoc.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        mapper.Map(produtoUpdateRequest, produto);

        uof.ProdutoRepository.Update(produto);
        await uof.CommitAsync();

        return Ok(mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> PutProduto(int id, ProdutoDTO dto)
    {
        if (dto.ProdutoId != id) return BadRequest();

        var produto = mapper.Map<Produto>(dto);

        var produtoAtualizado = uof.ProdutoRepository.Update(produto);
        await uof.CommitAsync();

        var dtoAtualizado = mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(dtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> DeleteProduto(int id)
    {
        var produto = await uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

        if (produto is null) return NotFound("Produto não localizado...");

        uof.ProdutoRepository.Delete(produto);
        await uof.CommitAsync();

        var dto = mapper.Map<ProdutoDTO>(produto);

        return Ok(dto);
    }

}