using System;

namespace API.Pagination;

public abstract class QueryStringParameters
{
  const int maxPageSize = 50;
  public int PageNumber { get; set; } = 1;
  private int pageSize = 10;
  public int PageSize { get { return pageSize; } set { pageSize = (value > maxPageSize) ? maxPageSize : value; } }
}
