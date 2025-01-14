using System;
using System.Collections.Concurrent;

namespace API.Logging;

public class CustomLoggerProvider(CustomLoggerProviderConfiguration config) : ILoggerProvider
{
  readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

  public ILogger CreateLogger(string categoryName)
  {
    return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, config));
  }

  public void Dispose()
  {
    loggers.Clear();
  }
}
