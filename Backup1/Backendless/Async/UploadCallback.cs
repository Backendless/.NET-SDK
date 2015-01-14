namespace BackendlessAPI.Async
{
  public delegate void ProgressHandler( int response );

  public class UploadCallback
  {

    internal readonly ProgressHandler ProgressHandler;

    public UploadCallback( ProgressHandler progressHandler )
    {
      ProgressHandler = progressHandler;
    }
  }
}
