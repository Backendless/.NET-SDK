using Weborb.Service;

namespace BackendlessAPI.Data
{
  public interface IBackendlessQuery
  {
    [SetClientClassMemberName( "offset" )]
    int Offset { get; set; }

    [SetClientClassMemberName( "pageSize" )]
    int PageSize { get; set; }

    IBackendlessQuery NewInstance();
  }
}