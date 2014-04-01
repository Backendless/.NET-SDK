using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.File;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BackendlessAPI.Service
{
  public class FileService
  {
    private static readonly Regex SERVER_ERROR_REGEXP =
      new Regex( "\"message\":\"(?<message>[^\"]*)\",\"code\":(?<code>[\\d]*)", RegexOptions.Compiled );

    private static readonly Regex SERVER_RESULT_REGEXP = new Regex( "\"fileURL\":\"(?<fileUrl>[^\"]*)",
                                                                    RegexOptions.Compiled );

    private const string FILE_MANAGER_SERVER_ALIAS = "com.backendless.services.file.FileService";
    private const int UPLOAD_BUFFER_DEFAULT_LENGTH = 8192;

    public FileService()
    {
    }

    public void Upload( FileStream fileStream, string remotePath, AsyncCallback<BackendlessFile> callback )
    {
      Upload( fileStream, remotePath, new EmptyUploadCallback(), callback );
    }

    public void Upload( FileStream fileStream, string remotePath, UploadCallback uploadCallback, AsyncCallback<BackendlessFile> callback )
    {
      if( string.IsNullOrEmpty( remotePath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      if( fileStream == null || string.IsNullOrEmpty( Path.GetFileName( fileStream.Name ) ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_FILE );

      MakeFileUpload( fileStream, remotePath, uploadCallback, callback );
    }

    public void Remove( string fileUrl )
    {
      if( string.IsNullOrEmpty( fileUrl ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeSync<object>( FILE_MANAGER_SERVER_ALIAS, "deleteFileOrDirectory",
                                  new Object[] {Backendless.AppId, Backendless.VersionNum, fileUrl} );
    }

    public void Remove( string fileUrl, AsyncCallback<object> callback )
    {
      if( string.IsNullOrEmpty( fileUrl ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeAsync<object>( FILE_MANAGER_SERVER_ALIAS, "deleteFileOrDirectory",
                                   new Object[] {Backendless.AppId, Backendless.VersionNum, fileUrl}, callback );
    }

    public void RemoveDirectory( string directoryPath )
    {
      if( string.IsNullOrEmpty( directoryPath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeSync<object>( FILE_MANAGER_SERVER_ALIAS, "deleteFileOrDirectory",
                                  new Object[] {Backendless.AppId, Backendless.VersionNum, directoryPath} );
    }

    public void RemoveDirectory( string directoryPath, AsyncCallback<object> callback )
    {
      if( string.IsNullOrEmpty( directoryPath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeAsync<object>( FILE_MANAGER_SERVER_ALIAS, "deleteFileOrDirectory",
                                   new Object[] {Backendless.AppId, Backendless.VersionNum, directoryPath}, callback );
    }

    private void MakeFileUpload( FileStream fileStream, string path, UploadCallback uploadCallback,
                                 AsyncCallback<BackendlessFile> callback )
    {
      string boundary = DateTime.Now.Ticks.ToString( "x" );
      byte[] boundaryBytes = Encoding.UTF8.GetBytes( "\r\n--" + boundary + "--\r\n" );

      var fileName = Path.GetFileName( fileStream.Name );
      var sb = new StringBuilder();
      sb.Append( "--" );
      sb.Append( boundary );
      sb.Append( "\r\n" );
      sb.Append( "Content-Disposition: form-data; name=\"file\"; filename=\"" );
      sb.Append(fileName);
      sb.Append( "\"" );
      sb.Append( "\r\n" );
      sb.Append( "Content-Type: " );
      sb.Append( "application/octet-stream" );
      sb.Append( "\r\n" );
      sb.Append( "Content-Transfer-Encoding: binary" );
      sb.Append( "\r\n" );
      sb.Append( "\r\n" );

      string header = sb.ToString();
      byte[] headerBytes = Encoding.UTF8.GetBytes( header );

      var httpRequest =
        (HttpWebRequest)
        WebRequest.Create(
          new Uri(
            Backendless.URL + "/" + Backendless.VersionNum + "/files/" + EncodeURL( path ) + "/" + EncodeURL( fileName ),
            UriKind.Absolute ) );

      httpRequest.ContentType = "multipart/form-data; boundary=" + boundary;
      httpRequest.Method = "POST";
      httpRequest.Headers["KeepAlive"] = "true";

      foreach( var h in HeadersManager.GetInstance().Headers )
        httpRequest.Headers[h.Key] = h.Value;

      try
      {
        var async = new RequestStreamAsyncState<BackendlessFile>
          {
            Callback = callback,
            UploadCallback = uploadCallback,
            HttpRequest = httpRequest,
            HeaderBytes = headerBytes,
            BoundaryBytes = boundaryBytes,
            FileStream = fileStream
          };
        httpRequest.BeginGetRequestStream( RequestStreamCallback, async );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    private static void RequestStreamCallback( IAsyncResult asyncResult )
    {
      var asyncState = (RequestStreamAsyncState<BackendlessFile>) asyncResult.AsyncState;
      try
      {
        var fileStream = asyncState.FileStream;
        var httpRequest = asyncState.HttpRequest;
        var uploadCallback = asyncState.UploadCallback;
        using( var postStream = httpRequest.EndGetRequestStream( asyncResult ) )
        {
          var headerBytes = asyncState.HeaderBytes;
          byte[] boundaryBytes = asyncState.BoundaryBytes;
          long fileLength = fileStream.Length;
          long offset = 0;
          int bufferLen = (int) (fileLength < UPLOAD_BUFFER_DEFAULT_LENGTH ? fileLength : UPLOAD_BUFFER_DEFAULT_LENGTH);
          byte[] buffer = new byte[bufferLen];

          // send the headers
          postStream.Write( headerBytes, 0, headerBytes.Length );

          int progress = 0;
          int size = fileStream.Read( buffer, 0, bufferLen );
          while( size > 0 )
          {
            postStream.Write( buffer, 0, size );
            offset += size;

            if( !(uploadCallback is EmptyUploadCallback) )
            {
              int currentProgress = (int) ((offset/fileLength)*100);
              if( progress != currentProgress )
              {
                progress = currentProgress;
                uploadCallback.ProgressHandler.Invoke( progress );
              }
            }
            size = fileStream.Read( buffer, 0, bufferLen );
          }

          if( !(uploadCallback is EmptyUploadCallback) && progress != 100 )
            uploadCallback.ProgressHandler.Invoke( 100 );

          postStream.Write( boundaryBytes, 0, boundaryBytes.Length );
        }

        httpRequest.BeginGetResponse( ResponseCallback, asyncState );
      }
      catch( System.Exception ex )
      {
        if( asyncState.Callback != null )
          asyncState.Callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    private static void ResponseCallback( IAsyncResult asyncResult )
    {
      var asyncState = (RequestStreamAsyncState<BackendlessFile>) asyncResult.AsyncState;
      using( asyncState.FileStream )
      {
        try
        {
          using( var response = asyncState.HttpRequest.EndGetResponse( asyncResult ).GetResponseStream() )
          {
            var encode = System.Text.Encoding.GetEncoding( "utf-8" );
            var result = new StreamReader( response, encode ).ReadToEnd();
            var matchGroups = SERVER_RESULT_REGEXP.Match( result ).Groups;
            var fileUrl = matchGroups["fileUrl"].Value;

            asyncState.Callback.ResponseHandler.Invoke( new BackendlessFile( fileUrl ) );
          }
        }
        catch( WebException ex )
        {
          var response = new StreamReader( ex.Response.GetResponseStream() ).ReadToEnd();
          var matchGroups = SERVER_ERROR_REGEXP.Match( response ).Groups;
          var message = matchGroups["message"].Value;
          var code = matchGroups["code"].Value;
          var fault =
            new BackendlessFault( (string.IsNullOrEmpty( code ) ? "" : "Code: " + code + "\n") + "Message: " + message );

          if( asyncState.Callback != null )
            asyncState.Callback.ErrorHandler.Invoke( fault );
          else
            throw new BackendlessException( fault );
        }
      }
    }

#if SILVERLIGHT || WINDOWS_PHONE
      private String EncodeURL( string urlStr ) 
  {
    var splitedStr = urlStr.Split( '/' );
    var result = "";

    for( var i = 0; i < splitedStr.Length; i++ )
    {
      if( i != 0 )
        result += "/";

      result += System.Net.HttpUtility.UrlEncode(splitedStr[i]);
    }

    return result;
  }
#else
    private String EncodeURL( string urlStr )
    {
      var splitedStr = urlStr.Split( '/' );
      var result = "";

      for( var i = 0; i < splitedStr.Length; i++ )
      {
        if( i != 0 )
          result += "/";

        result += System.Web.HttpUtility.UrlEncode( splitedStr[i] );
      }

      return result;
    }
#endif
  }

  internal class EmptyUploadCallback : UploadCallback
  {
    public EmptyUploadCallback() : base( response =>
      {
        /*A stub. Needed for handy methods.*/
      } )
    {
    }
  }

  internal class RequestStreamAsyncState<T>
  {
    public UploadCallback UploadCallback;
    public HttpWebRequest HttpRequest;
    public FileStream FileStream;
    public byte[] BoundaryBytes;
    public byte[] HeaderBytes;
    public AsyncCallback<T> Callback;
  }
}