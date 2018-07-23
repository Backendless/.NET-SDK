using System;
using Weborb.Service;

namespace BackendlessAPI.RT.Data
{
  public class BulkEvent
  {
    [SetClientClassMemberName( "whereClause" )]
    public String WhereClause
    {
      get; set;
    }

    [SetClientClassMemberName( "count" )]
    public int Count
    {
      get; set;
    }

    public override string ToString()
    {
      return "BulkEvent{whereClause='" + WhereClause + '\'' + ", count=" + Count + '}';
    }
  }
}
