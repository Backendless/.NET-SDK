using BackendlessAPI.Exception;

namespace BackendlessAPI.Persistence
{
  public class PagedGroupQueryBuilder<Builder>
  {
    private int pageSize;
    private int groupPageSize;
    private int recordsPageSize;
    private int offset;


    private Builder builder;

    internal PagedGroupQueryBuilder(Builder builder)
    {
      pageSize = BackendlessDataQuery.DEFAULT_PAGE_SIZE;
      groupPageSize = BackendlessDataQuery.DEFAULT_PAGE_SIZE;
      recordsPageSize = BackendlessDataQuery.DEFAULT_PAGE_SIZE;
      offset = BackendlessDataQuery.DEFAULT_OFFSET;
      this.builder = builder;
    }

    internal Builder SetPageSize(int pageSize)
    {
      ValidatePageSize(pageSize);
      this.pageSize = pageSize;
      return builder;
    }

    internal Builder SetGroupPageSize(int groupPageSize)
    {
      ValidatePageSize(groupPageSize);
      this.groupPageSize = groupPageSize;
      return builder;
    }

    internal Builder SetRecordsPageSize(int recordsPageSize)
    {
      ValidatePageSize(recordsPageSize);
      this.recordsPageSize = recordsPageSize;
      return builder;
    }

    internal Builder SetOffset(int offset)
    {
      ValidateOffset(offset);
      this.offset = offset;
      return builder;
    }

    /**
    * Updates offset to point at next data page by adding pageSize.
    */
    internal Builder PrepareNextPage()
    {
      int offset = this.offset + pageSize;
      ValidateOffset(offset);
      this.offset = offset;

      return builder;
    }

    /**
     * Updates offset to point at previous data page by subtracting pageSize.
    */
    internal Builder PreparePerviousPage()
    {
      int offset = this.offset - pageSize;
      ValidateOffset(offset);
      this.offset = offset;

      return builder;
    }

    internal BackendlessGroupDataQuery Build()
    {
      ValidateOffset(offset);
      ValidatePageSize(pageSize);
      ValidatePageSize(recordsPageSize);
      ValidatePageSize(groupPageSize);

      BackendlessGroupDataQuery groupDataQuery = new BackendlessGroupDataQuery();
      groupDataQuery.PageSize = pageSize;
      groupDataQuery.GroupPageSize = groupPageSize;
      groupDataQuery.RecordsPageSize = recordsPageSize;
      groupDataQuery.Offset = offset;

      return groupDataQuery;
    }

    private void ValidateOffset(int offset)
    {
      if (offset < 0)
        throw new BackendlessException(ExceptionMessage.WRONG_OFFSET);
    }

    private void ValidatePageSize(int pageSize)
    {
      if (pageSize <= 0)
        throw new BackendlessException(ExceptionMessage.WRONG_PAGE_SIZE);
    }
  }
}