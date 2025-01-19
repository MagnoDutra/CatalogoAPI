using System;
using API.Models;
using AutoMapper;

namespace API.DTOs.Mappings;

public class ProdutoDTOMappingProfile : Profile
{
  public ProdutoDTOMappingProfile()
  {
    CreateMap<Produto, ProdutoDTO>().ReverseMap();
    CreateMap<Produto, ProdutoDTOUpdateRequest>().ReverseMap();
    CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();
  }
}
