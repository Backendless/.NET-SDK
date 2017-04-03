using System;
using BackendlessAPI.Exception;

namespace BackendlessAPI.Persistence
{
  internal class PagedQueryBuilder<Builder>
  {
    private int pageSize;
    private int offset;

    private Builder builder;

    internal PagedQueryBuilder( Builder builder )
    {
      pageSize = BackendlessDataQuery.DEFAULT_PAGE_SIZE;
      offset = BackendlessDataQuery.DEFAULT_OFFSET;
      this.builder = builder;
    }

    internal Builder SetPageSize( int pageSize )
    {
      ValidatePageSize( pageSize );
      this.pageSize = pageSize;
      return builder;
    }

    internal Builder SetOffset( int offset )
    {
      ValidateOffset( offset );
      this.offset = offset;
      return builder;
    }

    /**
     * Updates offset to point at next data page by adding pageSize.
     */
    internal Builder PrepareNextPage()
    {
      int offset = this.offset + pageSize;
      ValidateOffset( offset );
      this.offset = offset;

      return builder;
    }

    /**
     * Updates offset to point at previous data page by subtracting pageSize.
     */
    internal Builder PreparePreviousPage()
    {
      int offset = this.offset - pageSize;
      ValidateOffset( offset );
      this.offset = offset;

      return builder;
    }

    internal BackendlessDataQuery Build()
    {
      ValidateOffset( offset );
      ValidatePageSize( pageSize );

      BackendlessDataQuery dataQuery = new BackendlessDataQuery();
      dataQuery.PageSize = pageSize;
      dataQuery.Offset = offset;

      return dataQuery;
    }


    private void ValidateOffset( int offset )
    {
      if( offset < 0 )
        throw new BackendlessException( ExceptionMessage.WRONG_OFFSET );
    }

    private void ValidatePageSize( int pageSize )
    {
      if( pageSize <= 0 )
        throw new BackendlessException( ExceptionMessage.WRONG_PAGE_SIZE );
    }
  }
}
