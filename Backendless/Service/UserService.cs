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
using Weborb.Types;

namespace BackendlessAPI.Service
{
  public class UserService
  {
    private static string USER_MANAGER_SERVER_ALIAS = "com.backendless.services.users.UserService";
    private BackendlessUser _currentUser;

    public BackendlessUser CurrentUser
    {
      get { return _currentUser; }
      set { _currentUser = value; }
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
                                                                        new Object[]
                                                                                  {
                                                                                      Backendless.AppId,
                                                                                      Backendless.VersionNum,
                                                                                      user.Properties
                                                                                  } ) );

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
                                                        new Object[]
                                                                    {
                                                                        Backendless.AppId, Backendless.VersionNum,
                                                                        user.Properties
                                                                    },
                                                        responder );
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

      if( string.IsNullOrEmpty( user.UserId ) )
        throw new ArgumentNullException( ExceptionMessage.WRONG_USER_ID );

      user.PutProperties( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "update",
                                                                        new object[]
                                                                                  {
                                                                                      Backendless.AppId,
                                                                                      Backendless.VersionNum,
                                                                                      user.Properties
                                                                                  } ) );

      return user;
    }

    public void Update( BackendlessUser user, AsyncCallback<BackendlessUser> callback )
    {
      try
      {
        CheckUserToBeProper( user, false );

        if( string.IsNullOrEmpty( user.UserId ) )
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
                            new object[] { Backendless.AppId, Backendless.VersionNum, user.Properties }, responder );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public BackendlessUser Login( string login, string password )
    {
      if( CurrentUser != null )
        Logout();

      if( string.IsNullOrEmpty( login ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_LOGIN );

      if( string.IsNullOrEmpty( password ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PASSWORD );

      HandleUserLogin( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "login",
                                                                     new Object[]
                                                                               {
                                                                                   Backendless.AppId,
                                                                                   Backendless.VersionNum,
                                                                                   login, password
                                                                               } ) );

      return CurrentUser;
    }

    public void Login( string login, string password, AsyncCallback<BackendlessUser> callback )
    {
      try
      {
        if( string.IsNullOrEmpty( login ) )
          throw new ArgumentNullException( ExceptionMessage.NULL_LOGIN );

        if( string.IsNullOrEmpty( password ) )
          throw new ArgumentNullException( ExceptionMessage.NULL_PASSWORD );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "login",
                            new Object[] { Backendless.AppId, Backendless.VersionNum, login, password },
                            GetUserLoginAsyncHandler( callback ) );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

    public void Logout()
    {
      Invoker.InvokeSync<object>( USER_MANAGER_SERVER_ALIAS, "logout",
                                 new object[] { Backendless.AppId, Backendless.VersionNum } );

      CurrentUser = null;
      HeadersManager.GetInstance().RemoveHeader( HeadersEnum.USER_TOKEN_KEY );
    }

    public void Logout( AsyncCallback<object> callback )
    {
      var responder = new AsyncCallback<object>( r =>
          {
            CurrentUser = null;
            HeadersManager.GetInstance().RemoveHeader( HeadersEnum.USER_TOKEN_KEY );

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
                          new object[] { Backendless.AppId, Backendless.VersionNum },
                          responder );
    }

    public void RestorePassword( string identity )
    {
      if( identity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_IDENTITY );

      Invoker.InvokeSync<object>( USER_MANAGER_SERVER_ALIAS, "restorePassword",
                                 new object[] { Backendless.AppId, Backendless.VersionNum, identity } );
    }

    public void RestorePassword( string identity, AsyncCallback<object> callback )
    {
      try
      {
        if( identity == null )
          throw new ArgumentNullException( ExceptionMessage.NULL_IDENTITY );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "restorePassword",
                            new object[] { Backendless.AppId, Backendless.VersionNum, identity }, callback );
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
                                 new object[] { Backendless.AppId, Backendless.VersionNum, identity, roleName } );
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
                            new object[] { Backendless.AppId, Backendless.VersionNum, identity, roleName },
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
                                 new object[] { Backendless.AppId, Backendless.VersionNum, identity, roleName } );
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
                            new object[] { Backendless.AppId, Backendless.VersionNum, identity, roleName },
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
      return Invoker.InvokeSync<IList<string>>( USER_MANAGER_SERVER_ALIAS, "getUserRoles",
          new object[] { Backendless.AppId, Backendless.VersionNum } );
    }

    public void GetUserRoles( AsyncCallback<IList<string>> callback )
    {
      try
      {
        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "getUserRoles",
            new object[] { Backendless.AppId, Backendless.VersionNum },
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
                                                                         new object[]
                                                                                   {
                                                                                       Backendless.AppId,
                                                                                       Backendless.VersionNum
                                                                                   } );

      return result;
    }

    public void DescribeUserClass( AsyncCallback<List<UserProperty>> callback )
    {
      Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "describeUserClass",
                          new object[] { Backendless.AppId, Backendless.VersionNum }, callback );
    }

    private static void CheckUserToBeProper( BackendlessUser user, bool passwordCheck )
    {
      if( user == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_USER );

      if( passwordCheck && string.IsNullOrEmpty( user.Password ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PASSWORD );
    }

    private void HandleUserLogin( Dictionary<string, object> invokeResult )
    {
      HeadersManager.GetInstance()
                    .AddHeader( HeadersEnum.USER_TOKEN_KEY,
                               invokeResult[ HeadersEnum.USER_TOKEN_KEY.Header ].ToString() );

      if( CurrentUser == null )
        CurrentUser = new BackendlessUser();

      CurrentUser.PutProperties( invokeResult );
    }

    private AsyncCallback<Dictionary<string, object>> GetUserLoginAsyncHandler(
        AsyncCallback<BackendlessUser> callback )
    {
      return new AsyncCallback<Dictionary<string, object>>( r =>
          {
            HeadersManager.GetInstance()
                          .AddHeader( HeadersEnum.USER_TOKEN_KEY, r[ HeadersEnum.USER_TOKEN_KEY.Header ].ToString() );

            if( CurrentUser == null )
              CurrentUser = new BackendlessUser();

            CurrentUser.PutProperties( r );

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
                                GetUserLoginAsyncHandler(callback).ResponseHandler.Invoke(result);
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