using System;

namespace API.Logging;

public class CustomLogger(string name, CustomLoggerProviderConfiguration config) : ILogger
{
  public IDisposable? BeginScope<TState>(TState state) where TState : notnull
  {
    return null;
  }

  public bool IsEnabled(LogLevel logLevel)
  {
    return logLevel == config.LogLevel;
  }

  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
  {
    string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

    EscreverTextoNoArquivo(mensagem);
  }

  private void EscreverTextoNoArquivo(string mensagem)
  {
    string caminhoArquivoLog = @"/Users/jmagno/Cursos/rocketseat-csharp/dados/log/Magno_Log.txt";

    using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
    {
      try
      {
        streamWriter.WriteLine(mensagem);
        streamWriter.Close();
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
