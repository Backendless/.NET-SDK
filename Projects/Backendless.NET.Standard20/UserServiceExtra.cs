using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using System.Text;
using BackendlessAPI.Exception;
using BackendlessAPI.Engine;
using BackendlessAPI.Service;
using Weborb.Writer;
using Weborb.Client;

namespace BackendlessAPI
{
  internal class UserServiceExtra
  {
    public static UserServiceExtra Instance { get; } = new UserServiceExtra();

    internal void LoginWithOAuth1( String authProviderCode, String authToken, BackendlessUser guestUser,
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

    internal void LoginWithOAuth2( String authProviderCode, String accessToken, BackendlessUser guestUser,
                                   Dictionary<String, String> fieldsMappings, AsyncCallback<Dictionary<String, Object>> callback )
    {
      if( fieldsMappings == null )
        fieldsMappings = new Dictionary<String, String>();

      Invoker.InvokeAsync( UserService.USER_MANAGER_SERVER_ALIAS, "loginWithAuth2",
        new Object[] { authProviderCode, accessToken, fieldsMappings, guestUser == null ? null : guestUser.Properties },
        new AsyncCallback<Dictionary<String, Object>>(
          response =>
            callback?.ResponseHandler( response ),
          fault =>
            callback?.ErrorHandler( fault ) )/* new AdaptingResponder<>( BackendlessUser.class, new BackendlessUserAdaptingPolicy() ) */ );
    }
  }
}
