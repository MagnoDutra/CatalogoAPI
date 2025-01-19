using System;
using API.Models;
using AutoMapper;

namespace API.DTOs.Mappings;

public class ProdutoDTOMappingProfile : Profile
{
  public ProdutoDTOMappingProfile()
  {
    CreateMap<Produto, ProdutoDTO>().ReverseMap();
  }
}
