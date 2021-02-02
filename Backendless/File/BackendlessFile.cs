using System;
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

    public Int32 Remove()
    {
      return Backendless.Files.Remove( FileURL );
    }

    public void Remove( AsyncCallback<Int32> callback )
    {
      Backendless.Files.Remove( FileURL, callback );
    }
  }
}