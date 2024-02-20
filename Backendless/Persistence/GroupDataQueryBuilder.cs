using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Persistence
{

  public class GroupDataQueryBuilder : DataQueryBuilder
  {
    private PagedGroupQueryBuilder<GroupDataQueryBuilder> pagedQueryBuilder;
    private QueryOptionsBuilder<GroupDataQueryBuilder> queryOptionsBuilder;
    public List<GroupingColumnValue> GroupPath { get; set; }
    public Int32 GroupDepth { get; set; }
    private List<String> groupBy;

    private GroupDataQueryBuilder() : base()
    {
      pagedQueryBuilder = new PagedGroupQueryBuilder<GroupDataQueryBuilder>(this);
      queryOptionsBuilder = new QueryOptionsBuilder<GroupDataQueryBuilder>(this);
      GroupPath = new List<GroupingColumnValue>();
    }

    public static new GroupDataQueryBuilder Create()
    {
      return new GroupDataQueryBuilder();
    }

    public new BackendlessGroupDataQuery Build()
    {
      BackendlessGroupDataQuery dataQuery = pagedQueryBuilder.Build();
      dataQuery.Distinct = GetDistinct();
      dataQuery.QueryOptions = queryOptionsBuilder.Build();
      dataQuery.Properties = GetProperties();
      dataQuery.ExcludeProperties = GetProperties();
      dataQuery.WhereClause = GetWhereClause();
      dataQuery.GroupBy = groupBy;
      dataQuery.GroupDepth = GroupDepth;
      dataQuery.GroupPath = GroupPath;

      return dataQuery;
    }
  }
}