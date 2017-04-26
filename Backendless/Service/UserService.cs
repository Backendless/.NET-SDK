using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.Property;
using BackendlessAPI.Utils;
using Weborb.Types;

namespace BackendlessAPI.Service
{
  public class UserService
  {
    private static string USER_MANAGER_SERVER_ALIAS = "com.backendless.services.users.UserService";
    private BackendlessUser _currentUser;
    private ILoginStorage _loginStorage = null;

    public BackendlessUser CurrentUser
    {
      get { return _currentUser; }
      set { _currentUser = value; }
    }

    public ILoginStorage LoginStorage
    {
      get 
      {
        if( _loginStorage == null )
          _loginStorage = new LoginStorage();

        return _loginStorage; 
      }

      set { _loginStorage = value; }
    }

    static UserService()
    {
      Types.AddClientClassMapping( "com.backendless.services.users.property.AbstractProperty", typeof( AbstractProperty ) );
      Types.AddClientClassMapping( "com.backendless.services.users.property.UserProperty", typeof( UserProperty ) );
    }

    public BackendlessUser Register( BackendlessUser user )
    {
      CheckUserToBeProper( user, true );
      user.PutProperties( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "register",
                                                                        new Object[] { user.Properties } ) );

      return user;
    }

    public void Register( BackendlessUser user, AsyncCallback<BackendlessUser> callback )
    {
      try
      {
        CheckUserToBeProper( user, true );

        var responder = new AsyncCallback<Dictionary<string, object>>( r =>
            {
              user.PutProperties( r );
              if( callback != null )
                callback.ResponseHandler.Invoke( user );
            }, f =>
                {
                  if( callback != null )
                    callback.ErrorHandler.Invoke( f );
                  else
                    throw new BackendlessException( f );
                } );

        Invoker.InvokeAsync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "register",
                                                        new Object[] { user.Properties }, responder );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public BackendlessUser Update( BackendlessUser user )
    {
      CheckUserToBeProper( user, false );

      if( string.IsNullOrEmpty( user.ObjectId ) )
        throw new ArgumentNullException( ExceptionMessage.WRONG_USER_ID );

      user.PutProperties( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "update",
                                                                        new object[] { user.Properties } ) );

      return user;
    }

