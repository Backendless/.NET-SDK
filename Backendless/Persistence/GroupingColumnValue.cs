using System;

namespace BackendlessAPI.Persistence
{
  public class GroupingColumnValue
  {
    private String column;
    private Object value;

    public GroupingColumnValue(String field, Object value)
    {
      column = field;
      this.value = value;
    }
  }
}