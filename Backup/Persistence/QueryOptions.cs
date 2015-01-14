using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Persistence
{
  public class QueryOptions
  {
    public QueryOptions()
    {
    }

    public QueryOptions( int pageSize, int offset )
    {
      PageSize = pageSize;
      Offset = offset;
    }

    public QueryOptions( int pageSize, int offset, string sortBy )
    {
      PageSize = pageSize;
      Offset = offset;
      SortBy = new List<String> {sortBy};
    }

    public QueryOptions( int pageSize, int offset, string sortBy, int relationsDepth )
    {
      PageSize = pageSize;
      Offset = offset;
      SortBy = new List<String> { sortBy };
      RelationsDepth = relationsDepth;
    }

    [SetClientClassMemberName( "pageSize" )]
    public int PageSize { get; set; }

    [SetClientClassMemberName( "offset" )]
    public int Offset { get; set; }

    [SetClientClassMemberName( "sortBy" )]
    public List<String> SortBy { get; set; }

    [SetClientClassMemberName( "relationsDepth" )]
    public int RelationsDepth { get; set; }

    [SetClientClassMemberName( "related" )]
    public List<string> Related { get; set; }

    public QueryOptions NewInstance()
    {
      return new QueryOptions {PageSize = PageSize, Offset = Offset, SortBy = SortBy, Related = Related};
    }
  }
}