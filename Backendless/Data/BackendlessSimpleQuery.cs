using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Service;

namespace BackendlessAPI.Data
{
  public class BackendlessSimpleQuery : IBackendlessQuery
  {
    public static int DEFAULT_PAGE_SIZE = 10;
    public static int DEFAULT_OFFSET = 0;
    public static IBackendlessQuery DEFAULT = new BackendlessSimpleQuery( DEFAULT_PAGE_SIZE, DEFAULT_OFFSET );

    public BackendlessSimpleQuery()
    {
    }

    public BackendlessSimpleQuery( int pagesize, int offset )
    {
      PageSize = pagesize;
      Offset = offset;
    }

    [SetClientClassMemberName( "offset" )]
    public int Offset { get; set; }

    [SetClientClassMemberName( "pageSize" )]
    public int PageSize { get; set; }

    public IBackendlessQuery NewInstance()
    {
      return new BackendlessSimpleQuery( PageSize, Offset );
    }
  }
}
