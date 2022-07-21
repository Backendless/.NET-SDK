using System;
using System.Collections.Generic;
using System.Security.Principal;
#if !(NET_35 || NET_40)
using System.Threading.Tasks;
#endif
using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.Property;
using BackendlessAPI.Utils;
using Weborb.Types;
using BackendlessAPI.Persistence;

namespace BackendlessAPI.Service
{
  public class UserService
  {
    private static string USER_MANAGER_SERVER_ALIAS = "com.backendless.services.users.UserService";
    private ILoginStorage _loginStorage = null;

    private BackendlessUser _currentUser = null;

    public BackendlessUser CurrentUser
    {
      get
      {
        if( _currentUser == null )
        {
          if(LoginStorage.HasData)
          {
            try
            {
              CurrentUser = Backendless.Data.Of<BackendlessUser>().FindById( LoginStorage.ObjectId );
            }
            catch
            {
              Console.WriteLine( "User token was expired. Logout..." );
              Backendless.UserService.Logout();
            }
          }
        }

        return _currentUser;
      }

      set => _currentUser = value;
    }

    public ILoginStorage LoginStorage
    {
      get
      {
        if( _loginStorage == null )
          _loginStorage = new LoginStorage();

        return _loginStorage;
      }

      set => _loginStorage = value;
    }

    static UserService()
    {
      Types.AddClientClassMapping( "com.backendless.services.users.property.AbstractProperty",
                                   typeof( AbstractProperty ) );
      Types.AddClientClassMapping( "com.backendless.services.users.property.UserProperty", typeof( UserProperty ) );
    }

    public BackendlessUser Register( BackendlessUser user )
    {
      CheckUserToBeProper( user, true );
      user.PutProperties( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "register",
                                                                          new Object[] { user.Properties } ) );

      return user;
    }
  #if !(NET_35 || NET_40)
    public async Task<BackendlessUser> RegisterAsync( BackendlessUser user )
    {
      return await Task.Run( () => Register( user ) ).ConfigureAwait( false );
    }
  #endif

    public void Register( BackendlessUser user, AsyncCallback<BackendlessUser> callback )
    {
      try
      {
        CheckUserToBeProper( user, true );

        var responder = new AsyncCallback<Dictionary<string, object>>( r =>
        {
          user.PutProperties( r );
          callback?.ResponseHandler( user );
        }, f =>
        {
          if( callback != null )
            callback.ErrorHandler.Invoke( f );
          else
            throw new BackendlessException( f );
        } );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "register",
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

    public BackendlessUser Update( BackendlessUser user )
    {
      CheckUserToBeProper( user, false );

      if( String.IsNullOrEmpty( user.ObjectId ) )
        throw new ArgumentNullException( ExceptionMessage.WRONG_USER_ID );

      user.PutProperties( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "update",
                                                                          new object[] { user.Properties } ) );

      return user;
    }

  #if !(NET_35 || NET_40)
    public async Task<BackendlessUser> UpdateAsync( BackendlessUser user )
    {
      return await Task.Run( () => Update( user ) ).ConfigureAwait( false );
    }
  #endif

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
          callback?.ResponseHandler( user );
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

    public string LoggedInUserObjectId()
    {
      if( !LoginStorage.HasData )
        return null;

      return LoginStorage.ObjectId;
    }

    public BackendlessUser Login( string login, string password, bool stayLoggedIn = false )
    {
      if( CurrentUser != null )
        Logout();

      if( string.IsNullOrEmpty( login ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_LOGIN );

      if( string.IsNullOrEmpty( password ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_PASSWORD );

      HandleUserLogin( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS, "login",
                                                                       new object[] { login, password } ),
                       stayLoggedIn );

      return CurrentUser;
    }

  #if !(NET_35 || NET_40)
    public async Task<BackendlessUser> LoginAsync( string login, string password, bool stayLoggedIn = false )
    {
      return await Task.Run( () => Login( login, password, stayLoggedIn ) ).ConfigureAwait( false );
    }
  #endif

    public void Login( string login, string password, AsyncCallback<BackendlessUser> callback,
                       bool stayLoggedIn = false )
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
      if( LoginStorage.HasData && !string.IsNullOrEmpty( LoginStorage.UserToken ) )
        return Invoker.InvokeSync<Boolean>( USER_MANAGER_SERVER_ALIAS, "isValidUserToken",
                                            new object[] { LoginStorage.UserToken } );
      else
        return CurrentUser != null;
    }

  #if !(NET_35 || NET_40)
    public async Task<bool> IsValidLoginAsync()
    {
      return await Task.Run( () => IsValidLogin() ).ConfigureAwait( false );
    }
  #endif

    public void IsValidLogin( AsyncCallback<bool> callback )
    {
      if( LoginStorage.HasData && !string.IsNullOrEmpty( LoginStorage.UserToken ) )
        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "isValidUserToken",
                                      new object[] { LoginStorage.UserToken }, callback );
      else
        callback.ResponseHandler( CurrentUser != null );
    }

