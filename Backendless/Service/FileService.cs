﻿using BackendlessAPI.Async;
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
#if !(NET_35 || NET_40)
using System.Threading.Tasks;
#endif

namespace BackendlessAPI.Service
{
  public class FileService
  {
    private static readonly Regex SERVER_ERROR_REGEXP =
      new Regex( "\"message\":\"(?<message>[^\"]*)\",\"code\":(?<code>[\\d]*)" ); //, RegexOptions.Compiled );

    private static readonly Regex
      SERVER_RESULT_REGEXP = new Regex( "\"fileURL\":\"(?<fileUrl>[^\"]*)" ); //, RegexOptions.Compiled );

    private const string FILE_MANAGER_SERVER_ALIAS = "com.backendless.services.file.FileService";
    private const int UPLOAD_BUFFER_DEFAULT_LENGTH = 8192;

    public FileService()
    {
    }

  #region UPLOAD

  #if !(NET_35 || NET_40)
    public async Task<BackendlessFile> UploadAsync( FileStream fileStream, string remotePath )
    {
      return await UploadAsync( fileStream, remotePath, false );
    }

    public async Task<BackendlessFile> UploadAsync( FileStream fileStream, string remotePath, bool overwrite )
    {
      return await UploadAsync( fileStream, remotePath, overwrite, new EmptyUploadCallback() );
    }

    public async Task<BackendlessFile> UploadAsync( FileStream fileStream, string remotePath,
                                                    UploadCallback uploadCallback )
    {
      return await UploadAsync( fileStream, remotePath, false, uploadCallback );
    }

