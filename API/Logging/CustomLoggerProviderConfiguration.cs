using System;

namespace API.Logging;

public class CustomLoggerProviderConfiguration
{
  public int EventId { get; set; } = 0;
  public LogLevel LogLevel { get; set; } = LogLevel.Warning;
}
