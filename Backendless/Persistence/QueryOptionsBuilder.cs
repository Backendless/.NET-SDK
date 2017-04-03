using System;
using System.Collections.Generic;

namespace BackendlessAPI.Persistence
{
  public class QueryOptionsBuilder<Builder>
  {
    private List<String> sortBy;
    private List<String> related;
    private int relationsDepth;
    private Builder builder;

    internal QueryOptionsBuilder( Builder builder )
    {
      sortBy = new List<String>();
      related = new List<String>();
      this.builder = builder;
    }

    internal QueryOptions Build()
    {
      QueryOptions queryOptions = new QueryOptions();
      queryOptions.Related = related;
      queryOptions.RelationsDepth = relationsDepth;
      queryOptions.SortBy = sortBy;
      return queryOptions;
    }

    /*--- Auto-generated code ---*/

    public List<String> GetSortBy()
    {
      return sortBy;
    }

    public Builder SetSortBy( List<String> sortBy )
    {
      this.sortBy = sortBy;
      return builder;
    }

    public Builder AddSortBy( String sortBy )
    {
      this.sortBy.Add( sortBy );
      return builder;
    }

    public List<String> GetRelated()
    {
      return related;
    }

    public Builder SetRelated( List<String> related )
    {
      this.related = related;
      return builder;
    }

    public int GetRelationsDepth()
    {
      return relationsDepth;
    }

    public Builder SetRelationsDepth( int relationsDepth )
    {
      this.relationsDepth = relationsDepth;
      return builder;
    }
  }
}
