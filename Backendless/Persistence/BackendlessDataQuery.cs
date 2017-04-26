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

    public BackendlessDataQuery( List<string> properties )
    {
      Properties = properties;
    }

    public BackendlessDataQuery( string whereClause )
    {
      WhereClause = whereClause;
    }

    public BackendlessDataQuery( QueryOptions queryOptions )
    {
      QueryOptions = queryOptions;
    }

    public BackendlessDataQuery( List<string> properties, string whereClause, QueryOptions queryOptions )
    {
      Properties = properties;
      WhereClause = whereClause;
      QueryOptions = queryOptions;
    }

    [SetClientClassMemberName( "whereClause" )]
    public string WhereClause { get; set; }

    [SetClientClassMemberName( "queryOptions" )]
    public QueryOptions QueryOptions { get; set; }

    [SetClientClassMemberName( "properties" )]
    public List<string> Properties { get; set; }

    [SetClientClassMemberName( "pageSize" )]
    public int PageSize { get;set; }

    [SetClientClassMemberName( "offset" )]
    public int Offset {get; set;}

    public IBackendlessQuery NewInstance()
    {
      return new BackendlessDataQuery {Properties = Properties, WhereClause = WhereClause, QueryOptions = QueryOptions};
    }
  }
}