    public void Logout()
    {
      try
      {
        Invoker.InvokeSync<object>( USER_MANAGER_SERVER_ALIAS, "logout",
                                    new object[] {} );
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

  #if !(NET_35 || NET_40)
    public async Task LogoutAsync()
    {
      await Task.Run( () => Logout() ).ConfigureAwait( false );
    }
  #endif

    public void Logout( AsyncCallback<object> callback )
    {
      var responder = new AsyncCallback<object>( r =>
      {
        CurrentUser = null;
        HeadersManager.GetInstance().RemoveHeader( HeadersEnum.USER_TOKEN_KEY );
        LoginStorage.DeleteFiles();

        callback?.ResponseHandler( null );
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

  #if !(NET_35 || NET_40)
    public async Task RestorePasswordAsync( string identity )
    {
      await Task.Run( () => RestorePassword( identity ) ).ConfigureAwait( false );
    }
  #endif

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

#if !( NET_35 || NET_40 )
    public async Task ResendEmailConfirmationAsync( String identity )
    {
      await Task.Run( () => ResendEmailConfirmation( identity ) ).ConfigureAwait( false );
    }
#endif

    public void ResendEmailConfirmation( String identity )
    {
      if( identity == null )
        throw new ArgumentException( ExceptionMessage.NULL_EMAIL );

      Invoker.InvokeSync<Object>( USER_MANAGER_SERVER_ALIAS, "resendEmailConfirmation", new Object[] { identity } );
    }

    public void ResendEmailConfirmation( String identity, AsyncCallback<Object> callback )
    {
      try
      {
        if( identity == null )
          throw new ArgumentException( ExceptionMessage.NULL_EMAIL );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "resendEmailConfirmation", new Object[] { identity }, callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );

        else
          throw;
      }
    }

    public void GetAuthorizationUrlLink( String authProviderCode, Dictionary<String, String> fieldsMappings, List<String> scope,
                                     AsyncCallback<String> callback )
    {
      if( fieldsMappings == null )
        fieldsMappings = new Dictionary<String, String>();

      if( scope == null )
        scope = new List<String>();

      Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "getAuthorizationUrlLink", new Object[] { authProviderCode, null, null,
                                                                                fieldsMappings, scope }, callback );
    }

    public IList<String> GetAuthorizationUrlLink( String authProviderCode, Dictionary<String, String> fieldsMappings, List<String> scope )
    {
      if( fieldsMappings == null )
        fieldsMappings = new Dictionary<String, String>();

      if( scope == null )
        scope = new List<String>();

      return Invoker.InvokeSync<IList<String>>( USER_MANAGER_SERVER_ALIAS, "getAuthorizationUrlLink", new Object[] { authProviderCode, null, null,
                                                                                fieldsMappings, scope } );
    }

#if !( NET_35 || NET_40 )
    public async Task<IList<String>> GetAuthorizationUrlLinkAsync( String authProviderCode, Dictionary<String, String> fieldsMappings, List<String> scope )
    {
      return await Task.Run( () => GetAuthorizationUrlLink( authProviderCode, fieldsMappings, scope ) ).ConfigureAwait( false ); 
    }
#endif

    public void LoginWithOAuth2( String authProviderCode, String accessToken, Dictionary<String, String> fieldsMappings,
      AsyncCallback<BackendlessUser> callback, Boolean stayLoggedIn = false )
    {
      AsyncCallback<Dictionary<String, Object>> internalResponder = GetUserLoginAsyncHandler( callback, stayLoggedIn );
      LoginWithOAuth2( authProviderCode, accessToken, null, fieldsMappings, internalResponder );
    }

    public void LoginWithOAuth2( String authProviderCode, String accessToken, BackendlessUser guestUser, Dictionary<String, String> fieldsMappings,
      AsyncCallback<BackendlessUser> callback, Boolean stayLoggedIn = false )
    {
      AsyncCallback<Dictionary<String, Object>> internalResponser = GetUserLoginAsyncHandler( callback, stayLoggedIn );
      LoginWithOAuth2( authProviderCode, accessToken, guestUser, fieldsMappings, internalResponser );
    }

#if !( NET_35 || NET_40 )
    public async Task LoginWithOAuth2Async( String authProviderCode, String accessToken, Dictionary<String, String> fieldsMappings,
      AsyncCallback<BackendlessUser> callback, Boolean stayLoggedIn = false )
    {
      await Task.Run( () => LoginWithOAuth2( authProviderCode, accessToken, fieldsMappings, callback,
                                                                          stayLoggedIn ) ).ConfigureAwait( false );
    }

    public async Task LoginWithOAuth2Async( String authProviderCode, String accessToken, BackendlessUser guestUser, Dictionary<String, String> fieldsMappings,
      AsyncCallback<BackendlessUser> callback, Boolean stayLoggedIn = false )
    {
      await Task.Run( () => LoginWithOAuth2( authProviderCode, accessToken, guestUser, fieldsMappings, callback, stayLoggedIn ) ).ConfigureAwait( false );
    }
#endif

    public void LoginWithOAuth1( String authProviderCode, String authToken, String authTokenSecret,
      Dictionary<String, String> fieldsMappings, AsyncCallback<BackendlessUser> callback, Boolean stayLoggedIn = false )
    {
      AsyncCallback<Dictionary<String, Object>> internalResponder = GetUserLoginAsyncHandler( callback, stayLoggedIn );
      LoginWithOAuth1( authProviderCode, authToken, null, authTokenSecret, fieldsMappings, internalResponder );
    }

    public void LoginWithOAuth1( String authProviderCode, String authToken, String authTokenSecret, BackendlessUser guestUser,
      Dictionary<String, String> fieldsMappings, AsyncCallback<BackendlessUser> callback, Boolean stayLoggedIn )
    {
      AsyncCallback<Dictionary<String, Object>> internalResponder = GetUserLoginAsyncHandler( callback, stayLoggedIn );
      LoginWithOAuth1( authProviderCode, authToken, guestUser, authTokenSecret, fieldsMappings, internalResponder );
    }

#if !( NET_35 || NET_40 )
    public async Task LoginWithOAuth1Async( String authProviderCode, String authToken, String authTokenSecret,
      Dictionary<String, String> fieldsMappings, AsyncCallback<BackendlessUser> callback, Boolean stayLoggedIn = false )
    {
      await Task.Run( () => LoginWithOAuth1( authProviderCode, authToken, authTokenSecret, fieldsMappings, callback, stayLoggedIn ) )
                                         .ConfigureAwait( false );
    }

    public async Task LoginWithOAuth1Async( String authProviderCode, String authToken, String authTokenSecret, BackendlessUser guestUser,
      Dictionary<String, String> fieldsMappings, AsyncCallback<BackendlessUser> callback, Boolean stayLoggedIn )
    {
      await Task.Run( () => LoginWithOAuth1( authProviderCode, authToken, authTokenSecret, guestUser, fieldsMappings,
                                                              callback, stayLoggedIn ) ).ConfigureAwait( false );
    }
#endif

    private void LoginWithOAuth1( String authProviderCode, String authToken, BackendlessUser guestUser,
      String authTokenSecret, Dictionary<String, String> fieldsMappings, AsyncCallback<Dictionary<String, Object>> callback )
    {
      if( !authProviderCode.Equals( "twitter" ) )
        throw new ArgumentException( $"OAuth1 provider '{authProviderCode}' is not supported" );

      if( fieldsMappings == null )
        fieldsMappings = new Dictionary<String, String>();

      Invoker.InvokeAsync( UserService.USER_MANAGER_SERVER_ALIAS, "loginWithTwitter",
                          new Object[] { authToken, authTokenSecret, fieldsMappings, guestUser == null ? null : guestUser.Properties },
                          new AsyncCallback<Dictionary<String, Object>>(
                          response =>
                            callback?.ResponseHandler( response ),
                          fault =>
                            callback?.ErrorHandler( fault ) ) );
    }

    private void LoginWithOAuth2( String authProviderCode, String accessToken, BackendlessUser guestUser,
                               Dictionary<String, String> fieldsMappings, AsyncCallback<Dictionary<String, Object>> callback )
    {
      if( fieldsMappings == null )
        fieldsMappings = new Dictionary<String, String>();

      Invoker.InvokeAsync( UserService.USER_MANAGER_SERVER_ALIAS, "loginWithOAuth2",
        new Object[] { authProviderCode, accessToken, fieldsMappings, guestUser == null ? null : guestUser.Properties },
        new AsyncCallback<Dictionary<String, Object>>(
          response =>
            callback?.ResponseHandler( response ),
          fault =>
            callback?.ErrorHandler( fault ) )/*, new AdaptingResponder<>( BackendlessUser.class, new BackendlessUserAdaptingPolicy() ) */ );
    }

#if !( NET_35 || NET_40 )
    public async Task<String> CreateEmailConfirmationURLAsync( String identity )
    {
      return await Task.Run( () => CreateEmailConfirmationURL( identity ) ).ConfigureAwait( false );
    }
#endif

    public String CreateEmailConfirmationURL( String identity )
    {
      if( String.IsNullOrEmpty( identity ) )
        throw new ArgumentException( ExceptionMessage.NULL_OR_EMPTY_INDENTITY );

      return Invoker.InvokeSync<String>( USER_MANAGER_SERVER_ALIAS, "createEmailConfirmationURL", new Object[] { identity } );
    }

    public void CreateEmailConfirmationURL( String identity, AsyncCallback<String> callback )
    {
      try
      {
        if( String.IsNullOrEmpty( identity ) )
          throw new ArgumentException( ExceptionMessage.NULL_OR_EMPTY_INDENTITY );

        Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "createEmailConfirmationURL", new Object[] { identity }, callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
        else
          throw;
      }
    }

#if !( NET_35 || NET_40 )
    public async Task AssignRoleAsync( string identity, string roleName )
    {
      await Task.Run( () => AssignRole( identity, roleName ) ).ConfigureAwait( false );
    }
  #endif

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

  #if !(NET_35 || NET_40)
    public async Task UnassignRoleAsync( string identity, string roleName )
    {
      await Task.Run( () => UnassignRole( identity, roleName ) ).ConfigureAwait( false );
    }
  #endif

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

    public void ReloadCurrentUser()
    {

      if( CurrentUser == null || String.IsNullOrEmpty( CurrentUser.ObjectId ) )
        throw new ArgumentNullException( ExceptionMessage.WRONG_USER_ID );

      var actualUser = Backendless.Data.Of( "Users" ).FindById( CurrentUser.ObjectId );

      CurrentUser.PutProperties( actualUser );
    }

#if !(NET_35 || NET_40)
    public async Task ReloadCurrentUserAsync()
    {
      await Task.Run( () => ReloadCurrentUser() ).ConfigureAwait( false );
    }
#endif

    public IList<BackendlessUser> FindByRole( String roleName, Boolean loadRoles, DataQueryBuilder queryBuilder)
    {
      if( String.IsNullOrEmpty( roleName ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ROLE_NAME );

      if( queryBuilder == null )
        queryBuilder = DataQueryBuilder.Create();

      return Invoker.InvokeSync<IList<BackendlessUser>>( USER_MANAGER_SERVER_ALIAS, "findByRole", new Object[] { roleName,
                                                                                      loadRoles, queryBuilder.Build() } );
    }

#if !( NET_35 || NET_40 )
    public async Task<IList<BackendlessUser>> FindByRoleAsync( String roleName, Boolean loadRoles, DataQueryBuilder queryBuilder )
    {
      return await Task.Run( () => FindByRole( roleName, loadRoles, queryBuilder ) );
    }
#endif

    public void FindByRole( String roleName, Boolean loadRoles, DataQueryBuilder queryBuilder, AsyncCallback<IList<BackendlessUser>> callback )
    {
      if( String.IsNullOrEmpty( roleName ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ROLE_NAME );

      if( queryBuilder == null )
        queryBuilder = DataQueryBuilder.Create();

      Invoker.InvokeAsync<IList<BackendlessUser>>( USER_MANAGER_SERVER_ALIAS, "findByRole",
                                                   new Object[] { roleName, loadRoles, queryBuilder.Build() }, callback );
    }

    public IList<string> GetUserRoles()
    {
      return Invoker.InvokeSync<List<string>>( USER_MANAGER_SERVER_ALIAS, "getUserRoles",
                                               new object[] {} );
    }

  #if !(NET_35 || NET_40)
    public async Task<IList<string>> GetUserRolesAsync()
    {
      return await Task.Run( () => GetUserRoles() ).ConfigureAwait( false );
    }
  #endif

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

  #if !(NET_35 || NET_40)
    public async Task<List<UserProperty>> DescribeUserClassAsync()
    {
      return await Task.Run( () => DescribeUserClass() ).ConfigureAwait( false );
    }
  #endif

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

  #if !(NET_35 || NET_40)
    public async Task<BackendlessUser> LoginAsGuestAsync( bool stayLoggedIn = false )
    {
      return await Task.Run( () => LoginAsGuest( stayLoggedIn ) ).ConfigureAwait( false );
    }
  #endif

    public BackendlessUser LoginAsGuest()
    {
      return LoginAsGuest( false );
    }

    public BackendlessUser LoginAsGuest( bool stayLoggedIn )
    {
      HandleUserLogin( Invoker.InvokeSync<Dictionary<string, object>>( USER_MANAGER_SERVER_ALIAS,
                                                "loginAsGuest", new object[] { } ), stayLoggedIn );
      return CurrentUser;
    }

    public void LoginAsGuest( AsyncCallback<BackendlessUser> responder )
    {
      LoginAsGuest( responder, false );
    }

    public void LoginAsGuest( AsyncCallback<BackendlessUser> responder, bool stayLoggedIn )
    { 
      if(CurrentUser != null)
        Logout();
      else
        try
        {
          Invoker.InvokeAsync( USER_MANAGER_SERVER_ALIAS, "loginAsGuest", new Object[] { },
                                   GetUserLoginAsyncHandler( responder, stayLoggedIn ) );
        }
        catch ( System.Exception ex )
        {
          if( responder != null )
            responder.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
          else
            throw;
        }
    }

    private AsyncCallback<Dictionary<string, object>> GetUserLoginAsyncHandler(
                     AsyncCallback<BackendlessUser> callback, bool stayLoggedIn )
    {
      return new AsyncCallback<Dictionary<string, object>>( r =>
      {
        HandleUserLogin( r, stayLoggedIn );

        callback?.ResponseHandler( CurrentUser );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );
    }

  #if WINDOWS_PHONE
        public void LoginWithFacebook( Microsoft.Phone.Controls.WebBrowser webBrowser,
                                         IDictionary<string, string> facebookFieldsMappings, IList<string> permissions,
                                         AsyncCallback<BackendlessUser> callback )
        {
            try
            {
                if ( webBrowser == null )
                    throw new ArgumentNullException( ExceptionMessage.NUL_WEBBROWSER );

                Invoker.InvokeAsync(USER_MANAGER_SERVER_ALIAS, "getFacebookServiceAuthorizationUrlLink",
                                    new Object[]
                                        {
                                            Backendless.AppId, Backendless.VersionNum,
                                            HeadersManager.GetInstance().Headers[HeadersEnum.APP_TYPE_NAME.Header],
                                            facebookFieldsMappings, permissions
                                        },
                                    GetSocialEasyLoginAsyncHandler( webBrowser, callback ) );
            }
            catch ( System.Exception ex )
            {
                if ( callback != null )
                    callback.ErrorHandler.Invoke( new BackendlessFault( ex.Message ) );
                else
                    throw;
            }
        }

        public void LoginWithTwitter( Microsoft.Phone.Controls.WebBrowser webBrowser,
                                        IDictionary<string, string> twitterFieldsMappings, AsyncCallback<BackendlessUser> callback )
        {
            try
            {
                if ( webBrowser == null )
                    throw new ArgumentNullException(ExceptionMessage.NUL_WEBBROWSER);

                Invoker.InvokeAsync(USER_MANAGER_SERVER_ALIAS, "getTwitterServiceAuthorizationUrlLink",
                                    new Object[]
                                        {
                                            Backendless.AppId, Backendless.VersionNum,
                                            HeadersManager.GetInstance().Headers[HeadersEnum.APP_TYPE_NAME.Header],
                                            twitterFieldsMappings
                                        },
                                    GetSocialEasyLoginAsyncHandler( webBrowser, callback ) );
            }
            catch ( System.Exception ex )
            {
                if ( callback != null )
                    callback.ErrorHandler.Invoke(new BackendlessFault( ex.Message ) );
                else
                    throw;
            }
        }

        private AsyncCallback<string> GetSocialEasyLoginAsyncHandler( Microsoft.Phone.Controls.WebBrowser webBrowser,
                                                                     AsyncCallback<BackendlessUser> callback )
        {
            return new AsyncCallback<string>(
                response => Deployment.Current.Dispatcher.BeginInvoke( () =>
                    {
                        var uri = new Uri( response, UriKind.Absolute );

                        webBrowser.IsScriptEnabled = true;
                        webBrowser.ScriptNotify += ( sender, args ) =>
                            {
                                var result = ( new Utils.Json() ).Deserialize( args.Value );
                                GetUserLoginAsyncHandler( callback, false ).ResponseHandler.Invoke( result );
                            };
                        webBrowser.Navigate(uri);
                    }), fault =>
                        {
                            if ( callback != null )
                                callback.ErrorHandler.Invoke( fault );
                        } );
        }
#endif
  }
}