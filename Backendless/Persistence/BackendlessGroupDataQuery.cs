using System;
using System.Collections.Generic;
using BackendlessAPI.Data;
using Weborb.Service;

namespace BackendlessAPI.Persistence
{
  public class BackendlessGroupDataQuery : BackendlessDataQuery
  {
    private const Int32 GROUP_DEPTH = 3;

    [SetClientClassMemberName("groupPageSize")]
    public Int32 GroupPageSize { get; set; } = DEFAULT_PAGE_SIZE;

    [SetClientClassMemberName("RecordsPageSize")]
    public Int32 RecordsPageSize { get; set; } = DEFAULT_PAGE_SIZE;

    [SetClientClassMemberName("groupDepth")]
    public Int32 GroupDepth { get; set; } = GROUP_DEPTH;

    [SetClientClassMemberName("groupPath")]
    public List<GroupingColumnValue> GroupPath { get; set; }

    public BackendlessGroupDataQuery() : base() { }
  }
}