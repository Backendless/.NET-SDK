using System;
using System.Collections.Generic;
using BackendlessAPI.Exception;

namespace BackendlessAPI.Persistence
{
  public class LoadRelationsQueryBuilder<M>
  {
    private String relationName;
    private PagedQueryBuilder<LoadRelationsQueryBuilder<M>> pagedQueryBuilder;

    private LoadRelationsQueryBuilder()
    {
      pagedQueryBuilder = new PagedQueryBuilder<LoadRelationsQueryBuilder<M>>( this );
    }

    public static LoadRelationsQueryBuilder<M> Create()
    {
      return new LoadRelationsQueryBuilder<M>();
    }

    public BackendlessDataQuery Build()
    {
      if( string.IsNullOrEmpty( relationName ) )
        throw new System.Exception( "Unable to build a query for LoadRelations. Relation name is not set" );

      BackendlessDataQuery dataQuery = pagedQueryBuilder.Build();
      QueryOptions queryOptions = new QueryOptions();
      queryOptions.Related = new List<String>( new[] { relationName } );
      dataQuery.QueryOptions = queryOptions;
      return dataQuery;
    }

    public LoadRelationsQueryBuilder<M> SetRelationName( String relationName )
    {
      this.relationName = relationName;
      return this;
    }

    public LoadRelationsQueryBuilder<M> SetPageSize( int pageSize )
    {
      return pagedQueryBuilder.SetPageSize( pageSize );
    }

    public LoadRelationsQueryBuilder<M> setOffset( int offset )
    {
      return pagedQueryBuilder.SetOffset( offset );
    }

    public LoadRelationsQueryBuilder<M> prepareNextPage()
    {
      return pagedQueryBuilder.PrepareNextPage();
    }

    public LoadRelationsQueryBuilder<M> PreparePreviousPage()
    {
      return pagedQueryBuilder.PreparePreviousPage();
    }
  }
}
