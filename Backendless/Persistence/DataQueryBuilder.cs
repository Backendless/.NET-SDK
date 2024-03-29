﻿using System;
using System.Collections.Generic;

namespace BackendlessAPI.Persistence
{
  public class DataQueryBuilder
  {
    private PagedQueryBuilder<DataQueryBuilder> pagedQueryBuilder;
    private QueryOptionsBuilder<DataQueryBuilder> queryOptionsBuilder;
    private Boolean distinct = false;
    private List<String> properties;
    private List<String> excludeProperties;
    private String whereClause;
    private List<String> groupBy;
    private String havingClause;

    protected DataQueryBuilder()
    {
      properties = new List<String>();
      excludeProperties = new List<String>();
      pagedQueryBuilder = new PagedQueryBuilder<DataQueryBuilder>( this );
      queryOptionsBuilder = new QueryOptionsBuilder<DataQueryBuilder>( this );
      groupBy = new List<String>();
      havingClause = "";
    }

    public static DataQueryBuilder Create()
    {
      return new DataQueryBuilder();
    }

    public BackendlessDataQuery Build()
    {
      BackendlessDataQuery dataQuery = pagedQueryBuilder.Build();
      dataQuery.Distinct = distinct;
      dataQuery.QueryOptions = queryOptionsBuilder.Build();
      dataQuery.Properties = properties;
      dataQuery.ExcludeProperties = excludeProperties;
      dataQuery.WhereClause = whereClause;
      dataQuery.GroupBy = groupBy;
      dataQuery.HavingClause = havingClause;
      return dataQuery;
    }

    public DataQueryBuilder SetDistinct( Boolean distinct )
    {
      this.distinct = distinct;
      return this;
    }

    public Boolean GetDistinct()
    {
      return distinct;
    }

    public DataQueryBuilder SetPageSize( int pageSize )
    {
      return pagedQueryBuilder.SetPageSize( pageSize );
    }

    public DataQueryBuilder SetOffset( int offset )
    {
      return pagedQueryBuilder.SetOffset( offset );
    }

    public DataQueryBuilder PrepareNextPage()
    {
      return pagedQueryBuilder.PrepareNextPage();
    }

    public DataQueryBuilder PreparePreviousPage()
    {
      return pagedQueryBuilder.PreparePreviousPage();
    }

    public List<String> GetProperties()
    {
      return properties;
    }

    public DataQueryBuilder SetProperties( List<String> properties )
    {
      this.properties = properties;
      return this;
    }

    public DataQueryBuilder SetProperties( params String[] properties )
    {
      this.properties = new List<String>();
      this.properties.AddRange( properties );
      return this;
    }

    public DataQueryBuilder AddProperty( String property )
    {
      this.properties.Add( property );
      return this;
    }

    public DataQueryBuilder AddProperties( List<String> properties )
    {
      this.properties.AddRange( properties );
      return this;
    }

    public DataQueryBuilder AddProperties( params String[] properties )
    {
      this.properties.AddRange( properties );
      return this;
    }

    public DataQueryBuilder AddAllProperties()
    {
      return AddProperty( "*" );
    }

    public List<String> GetExcludedProperties()
    {
      return new List<String>( excludeProperties );
    }

    public DataQueryBuilder ExcludeProperties( List<String> excludeProperties )
    {
      this.excludeProperties.Clear();

      if( excludeProperties != null )
        this.excludeProperties.AddRange( excludeProperties );

      return this;
    }

    public DataQueryBuilder ExcludeProperties( params String[] excludeProperties )
    {
      this.excludeProperties.Clear();

      if( excludeProperties != null )
        this.excludeProperties.AddRange( excludeProperties );

      return this;
    }

    public DataQueryBuilder ExcludeProperty( String excludeProperty )
    {
      if( excludeProperties != null )
        this.excludeProperties.Add( excludeProperty );
      return this;
    }

    public String GetWhereClause()
    {
      return whereClause;
    }

    public DataQueryBuilder SetWhereClause( String whereClause )
    {
      this.whereClause = whereClause;
      return this;
    }

    public List<String> GetSortBy()
    {
      return queryOptionsBuilder.GetSortBy();
    }

    public DataQueryBuilder SetSortBy( List<String> sortBy )
    {
      return queryOptionsBuilder.SetSortBy( sortBy );
    }

    public DataQueryBuilder AddSortBy( String sortBy )
    {
      return queryOptionsBuilder.AddSortBy( sortBy );
    }

    public List<String> GetRelated()
    {
      return queryOptionsBuilder.GetRelated();
    }

    public DataQueryBuilder AddRelated( String related )
    {
      return queryOptionsBuilder.AddRelated( related );
    }

    public DataQueryBuilder SetRelated( List<String> related )
    {
      return queryOptionsBuilder.SetRelated( related );
    }

    public int? GetRelationsDepth()
    {
      return queryOptionsBuilder.GetRelationsDepth();
    }

    public DataQueryBuilder SetRelationsDepth( int? relationsDepth )
    {
      return queryOptionsBuilder.SetRelationsDepth( relationsDepth );
    }

    public int? GetRelationsPageSize()
    {
      return queryOptionsBuilder.GetRelationsPageSize();
    }
    public DataQueryBuilder SetRelationsPageSize( int? relationsPageSize )
    {
      return queryOptionsBuilder.SetRelationsPageSize( relationsPageSize );
    }

    public DataQueryBuilder SetGroupBy( List<String> groupBy )
    {
      this.groupBy = new List<String>();
      this.groupBy.AddRange( groupBy );
      return this;
    }

    public DataQueryBuilder SetGroupBy( String groupBy )
    {
      this.groupBy = new List<String> { groupBy };
      return this;
    }

    public DataQueryBuilder AddGroupBy( List<String> groupBy )
    {
      this.groupBy = this.groupBy ?? new List<String>();
      this.groupBy.AddRange( groupBy );
      return this;
    }

    public DataQueryBuilder AddGroupBy( String groupBy )
    {
      this.groupBy = this.groupBy ?? new List<String>();
      this.groupBy.Add( groupBy );
      return this;
    }

    public DataQueryBuilder SetHavingClause( String havingClause )
    {
      if ( havingClause == null )
        havingClause = "";

      this.havingClause = havingClause;
      return this;   
    }
  }
}
