using System;
using System.Collections.Generic;
using BackendlessAPI.Exception;

namespace BackendlessAPI.Persistence
{
  public class GroupResult
  {
    private Boolean hasNextPage = false;
    private List<GroupedData> items = new List<GroupedData>();



    public Boolean IsGroups()
    {
      if (items.Count == 0)
      {
        return false;
      }

      return true;
    }

    public List<GroupedData> GetGroupedData()
    {
      if(!IsGroups())
      {
        throw new BackendlessException("List of items doesn't contain instances of GroupedData."
                                         + " Use GroupResult#GetPlainItems to access data.");
      }

      return items;
    }

    public List<GroupedData> GetPlainItems()
    {
      if (IsGroups())
      {
        throw new BackendlessException("List of items contains instances of GroupedData. " +
          "Use GroupResult#GetGroupedData to access data.");

      }

      return items;
    }
  }
}