    public async Task<BackendlessFile> UploadAsync( FileStream fileStream,
                                                    string remotePath,
                                                    bool overwrite,
                                                    UploadCallback uploadCallback )
    {
      if( string.IsNullOrEmpty( remotePath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      if( fileStream == null || string.IsNullOrEmpty( Path.GetFileName( fileStream.Name ) ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_FILE );

      var fileName = Path.GetFileName( fileStream.Name );

      //You cannot get name of IsolatedStorageFileStream the normal way, it always returns [Unknown] (making it pass the checks against null)
      if( fileStream.GetType() == typeof( System.IO.IsolatedStorage.IsolatedStorageFileStream ) )
        fileName = Path.GetFileName( ((System.IO.IsolatedStorage.IsolatedStorageFileStream) fileStream).Name );

      return await MakeFileUploadAsync( fileStream, remotePath, fileName, overwrite, uploadCallback );
    }
    
    public async Task<BackendlessFile> UploadAsync( byte[] bytes, string remotePath, string fileName )
    {
      return await UploadAsync( bytes, remotePath, fileName, false );
    }

    public async Task<BackendlessFile> UploadAsync( byte[] bytes, string remotePath, string fileName, bool overwrite )
    {
      return await UploadAsync( bytes, remotePath, fileName, overwrite, new EmptyUploadCallback() );
    }

    public async Task<BackendlessFile> UploadAsync( byte[] bytes, string remotePath, string fileName,
                                                    UploadCallback uploadCallback )
    {
      return await UploadAsync( bytes, remotePath, fileName, false, uploadCallback );
    }

    public async Task<BackendlessFile> UploadAsync( byte[] bytes,
                                                    string remotePath,
                                                    string fileName,
                                                    bool overwrite,
                                                    UploadCallback uploadCallback )
    {
      if( string.IsNullOrEmpty( remotePath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      if( bytes == null || bytes.Length == 0 )
        throw new ArgumentNullException( ExceptionMessage.NULL_FILE_CONTENTS );
      
      if( string.IsNullOrEmpty( fileName ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_NAME );


      MemoryStream memoryStream = new MemoryStream();
      {
        memoryStream.Write( bytes, 0, bytes.Length );
        memoryStream.Flush();
        memoryStream.Seek( 0, SeekOrigin.Begin );
        return await MakeFileUploadAsync( memoryStream, remotePath, fileName, overwrite, uploadCallback );
      }
    }

    public async Task<BackendlessFile> UploadAsync(String urlToFile, String backendlessPath, Boolean overwrite = false)
    {
      return await Task.Run(() => Upload(urlToFile, backendlessPath, overwrite)).ConfigureAwait(false);
    }
  #endif

    public void Upload( FileStream fileStream, string remotePath, AsyncCallback<BackendlessFile> callback )
    {
      Upload( fileStream, remotePath, false, callback );
    }

    public void Upload( FileStream fileStream, string remotePath, bool overwrite,
                        AsyncCallback<BackendlessFile> callback )
    {
      Upload( fileStream, remotePath, overwrite, new EmptyUploadCallback(), callback );
    }

    public void Upload( FileStream fileStream, string remotePath, UploadCallback uploadCallback,
                        AsyncCallback<BackendlessFile> callback )
    {
      Upload( fileStream, remotePath, false, uploadCallback, callback );
    }

    public void Upload( FileStream fileStream, string remotePath, bool overwrite, UploadCallback uploadCallback,
                        AsyncCallback<BackendlessFile> callback )
    {
      if( string.IsNullOrEmpty( remotePath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      if( fileStream == null || string.IsNullOrEmpty( Path.GetFileName( fileStream.Name ) ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_FILE );

      MakeFileUpload( fileStream, remotePath, overwrite, uploadCallback, callback );
    }

    public BackendlessFile Upload(String urlToFile, String backendlessPath, Boolean overwrite = false)
    {
      if (String.IsNullOrEmpty(urlToFile))
      {
        throw new ArgumentNullException(ExceptionMessage.NULL_URL_TO_FILE);
      }

      if (String.IsNullOrEmpty(backendlessPath))
      {
        throw new ArgumentNullException(ExceptionMessage.NULL_PATH);
      }

      return Invoker.InvokeSync<BackendlessFile>(FILE_MANAGER_SERVER_ALIAS, "upload", new Object[] { urlToFile, backendlessPath, overwrite });
    }

    public void Upload(String urlToFile, String backendlessPath, AsyncCallback<BackendlessFile> callback, Boolean overwrite = false)
    {
      if (String.IsNullOrEmpty(urlToFile))
      {
        throw new ArgumentNullException(ExceptionMessage.NULL_URL_TO_FILE);
      }

      if (String.IsNullOrEmpty(backendlessPath))
      {
        throw new ArgumentNullException(ExceptionMessage.NULL_PATH);
      }

      Invoker.InvokeAsync(FILE_MANAGER_SERVER_ALIAS, "upload", new Object[] { urlToFile, backendlessPath, overwrite }, callback);
    }
    #endregion

    #region REMOVE_FILE

    public Int32 Remove( String fileURL )
    {
      return this.RemoveDirectory( fileURL );
    }

#if !( NET_35 || NET_40 )
    public async Task<Int32> RemoveAsync( String fileURL )
    {
      return await Task.Run( () => Remove( fileURL ) ).ConfigureAwait( false );
    }
#endif

    public void Remove( String fileURL, AsyncCallback<Int32> callback )
    {
      this.RemoveDirectory( fileURL, callback );
    }

    #endregion

    #region REMOVE_DIR

    public Int32 RemoveDirectory( String directoryPath, String pattern, Boolean recursive )
    {
      if( String.IsNullOrEmpty( directoryPath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      return Invoker.InvokeSync<Int32>( FILE_MANAGER_SERVER_ALIAS,
                                  "deleteFileOrDirectory",
                                  new Object[] { directoryPath, pattern, recursive } );
    }

    public Int32 RemoveDirectory( String directoryPath )
    {
      return this.RemoveDirectory( directoryPath, "*", true );
    }

#if !( NET_35 || NET_40 )
    public async Task RemoveDirectoryAsync( String directoryPath )
    {
      await Task.Run( () => RemoveDirectory( directoryPath ) ).ConfigureAwait( false );
    }
#endif

    public void RemoveDirectory( String directoryPath, AsyncCallback<Int32> callback )
    {
      this.RemoveDirectory( directoryPath, "*", true, callback );
    }

    public void RemoveDirectory( String directoryPath, String pattern, Boolean recursive, AsyncCallback<Int32> callback )
    {
      if( String.IsNullOrEmpty( directoryPath ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PATH );

      Invoker.InvokeAsync<Int32>( FILE_MANAGER_SERVER_ALIAS,
                                   "deleteFileOrDirectory",
                                   new Object[] { directoryPath, pattern, recursive },
                                   callback );
    }

    #endregion

    #region SAVEFILE

    public string SaveFile( string filePathName, byte[] fileContent, bool overwrite = false )
    {
      string fileName = filePathName.Substring( filePathName.LastIndexOf( "/" ) );
      string path = filePathName.Substring( 0, filePathName.LastIndexOf( "/" ) );
      return SaveFile( path, fileName, fileContent, overwrite );
    }

    public string SaveFile( string path, string fileName, byte[] fileContent, bool overwrite = false )
    {
      return Invoker.InvokeSync<String>( FILE_MANAGER_SERVER_ALIAS, "saveFile",
                                         new object[] { path, fileName, fileContent, overwrite } );
    }

  #if !(NET_35 || NET_40)
    public async Task<string> SaveFileAsync( string filePathName, byte[] fileContent, bool overwrite = false )
    {
      return await Task.Run( () => SaveFile( filePathName, fileContent, overwrite ) ).ConfigureAwait( false );
    }

    public async Task<string> SaveFileAsync( string path, string fileName, byte[] fileContent, bool overwrite = false )
    {
      return await Task.Run( () => SaveFile( path, fileName, fileContent, overwrite ) ).ConfigureAwait( false );
    }
  #endif

    public void SaveFile( string filePathName, byte[] fileContent, AsyncCallback<string> responder )
    {
      SaveFile( filePathName, fileContent, false, responder );
    }

    public void SaveFile( string filePathName, byte[] fileContent, bool overwrite, AsyncCallback<string> responder )
    {
      string fileName = filePathName.Substring( filePathName.LastIndexOf( "/" ) );
      string path = filePathName.Substring( 0, filePathName.LastIndexOf( "/" ) );
      SaveFile( path, fileName, fileContent, overwrite, responder );
    }

    public void SaveFile( string path, string fileName, byte[] fileContent, AsyncCallback<string> responder )
    {
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS,
                           "saveFile",
                           new object[] { path, fileName, fileContent },
                           responder );
    }

    public void SaveFile( string path,
                          string fileName,
                          byte[] fileContent,
                          bool overwrite,
                          AsyncCallback<string> responder )
    {
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS,
                           "saveFile",
                           new object[] { path, fileName, fileContent, overwrite },
                           responder );
    }

  #endregion

  #region RENAME

    public string RenameFile( string oldPathName, string newName )
    {
      checkPaths( oldPathName, newName );
      return Invoker.InvokeSync<string>( FILE_MANAGER_SERVER_ALIAS, "renameFile",
                                         new object[] { oldPathName, newName } );
    }

  #if !(NET_35 || NET_40)
    public async Task<string> RenameFileAsync( string oldPathName, string newName )
    {
      return await Task.Run( () => RenameFile( oldPathName, newName ) ).ConfigureAwait( false );
    }
  #endif

    public void RenameFile( string oldPathName, string newName, AsyncCallback<string> responder )
    {
      checkPaths( oldPathName, newName );
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "renameFile", new Object[] { oldPathName, newName }, responder );
    }

  #endregion

  #region COPY

    public string CopyFile( string sourcePathName, string targetPath )
    {
      checkPaths( sourcePathName, targetPath );
      return Invoker.InvokeSync<string>( FILE_MANAGER_SERVER_ALIAS, "copyFile",
                                         new object[] { sourcePathName, targetPath } );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<string> CopyFileAsync( string sourcePathName, string targetPath )
    {
      return await Task.Run( () => CopyFile( sourcePathName, targetPath ) ).ConfigureAwait( false );
    }
  #endif

    public void CopyFile( string sourcePathName, string targetPath, AsyncCallback<string> responder )
    {
      checkPaths( sourcePathName, targetPath );
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "copyFile", new object[] { sourcePathName, targetPath },
                           responder );
    }

  #endregion

  #region MOVE

    public string MoveFile( string sourcePathName, string targetPath )
    {
      checkPaths( sourcePathName, targetPath );
      return Invoker.InvokeSync<string>( FILE_MANAGER_SERVER_ALIAS, "moveFile",
                                         new object[] { sourcePathName, targetPath } );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<string> MoveFileAsync( string sourcePathName, string targetPath )
    {
      return await Task.Run( () => MoveFile( sourcePathName, targetPath ) ).ConfigureAwait( false );
    }
  #endif

    public void MoveFile( string sourcePathName, string targetPath, AsyncCallback<string> responder )
    {
      checkPaths( sourcePathName, targetPath );
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "moveFile", new object[] { sourcePathName, targetPath },
                           responder );
    }

  #endregion

  #region LISTING

    public IList<File.FileInfo> Listing( string path, string pattern = "*", bool recursive = false )
    {
      return Listing( path, pattern, recursive, BackendlessSimpleQuery.DEFAULT_PAGE_SIZE,
                      BackendlessSimpleQuery.DEFAULT_OFFSET );
    }

    public IList<File.FileInfo> Listing( string path, String pattern, bool recursive, int pagesize, int offset )
    {
      return Invoker.InvokeSync<IList<File.FileInfo>>( FILE_MANAGER_SERVER_ALIAS, "listing",
                                                  new Object[] { path, pattern, recursive, pagesize, offset } );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<IList<File.FileInfo>> ListingAsync( string path, string pattern = "*", bool recursive = false )
    {
      return await ListingAsync( path, pattern, recursive, BackendlessSimpleQuery.DEFAULT_PAGE_SIZE,
                                 BackendlessSimpleQuery.DEFAULT_OFFSET );
    }
    
    public async Task<IList<File.FileInfo>> ListingAsync( string path, String pattern, bool recursive, int pagesize, int offset )
    {
      return await Task.Run( () => Listing( path, pattern, recursive, pagesize, offset ) ).ConfigureAwait( false );
    }
  #endif

    public void Listing( string path, AsyncCallback<IList<File.FileInfo>> responder )
    {
      Listing( path, "*", false, responder );
    }

    public void Listing( string path, string pattern, bool recursive, AsyncCallback<IList<File.FileInfo>> responder )
    {
      Listing( path, pattern, recursive, BackendlessSimpleQuery.DEFAULT_PAGE_SIZE,
               BackendlessSimpleQuery.DEFAULT_OFFSET, responder );
    }

    public void Listing( string path, string pattern, bool recursive, int pagesize, int offset,
                         AsyncCallback<IList<File.FileInfo>> responder )
    {
      AsyncCallback<IList<File.FileInfo>> listingCallback = new AsyncCallback<IList<File.FileInfo>>(
                                                                                          files =>
                                                                                          {
                                                                                            responder?.ResponseHandler( files );
                                                                                          },
                                                                                          error =>
                                                                                          {
                                                                                            responder?.ErrorHandler( error );
                                                                                          }
                                                                                         );
      object[] args = { path, pattern, recursive, pagesize, offset };
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "listing", args, listingCallback );
    }

  #endregion

  #region GET FILE COUNT

    public int GetFileCount( string path, string pattern, bool recursive, bool countDirectories )
    {
      return Invoker.InvokeSync<int>( FILE_MANAGER_SERVER_ALIAS, "count",
                                      new Object[] { path, pattern, recursive, countDirectories } );
    }

    public int GetFileCount( string path, string pattern = "*", bool recursive = false )
    {
      return GetFileCount( path, pattern, recursive, false );
    }

  #if !(NET_35 || NET_40)
    public async Task<int> GetFileCountAsync( string path, string pattern = "*", bool recursive = false )
    {
      return await GetFileCountAsync( path, pattern, recursive, false );
    }
    
    public async Task<int> GetFileCountAsync( string path, string pattern, bool recursive, bool countDirectories )
    {
      return await Task.Run( () => GetFileCount( path, pattern, recursive, countDirectories ) ).ConfigureAwait( false );
    }
    #endif

    public void GetFileCount( string path, string pattern, bool recursive, bool countDirectories,
                              AsyncCallback<int> responder )
    {
      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "count",
                                new object[] { path, pattern, recursive, countDirectories }, responder );
    }

    public void GetFileCount( string path, string pattern, bool recursive, AsyncCallback<int> responder )
    {
      GetFileCount( path, pattern, recursive, false, responder );
    }

    public void GetFileCount( string path, string pattern, AsyncCallback<int> responder )
    {
      GetFileCount( path, pattern, false, responder );
    }

    public void GetFileCount( string path, AsyncCallback<int> responder )
    {
      GetFileCount( path, "*", responder );
    }

    #endregion

    #region APPEND

    public String Append(String filePath, String fileSourceURL)
    {
      if (String.IsNullOrEmpty(filePath))
      {
        throw new ArgumentException(ExceptionMessage.NULL_PATH);
      }

      object[] parameters = new object[] { filePath, fileSourceURL };

      return AppendExecutor(parameters);
    }

    public void Append(String filePath, String fileSourceURL, AsyncCallback<String> responder)
    {
      if (String.IsNullOrEmpty(filePath))
      {
        throw new ArgumentException(ExceptionMessage.NULL_PATH);
      }

      object[] parameters = new object[] { filePath, fileSourceURL };

      AppendExecutorWithCallback(parameters, responder);
    }

    public async Task<String> AppendAsync(String filePath, String fileSourceURL)
    {
      return await Task.Run(() => Append(filePath, fileSourceURL)).ConfigureAwait(false);
    }
    
    public String Append(String directoryPath, String fileName, String fileSourceURL)
    {
      checkPaths(directoryPath, fileName);


      object[] parameters = new object[] { directoryPath, fileName, fileSourceURL };

      return AppendExecutor(parameters);
    }

    public void Append(String directoryPath, String fileName, String fileSourceURL, AsyncCallback<String> responder)
    {
      checkPaths(directoryPath, fileName);


      object[] parameters = new object[] { directoryPath, fileName, fileSourceURL };

      AppendExecutorWithCallback(parameters, responder);
    }


    public async Task<String> AppendAsync(String directoryPath, String fileName, String fileSourceURL)
    {
      return await Task.Run(() => Append(directoryPath, fileName, fileSourceURL)).ConfigureAwait(false);
    }

    public String Append(String directoryPath, String fileName, byte[] bytes)
    {
      checkPaths(directoryPath, fileName);

      object[] parameters = new object[] { directoryPath, fileName, bytes};

      return AppendExecutor(parameters);
    }

    public void Append(String directoryPath, String fileName, byte[] bytes, AsyncCallback<String> responder)
    {
      checkPaths(directoryPath, fileName);

      object[] parameters = new object[] { directoryPath, fileName, bytes };

      AppendExecutorWithCallback(parameters, responder);
    }

    public async Task<String> AppendAsync(String directoryPath, String fileName, byte[] bytes)
    {
      return await Task.Run(() => Append(directoryPath, fileName, bytes)).ConfigureAwait(false);
    }

    public String Append(String filePath, byte[] bytes)
    {
      if (String.IsNullOrEmpty(filePath))
      {
        throw new ArgumentException(ExceptionMessage.NULL_PATH);
      }

      object[] parameters = new object[] { filePath, bytes };

      return AppendExecutor(parameters);
    }

    public void Append(String filePath, byte[] bytes, AsyncCallback<String> responder)
    {
      if (String.IsNullOrEmpty(filePath))
      {
        throw new ArgumentException(ExceptionMessage.NULL_PATH);
      }

      object[] parameters = new object[] { filePath, bytes };

      AppendExecutorWithCallback(parameters, responder);
    }

    public async Task<String> AppendAsync(String filePath, byte[] bytes)
    {
      return await Task.Run(() => Append(filePath, bytes)).ConfigureAwait(false);
    }

    public String AppendText(String directoryPath, String fileName, String data)
    {
      checkPaths(directoryPath, fileName);

      object[] parameters = new object[] { directoryPath, fileName, data };

      return AppendExecutor(parameters);
    }

    public void AppendText(String directoryPath, String fileName, String data, AsyncCallback<String> responder)
    {
      checkPaths(directoryPath, fileName);

      object[] parameters = new object[] { directoryPath, fileName, data };

      AppendExecutorWithCallback(parameters, responder);
    }

    public async Task<String> AppendTextAsync(String directoryPath, String fileName, String data)
    {
      return await Task.Run(() => AppendText(directoryPath, fileName, data)).ConfigureAwait(false);
    }

    public String AppendText(String filePath, String data)
    {
      if (String.IsNullOrEmpty(filePath))
      {
        throw new ArgumentException(ExceptionMessage.NULL_PATH);
      }

      object[] parameters = new object[] { filePath, data };

      return AppendExecutor(parameters);
    }

    public void AppendText(String filePath, String data, AsyncCallback<String> responder)
    {
      if (String.IsNullOrEmpty(filePath))
      {
        throw new ArgumentException(ExceptionMessage.NULL_PATH);
      }

      object[] parameters = new object[] { filePath, data };

      AppendExecutorWithCallback(parameters, responder);
    }

    public async Task<String> AppendTextAsync(String filePath, String data)
    {
      return await Task.Run(() => AppendText(filePath, data)).ConfigureAwait(false);
    }

    private String AppendExecutor(object[] parametersList)
    {
      return Invoker.InvokeSync<String>(FILE_MANAGER_SERVER_ALIAS, "append", parametersList);
    }

    private void AppendExecutorWithCallback(object[] paramtersList, AsyncCallback<String> callback)
    {
      Invoker.InvokeAsync<String>(FILE_MANAGER_SERVER_ALIAS, "append", paramtersList, callback);
    }

    #endregion

    #region EXISTS

    public bool Exists( string path )
    {
      if( string.IsNullOrEmpty( path ) )
        throw new System.Exception( ExceptionMessage.NULL_PATH );

      return Invoker.InvokeSync<bool>( FILE_MANAGER_SERVER_ALIAS, "exists", new object[] { path } );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<bool> ExistsAsync( string path )
    {
      return await Task.Run( () => Exists( path ) ).ConfigureAwait( false );
    }
    #endif

    public void Exists( string path, AsyncCallback<bool> responder )
    {
      if( string.IsNullOrEmpty( path ) )
        throw new System.Exception( ExceptionMessage.NULL_PATH );

      Invoker.InvokeAsync( FILE_MANAGER_SERVER_ALIAS, "exists", new object[] { path }, responder );
    }

  #endregion

    private void checkPaths( string path1, string path2 )
    {
      if( string.IsNullOrEmpty( path1 ) )
        throw new System.Exception( ExceptionMessage.NULL_PATH );

      if( string.IsNullOrEmpty( path2 ) )
        throw new System.Exception( ExceptionMessage.NULL_NAME );
    }

  #if !(NET_35 || NET_40)
    private async Task<BackendlessFile> MakeFileUploadAsync( Stream fileStream, string path, string fileName, bool overwrite,
                                                             UploadCallback uploadCallback )
    {
      string boundary = DateTime.Now.Ticks.ToString( "x" );
      byte[] boundaryBytes = Encoding.UTF8.GetBytes( "\r\n--" + boundary + "--\r\n" );
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

      var header = sb.ToString();
      byte[] headerBytes = Encoding.UTF8.GetBytes( header );

      var urlStr = Backendless.InitAppData.FULL_QUERY_URL + "/files/" +
                   EncodeURL( path ) + "/" + EncodeURL( fileName );
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

      Stream stream = await httpRequest.GetRequestStreamAsync();
      return await RequestStreamCallbackAsync( stream, fileStream, httpRequest, uploadCallback, headerBytes,
                                               boundaryBytes );
    }

    private async Task<BackendlessFile> RequestStreamCallbackAsync( Stream postStream,
                                                                    Stream fileStream,
                                                                    WebRequest httpRequest,
                                                                    UploadCallback uploadCallback,
                                                                    byte[] headerBytes,
                                                                    byte[] boundaryBytes )
    {
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
          int currentProgress = (int) ((offset / fileLength) * 100);
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
      WebResponse webResponse = await httpRequest.GetResponseAsync();

      using( fileStream )
      {
        try
        {
          var encode = Encoding.GetEncoding( "utf-8" );
          var result = new StreamReader( webResponse.GetResponseStream(), encode ).ReadToEnd();
          var matchGroups = SERVER_RESULT_REGEXP.Match( result ).Groups;
          var fileUrl = matchGroups[ "fileUrl" ].Value;

          return new BackendlessFile( fileUrl );
        }
        catch( WebException ex )
        {
          var response = new StreamReader( ex.Response.GetResponseStream() ).ReadToEnd();
          var matchGroups = SERVER_ERROR_REGEXP.Match( response ).Groups;
          var message = matchGroups[ "message" ].Value;
          var code = matchGroups[ "code" ].Value;
          var fault =
            new BackendlessFault( (string.IsNullOrEmpty( code ) ? "" : "Code: " + code + "\n") + "Message: " +
                                  message );

          throw new BackendlessException( fault );
        }
      }
    }
  #endif

    private void MakeFileUpload( FileStream fileStream, string path, bool overwrite,
                                 UploadCallback uploadCallback,
                                 AsyncCallback<BackendlessFile> callback )
    {
      string boundary = DateTime.Now.Ticks.ToString( "x" );
      byte[] boundaryBytes = Encoding.UTF8.GetBytes( "\r\n--" + boundary + "--\r\n" );

      var fileName = Path.GetFileName( fileStream.Name );

      //You cannot get name of IsolatedStorageFileStream the normal way, it always returns [Unknown] (making it pass the checks against null)
      if( fileStream.GetType() == typeof( System.IO.IsolatedStorage.IsolatedStorageFileStream ) )
        fileName = Path.GetFileName( ((System.IO.IsolatedStorage.IsolatedStorageFileStream) fileStream).Name );

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

      var header = sb.ToString();
      byte[] headerBytes = Encoding.UTF8.GetBytes( header );

      var urlStr = Backendless.InitAppData.FULL_QUERY_URL + "/files/" +
                   EncodeURL( path ) + "/" + EncodeURL( fileName );
      if( overwrite )
        urlStr = urlStr + "?overwrite=true";

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
              int currentProgress = (int) ((offset / fileLength) * 100);
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
            var encode = Encoding.GetEncoding( "utf-8" );
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
            new BackendlessFault( (string.IsNullOrEmpty( code ) ? "" : "Code: " + code + "\n") + "Message: " +
                                  message );

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

        result += Uri.EscapeDataString( splitedStr[ i ] );
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