    public void Update( BackendlessUser user, AsyncCallback<BackendlessUser> callback )
    {
      try
      {
        CheckUserToBeProper( user, false );

        if( string.IsNullOrEmpty( user.ObjectId ) )
          throw new ArgumentNullException( ExceptionMessage.WRONG_USER_ID );

        var responder = new AsyncCallback<Dictionary<string, object>>( r =>
            {
              user.PutProperties( r );
              if( callback != null )
                callback.ResponseHandler.Invoke( user );
            }, f =>
                {
                  if( callback != null )
                    callback.ErrorHandler.Invoke( f );
                  else
                    throw new BackendlessException( f );
                } );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "update",
                            new object[] { user.Properties }, responder );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public String LoggedInUserObjectId()
    {
      if( !LoginStorage.HasData )
        return null;

      return LoginStorage.ObjectId;
    }

    public BackendlessUser Login( string login, string password )
    {
      return Login( login, password, false );
    }

    public BackendlessUser Login( string login, string password, bool stayLoggedIn )
    {
      if( CurrentUser != null )
        Logout();

      if( string.IsNullOrEmpty( login ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_LOGIN );

      if( string.IsNullOrEmpty( password ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PASSWORD );

      HandleUserLogin( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "login",
                                                                     new Object[] { login, password } ), stayLoggedIn );

      return CurrentUser;
    }

    public void Login( string login, string password, AsyncCallback<BackendlessUser> callback )
    {
      Login( login, password, callback, false );
    }

    public void Login( string login, string password, AsyncCallback<BackendlessUser> callback, bool stayLoggedIn )
    {
      try
      {
        if( string.IsNullOrEmpty( login ) )
          throw new ArgumentNullException( ExceptionMessage.NULL_LOGIN );

        if( string.IsNullOrEmpty( password ) )
          throw new ArgumentNullException( ExceptionMessage.NULL_PASSWORD );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "login",
                            new Object[] { login, password },
                            GetUserLoginAsyncHandler( callback, stayLoggedIn ) );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public bool IsValidLogin()
    {
      if( LoginStorage.HasData && LoginStorage.UserToken != null && LoginStorage.UserToken.Length > 0 )
          return Invoker.InvokeSync<Boolean>( USER_MANAGER_SERVER_ALIAS, "isValidUserToken",
                                     new object[] { LoginStorage.UserToken } );
      else
        return CurrentUser != null;
    }

    public void IsValidLogin( AsyncCallback<Boolean> callback )
    {
      if( LoginStorage.HasData && LoginStorage.UserToken != null && LoginStorage.UserToken.Length > 0 )
        Invoker.InvokeAsync<Boolean>( USER_MANAGER_SERVER_ALIAS, "isValidUserToken",
                                   new object[] { LoginStorage.UserToken }, callback );
      else
        callback.ResponseHandler( CurrentUser != null );
    }

    public void Logout()
    {
      try
      {
        Invoker.InvokeSync<object>( USER_MANAGER_SERVER_ALIAS, "logout",
                                   new object[] { } );
      }
      catch( BackendlessException exception )
      {
        BackendlessFault fault = exception.BackendlessFault;

        if( fault != null )
        {
          int faultCode = int.Parse( fault.FaultCode );

          if( faultCode != 3064 && faultCode != 3091 && faultCode != 3090 && faultCode != 3023 )
            throw exception;
        }
      }

      CurrentUser = null;
      HeadersManager.GetInstance().RemoveHeader( HeadersEnum.USER_TOKEN_KEY );
      LoginStorage.DeleteFiles();
    }

    public void Logout( AsyncCallback<object> callback )
    {
      var responder = new AsyncCallback<object>( r =>
          {
            CurrentUser = null;
            HeadersManager.GetInstance().RemoveHeader( HeadersEnum.USER_TOKEN_KEY );
            LoginStorage.DeleteFiles();

            if( callback != null )
              callback.ResponseHandler.Invoke( null );
          }, f =>
              {
                if( callback != null )
                  callback.ErrorHandler.Invoke( f );
                else
                  throw new BackendlessException( f );
              } );
      Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "logout",
                          new object[] {},
                          responder );
    }

    public void RestorePassword( string identity )
    {
      if( identity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_IDENTITY );

      Invoker.InvokeSync<object>( USER_MANAGER_SERVER_ALIAS, "restorePassword",
                                 new object[] { identity } );
    }

    public void RestorePassword( string identity, AsyncCallback<object> callback )
    {
      try
      {
        if( identity == null )
          throw new ArgumentNullException( ExceptionMessage.NULL_IDENTITY );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "restorePassword",
                            new object[] { identity }, callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public void AssignRole( string identity, string roleName )
    {
      if( string.IsNullOrEmpty( identity ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_IDENTITY );

      if( string.IsNullOrEmpty( roleName ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ROLE_NAME );

      Invoker.InvokeSync<object>( USER_MANAGER_SERVER_ALIAS, "assignRole",
                                 new object[] { identity, roleName } );
    }

    public void AssignRole( string identity, string roleName, AsyncCallback<object> callback )
    {
      try
      {
        if( identity == null )
          throw new ArgumentNullException( ExceptionMessage.NULL_IDENTITY );

        if( string.IsNullOrEmpty( roleName ) )
          throw new ArgumentNullException( ExceptionMessage.NULL_ROLE_NAME );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "assignRole",
                            new object[] { identity, roleName },
                            callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public void UnassignRole( string identity, string roleName )
    {
      if( string.IsNullOrEmpty( identity ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_IDENTITY );

      if( string.IsNullOrEmpty( roleName ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ROLE_NAME );

      Invoker.InvokeSync<object>( USER_MANAGER_SERVER_ALIAS, "unassignRole",
                                 new object[] { identity, roleName } );
    }

    public void UnassignRole( string identity, string roleName, AsyncCallback<object> callback )
    {
      try
      {
        if( identity == null )
          throw new ArgumentNullException( ExceptionMessage.NULL_IDENTITY );

        if( string.IsNullOrEmpty( roleName ) )
          throw new ArgumentNullException( ExceptionMessage.NULL_ROLE_NAME );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "unassignRole",
                            new object[] { identity, roleName },
                            callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public IList<string> GetUserRoles()
    {
      return Invoker.InvokeSync<List<string>>( USER_MANAGER_SERVER_ALIAS, "getUserRoles",
          new object[] {} );
    }

    public void GetUserRoles( AsyncCallback<IList<string>> callback )
    {
      try
      {
        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "getUserRoles",
            new object[] {},
            callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public List<UserProperty> DescribeUserClass()
    {
      List<UserProperty> result = Invoker.InvokeSync<List<UserProperty>>( USER_MANAGER_SERVER_ALIAS,
                                                                         "describeUserClass",
                                                                         new object[] {} );

      return result;
    }

    public void DescribeUserClass( AsyncCallback<List<UserProperty>> callback )
    {
      Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "describeUserClass",
                          new object[] {}, callback );
    }

    private static void CheckUserToBeProper( BackendlessUser user, bool passwordCheck )
    {
      if( user == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_USER );

      if( passwordCheck && string.IsNullOrEmpty( user.Password ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PASSWORD );
    }

    private void HandleUserLogin( Dictionary<string, object> invokeResult, bool stayLoggedIn )
    {
      HeadersManager.GetInstance()
                    .AddHeader( HeadersEnum.USER_TOKEN_KEY,
                               invokeResult[ HeadersEnum.USER_TOKEN_KEY.Header ].ToString() );

      if( CurrentUser == null )
        CurrentUser = new BackendlessUser();

      CurrentUser.PutProperties( invokeResult );

      if( stayLoggedIn )
      {
        LoginStorage.UserToken = invokeResult[ HeadersEnum.USER_TOKEN_KEY.Header ].ToString();
        LoginStorage.ObjectId = Backendless.UserService.CurrentUser.ObjectId;
        LoginStorage.SaveData();
      }
    }

    private AsyncCallback<Dictionary<string, object>> GetUserLoginAsyncHandler(
        AsyncCallback<BackendlessUser> callback, bool stayLoggedIn )
    {
      return new AsyncCallback<Dictionary<string, object>>( r =>
          {
            HandleUserLogin( r, stayLoggedIn );

            if( callback != null )
              callback.ResponseHandler.Invoke( CurrentUser );
          }, f =>
              {
                if( callback != null )
                  callback.ErrorHandler.Invoke( f );
                else
                  throw new BackendlessException( f );
              } );
    }

#if WINDOWS_PHONE
        public void LoginWithFacebook(Microsoft.Phone.Controls.WebBrowser webBrowser,
                                         IDictionary<string, string> facebookFieldsMappings, IList<string> permissions,
                                         AsyncCallback<BackendlessUser> callback)
        {
            try
            {
                if (webBrowser == null)
                    throw new ArgumentNullException(ExceptionMessage.NUL_WEBBROWSER);

                Invoker.InvokeAsync(USER_MANAGER_SERVER_ALIAS, "getFacebookServiceAuthorizationUrlLink",
                                    new Object[]
                                        {
                                            Backendless.AppId, Backendless.VersionNum,
                                            HeadersManager.GetInstance().Headers[HeadersEnum.APP_TYPE_NAME.Header],
                                            facebookFieldsMappings, permissions
                                        },
                                    GetSocialEasyLoginAsyncHandler(webBrowser, callback));
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex.Message));
                else
                    throw;
            }
        }

        public void LoginWithTwitter(Microsoft.Phone.Controls.WebBrowser webBrowser,
                                        IDictionary<string, string> twitterFieldsMappings, AsyncCallback<BackendlessUser> callback)
        {
            try
            {
                if (webBrowser == null)
                    throw new ArgumentNullException(ExceptionMessage.NUL_WEBBROWSER);

                Invoker.InvokeAsync(USER_MANAGER_SERVER_ALIAS, "getTwitterServiceAuthorizationUrlLink",
                                    new Object[]
                                        {
                                            Backendless.AppId, Backendless.VersionNum,
                                            HeadersManager.GetInstance().Headers[HeadersEnum.APP_TYPE_NAME.Header],
                                            twitterFieldsMappings
                                        },
                                    GetSocialEasyLoginAsyncHandler(webBrowser, callback));
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex.Message));
                else
                    throw;
            }
        }

        private AsyncCallback<string> GetSocialEasyLoginAsyncHandler(Microsoft.Phone.Controls.WebBrowser webBrowser,
                                                                     AsyncCallback<BackendlessUser> callback)
        {
            return new AsyncCallback<string>(
                response => Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var uri = new Uri(response, UriKind.Absolute);

                        webBrowser.IsScriptEnabled = true;
                        webBrowser.ScriptNotify += (sender, args) =>
                            {
                                var result = (new Utils.Json()).Deserialize(args.Value);
                                GetUserLoginAsyncHandler(callback, false).ResponseHandler.Invoke(result);
                            };
                        webBrowser.Navigate(uri);
                    }), fault =>
                        {
                            if (callback != null)
                                callback.ErrorHandler.Invoke(fault);
                        });
        }
#endif
  }
}