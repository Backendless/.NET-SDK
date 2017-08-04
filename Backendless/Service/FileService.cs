using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.File;
using BackendlessAPI.Data;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BackendlessAPI.Service
{
  public class FileService
  {
    private static readonly Regex SERVER_ERROR_REGEXP =
      new Regex( "\"message\":\"(?<message>[^\"]*)\",\"code\":(?<code>[\\d]*)" );//, RegexOptions.Compiled );

    private static readonly Regex SERVER_RESULT_REGEXP = new Regex( "\"fileURL\":\"(?<fileUrl>[^\"]*)" );//, RegexOptions.Compiled );

    private const string FILE_MANAGER_SERVER_ALIAS = "com.backendless.services.file.FileService";
    private const int UPLOAD_BUFFER_DEFAULT_LENGTH = 8192;

    public FileService()
    {
    }

	#region UPLOAD
	public void Upload(FileStream fileStream, string remotePath, AsyncCallback<BackendlessFile> callback)
	{
	  Upload(fileStream, remotePath, false, callback);
	}
	
    public void Upload( FileStream fileStream, string remotePath, bool overwrite, AsyncCallback<BackendlessFile> callback )
    {
      Upload( fileStream, remotePath, overwrite, new EmptyUploadCallback(), callback );
    }

	public void Upload(FileStream fileStream, string remotePath, UploadCallback uploadCallback, AsyncCallback<BackendlessFile> callback)
	{
	  Upload( fileStream, remotePath, false, uploadCallback, callback );
	}

    public void Upload( FileStream fileStream, string remotePath, bool overwrite, UploadCallback uploadCallback, AsyncCallback<BackendlessFile> callback )
    {
      if( string.IsNullOrEmpty( remotePath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      if( fileStream == null || string.IsNullOrEmpty( Path.GetFileName( fileStream.Name ) ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_FILE );

      MakeFileUpload( fileStream, remotePath, overwrite, uploadCallback, callback );
    }
    #endregion
    #region REMOVE_FILE
    public void Remove( string fileUrl )
    {
      if( string.IsNullOrEmpty( fileUrl ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeSync<object>( FILE_MANAGER_SERVER_ALIAS, "deleteFileOrDirectory",
                                  new Object[] { fileUrl } );
    }

    public void Remove( string fileUrl, AsyncCallback<object> callback )
    {
      if( string.IsNullOrEmpty( fileUrl ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeAsync<object>( FILE_MANAGER_SERVER_ALIAS, "deleteFileOrDirectory",
                                   new Object[] { fileUrl }, callback );
    }
    #endregion
    #region REMOVE_DIR
    public void RemoveDirectory( string directoryPath )
    {
      if( string.IsNullOrEmpty( directoryPath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeSync<object>( FILE_MANAGER_SERVER_ALIAS, "deleteFileOrDirectory",
                                  new Object[] { directoryPath } );
    }

    public void RemoveDirectory( string directoryPath, AsyncCallback<object> callback )
    {
      if( string.IsNullOrEmpty( directoryPath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeAsync<object>( FILE_MANAGER_SERVER_ALIAS, "deleteFileOrDirectory",
                                   new Object[] { directoryPath }, callback );
    }
    #endregion
    #region SAVEFILE

    public String SaveFile( String filePathName, byte[] fileContent )
    {
      return SaveFile( filePathName, fileContent, false );
    }

    public String SaveFile( String filePathName, byte[] fileContent, Boolean overwrite )
    {
      String fileName = filePathName.Substring( filePathName.LastIndexOf( "/" ) );
      String path = filePathName.Substring( 0, filePathName.LastIndexOf( "/" ) );
      return SaveFile( path, fileName, fileContent, overwrite );
    }

    public String SaveFile( String path, String fileName, byte[] fileContent )
    {
      return SaveFile( path, fileName, fileContent, false );
    }

    public String SaveFile( String path, String fileName, byte[] fileContent, Boolean overwrite )
    {
      return Invoker.InvokeSync<String>( FILE_MANAGER_SERVER_ALIAS, "saveFile", new Object[] { path, fileName, fileContent, overwrite } );
    }

    public void SaveFile( String filePathName, byte[] fileContent, AsyncCallback<String> responder )
    {
      SaveFile( filePathName, fileContent, false, responder );
    }

    public void SaveFile( String filePathName, byte[] fileContent, Boolean overwrite, AsyncCallback<String> responder )
    {
      String fileName = filePathName.Substring( filePathName.LastIndexOf( "/" ) );
      String path = filePathName.Substring( 0, filePathName.LastIndexOf( "/" ) );
      SaveFile( path, fileName, fileContent, overwrite, responder );
    }

    public void SaveFile( String path, String fileName, byte[] fileContent, AsyncCallback<String> responder )
    {
      Invoker.InvokeAsync<String>( FILE_MANAGER_SERVER_ALIAS, "saveFile", new Object[] { path, fileName, fileContent }, responder );
    }

    public void SaveFile( String path, String fileName, byte[] fileContent, Boolean overwrite, AsyncCallback<String> responder )
    {
      Invoker.InvokeAsync<String>( FILE_MANAGER_SERVER_ALIAS, "saveFile", new Object[] { path, fileName, fileContent, overwrite }, responder );
    }

    #endregion
    #region RENAME
    public string RenameFile( String oldPathName, String newName )
    {
      checkPaths( oldPathName, newName );
      return Invoker.InvokeSync<string>( FILE_MANAGER_SERVER_ALIAS, "renameFile", new Object[] { oldPathName, newName } );
    }

    public void RenameFile( String oldPathName, String newName, AsyncCallback<string> responder )
    {
      checkPaths( oldPathName, newName );
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "renameFile", new Object[] { oldPathName, newName }, responder );
    }
    #endregion 
    #region COPY
    public string CopyFile( String sourcePathName, String targetPath )
    {
      checkPaths( sourcePathName, targetPath );
      return Invoker.InvokeSync<string>( FILE_MANAGER_SERVER_ALIAS, "copyFile", new Object[] { sourcePathName, targetPath } );
    }

    public void CopyFile( String sourcePathName, String targetPath, AsyncCallback<string> responder )
    {
      checkPaths( sourcePathName, targetPath );
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "copyFile", new Object[] { sourcePathName, targetPath }, responder );
    }
    #endregion
    #region MOVE
    public string MoveFile( String sourcePathName, String targetPath )
    {
      checkPaths( sourcePathName, targetPath );
      return Invoker.InvokeSync<string>( FILE_MANAGER_SERVER_ALIAS, "moveFile", new Object[] { sourcePathName, targetPath } );
    }

    public void MoveFile( String sourcePathName, String targetPath, AsyncCallback<String> responder )
    {
      checkPaths( sourcePathName, targetPath );
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "moveFile", new Object[] { sourcePathName, targetPath }, responder );
    }
    #endregion
    #region LISTING
    public IList<FileInfo> Listing( String path )
    {
      return Listing( path, "*", false );
    }

    public IList<FileInfo> Listing( String path, String pattern, bool recursive )
    {
      return Listing( path, pattern, recursive, BackendlessSimpleQuery.DEFAULT_PAGE_SIZE, BackendlessSimpleQuery.DEFAULT_OFFSET );
    }

    public IList<FileInfo> Listing( String path, String pattern, bool recursive, int pagesize, int offset )
    {
      return Invoker.InvokeSync<IList<FileInfo>>( FILE_MANAGER_SERVER_ALIAS, "listing", new Object[] { path, pattern, recursive, pagesize, offset } );
    }

    public void Listing( String path, AsyncCallback<IList<FileInfo>> responder )
    {
      Listing( path, "*", false, responder );
    }

    public void Listing( String path, String pattern, bool recursive, AsyncCallback<IList<FileInfo>> responder )
    {
      Listing( path, pattern, recursive, BackendlessSimpleQuery.DEFAULT_PAGE_SIZE, BackendlessSimpleQuery.DEFAULT_OFFSET, responder );
    }

    public void Listing( String path, String pattern, bool recursive, int pagesize, int offset, AsyncCallback<IList<FileInfo>> responder )
    {
      AsyncCallback<IList<FileInfo>> listingCallback = new AsyncCallback<IList<FileInfo>>(
        files =>
        {
          if( responder != null )
            responder.ResponseHandler( files );
        },
        error =>
        {
          if( responder != null )
            responder.ErrorHandler( error );
        }
      );
      Object[] args = new Object[] { path, pattern, recursive, pagesize, offset };
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "listing", args, listingCallback );
    }
    #endregion
    #region GET FILE COUNT
    public int GetFileCount( String path, String pattern, Boolean recursive, Boolean countDirectories )
    {
      return Invoker.InvokeSync<int>( FILE_MANAGER_SERVER_ALIAS, "count", new Object[] { path, pattern, recursive, countDirectories } );
    }

    public int GetFileCount( String path, String pattern, Boolean recursive )
    {
      return GetFileCount( path, pattern, recursive, false );
    }

    public int GetFileCount( String path, String pattern )
    {
      return GetFileCount( path, pattern, false );
    }

    public int GetFileCount( String path )
    {
      return GetFileCount( path, "*" );
    }

    public void GetFileCount( String path, String pattern, Boolean recursive, Boolean countDirectories, AsyncCallback<int> responder )
    {
        Invoker.InvokeAsync<int>( FILE_MANAGER_SERVER_ALIAS, "count", new Object[] { path, pattern, recursive, countDirectories }, responder );
    }

    public void GetFileCount( String path, String pattern, Boolean recursive, AsyncCallback<int> responder )
    {
      GetFileCount( path, pattern, recursive, false, responder );
    }

    public void GetFileCount( String path, String pattern, AsyncCallback<int> responder )
    {
      GetFileCount( path, pattern, false, responder );
    }

    public void GetFileCount( String path, AsyncCallback<int> responder )
    {
      GetFileCount( path, "*", responder );
    }
    #endregion
    #region EXISTS
    public Boolean Exists( String path )
    {
      if( string.IsNullOrEmpty( path ) )
        throw new System.Exception( ExceptionMessage.NULL_PATH );

      return Invoker.InvokeSync<Boolean>( FILE_MANAGER_SERVER_ALIAS, "exists", new Object[] { path } );
    }

    public void Exists( String path, AsyncCallback<Boolean> responder )
    {
      if( string.IsNullOrEmpty( path ) )
        throw new System.Exception( ExceptionMessage.NULL_PATH );

      Invoker.InvokeAsync<Boolean>( FILE_MANAGER_SERVER_ALIAS, "exists", new Object[] { path }, responder );
    }
    #endregion
    private void checkPaths( string path1, string path2 )
    {
      if( path1 == null || path1.Length == 0 )
        throw new System.Exception( ExceptionMessage.NULL_PATH );

      if( path2 == null || path2.Length == 0 )
        throw new System.Exception( ExceptionMessage.NULL_NAME );
    }

    private void MakeFileUpload( FileStream fileStream, string path, bool overwrite,
								 UploadCallback uploadCallback, 
                                 AsyncCallback<BackendlessFile> callback )
    {
      string boundary = DateTime.Now.Ticks.ToString( "x" );
      byte[] boundaryBytes = Encoding.UTF8.GetBytes( "\r\n--" + boundary + "--\r\n" );

      var fileName = Path.GetFileName( fileStream.Name );

      //You cannot get name of IsolatedStorageFileStream the normal way, it always returns [Unknown] (making it pass the checks against null)
      if( fileStream.GetType() == typeof( System.IO.IsolatedStorage.IsolatedStorageFileStream ) )
        fileName = Path.GetFileName( ( (System.IO.IsolatedStorage.IsolatedStorageFileStream) fileStream ).Name );

      var sb = new StringBuilder();
      sb.Append( "--" );
      sb.Append( boundary );
      sb.Append( "\r\n" );
      sb.Append( "Content-Disposition: form-data; name=\"file\"; filename=\"" );
      sb.Append( fileName );
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

      string urlStr = Backendless.URL + "/" + Backendless.AppId + "/" + Backendless.APIKey + "/files/" + EncodeURL( path ) + "/" + EncodeURL( fileName );
      if( overwrite )
        urlStr = urlStr + "?" + "overwrite" + "=" + overwrite;  

      var httpRequest =
        (HttpWebRequest)
        WebRequest.Create(
          new Uri(
            urlStr,
            UriKind.Absolute ) );

      httpRequest.ContentType = "multipart/form-data; boundary=" + boundary;
      httpRequest.Method = "POST";
      httpRequest.Headers[ "KeepAlive" ] = "true";

      foreach( var h in HeadersManager.GetInstance().Headers )
        httpRequest.Headers[ h.Key ] = h.Value;

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
          int bufferLen = (int) ( fileLength < UPLOAD_BUFFER_DEFAULT_LENGTH ? fileLength : UPLOAD_BUFFER_DEFAULT_LENGTH );
          byte[] buffer = new byte[ bufferLen ];

          // send the headers
          postStream.Write( headerBytes, 0, headerBytes.Length );

          int progress = 0;
          int size = fileStream.Read( buffer, 0, bufferLen );
          while( size > 0 )
          {
            postStream.Write( buffer, 0, size );
            offset += size;

            if( !( uploadCallback is EmptyUploadCallback ) )
            {
              int currentProgress = (int) ( ( offset / fileLength ) * 100 );
              if( progress != currentProgress )
              {
                progress = currentProgress;
                uploadCallback.ProgressHandler.Invoke( progress );
              }
            }
            size = fileStream.Read( buffer, 0, bufferLen );
          }

          if( !( uploadCallback is EmptyUploadCallback ) && progress != 100 )
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
            var fileUrl = matchGroups[ "fileUrl" ].Value;

            asyncState.Callback.ResponseHandler.Invoke( new BackendlessFile( fileUrl ) );
          }
        }
        catch( WebException ex )
        {
          var response = new StreamReader( ex.Response.GetResponseStream() ).ReadToEnd();
          var matchGroups = SERVER_ERROR_REGEXP.Match( response ).Groups;
          var message = matchGroups[ "message" ].Value;
          var code = matchGroups[ "code" ].Value;
          var fault =
            new BackendlessFault( ( string.IsNullOrEmpty( code ) ? "" : "Code: " + code + "\n" ) + "Message: " + message );

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

        result += Uri.EscapeDataString( splitedStr[i] );
      }

      return result;
    }
#endif
  }

  internal class EmptyUploadCallback : UploadCallback
  {
    public EmptyUploadCallback()
      : base( response =>
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