using System;
using System.Collections.Generic;
using BackendlessAPI.Exception;

namespace BackendlessAPI.Persistence
{
  public class GroupedData : GroupResult
  {
    private GroupingColumnValue groupBy;
    private Boolean hasNextPage = false;
    private List<GroupedData> items = new List<GroupedData>();

    public GroupedData(GroupingColumnValue groupBy, List<GroupedData> items)
    {
      this.groupBy = groupBy;
      this.items = items;
    }
  }
}