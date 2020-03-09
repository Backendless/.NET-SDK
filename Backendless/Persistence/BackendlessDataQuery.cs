using System;
using System.Collections.Generic;
using BackendlessAPI.Data;
using Weborb.Service;

namespace BackendlessAPI.Persistence
{
  public class BackendlessDataQuery : IBackendlessQuery
  {
    public const int DEFAULT_PAGE_SIZE = 10;
    public const int DEFAULT_OFFSET = 0;

    public BackendlessDataQuery()
    {
    }

    public BackendlessDataQuery( List<String> properties )
    {
      Properties = properties;
    }

    public BackendlessDataQuery( String whereClause )
    {
      WhereClause = whereClause;
    }

    public BackendlessDataQuery( QueryOptions queryOptions )
    {
      QueryOptions = queryOptions;
    }

    public BackendlessDataQuery( List<String> properties, String whereClause, QueryOptions queryOptions )
    {
      Properties = properties;
      WhereClause = whereClause;
      QueryOptions = queryOptions;
    }

    public BackendlessDataQuery( List<String> properties, String whereClause, QueryOptions queryOptions, List<String> groupBy, String havingClause )
    {
      Properties = properties;
      WhereClause = whereClause;
      QueryOptions = queryOptions;
      GroupBy = groupBy;
      HavingClause = havingClause;
    }

    [SetClientClassMemberName( "whereClause" )]
    public String WhereClause { get; set; }

    [SetClientClassMemberName( "groupBy" )]
    public List<String> GroupBy { get; set; }

    [SetClientClassMemberName( "havingClause" )]
    public String HavingClause { get; set; }

    [SetClientClassMemberName( "queryOptions" )]
    public QueryOptions QueryOptions { get; set; }

    [SetClientClassMemberName( "properties" )]
    public List<String> Properties { get; set; }

    [SetClientClassMemberName( "pageSize" )]
    public int PageSize { get;set; }

    [SetClientClassMemberName( "offset" )]
    public int Offset { get; set; }

    public IBackendlessQuery NewInstance()
    {
      return new BackendlessDataQuery {Properties = Properties, WhereClause = WhereClause, QueryOptions = QueryOptions, GroupBy = GroupBy, HavingClause = HavingClause};
    }
  }
}