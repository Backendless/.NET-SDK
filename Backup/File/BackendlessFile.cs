using BackendlessAPI.Async;
using Weborb.Service;

namespace BackendlessAPI.File
{
  public class BackendlessFile
  {
    public BackendlessFile( string fileURL )
    {
      FileURL = fileURL;
    }

    [SetClientClassMemberName( "fileURL" )]
    public string FileURL { get; set; }

    public void Remove()
    {
      Backendless.Files.Remove( FileURL );
    }

    public void Remove( AsyncCallback<object> callback )
    {
      Backendless.Files.Remove( FileURL, callback );
    }
  }
}