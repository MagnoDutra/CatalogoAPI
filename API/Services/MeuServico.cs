using System;

namespace API.Services;

public class MeuServico : IMeuServico
{
  public string Saudacao(string nome)
  {
    return $"Olá {nome}";
  }
}
