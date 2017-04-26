using System;
using System.Collections.Generic;
using BackendlessAPI.Engine;
using Weborb.Service;

namespace BackendlessAPI
{
  public class BackendlessUser
  {
    public const string PASSWORD_KEY = "password";
    public const string EMAIL_KEY = "email";
    private const string ID_KEY = "objectId";

    private Dictionary<string, object> _properties = new Dictionary<string, object>();

    public BackendlessUser()
    {
    }

    internal BackendlessUser( Dictionary<string, object> properties )
    {
      _properties = properties;
    }

    [SetClientClassMemberName( "properties" )]
    public Dictionary<string, object> Properties
    {
      get { return _properties; }
      set { _properties = value; }
    }

    public string Password
    {
      get { return Properties.ContainsKey( PASSWORD_KEY ) ? (string) Properties[PASSWORD_KEY] : null; }
      set
      {
        if( Properties.ContainsKey( PASSWORD_KEY ) )
          SetProperty( PASSWORD_KEY, value );
        else
          AddProperty( PASSWORD_KEY, value );
      }
    }

    public string Email
    {
      get { return Properties.ContainsKey( EMAIL_KEY ) ? (string) Properties[EMAIL_KEY] : null; }
      set
      {
        if( Properties.ContainsKey( EMAIL_KEY ) )
          SetProperty( EMAIL_KEY, value );
        else
          AddProperty( EMAIL_KEY, value );
      }
    }

    public string ObjectId
    {
      get { return Properties.ContainsKey( ID_KEY ) ? (string) Properties[ID_KEY] : null; }
      set
      {
        if( Properties.ContainsKey( ID_KEY ) )
          SetProperty( ID_KEY, value );
        else
          AddProperty( ID_KEY, value );
      }
    }

    public void PutProperties( Dictionary<string, object> newProps )
    {
      foreach( var keyValuePair in newProps )
      {
        if( keyValuePair.Key.Equals( HeadersEnum.USER_TOKEN_KEY.ToString() ) )
          continue;

        if( Properties.ContainsKey( keyValuePair.Key ) )
          SetProperty( keyValuePair.Key, keyValuePair.Value );
        else
          AddProperty( keyValuePair.Key, keyValuePair.Value );
      }
    }

    public void AddProperty( string key, object value )
    {
      Properties.Add( key, value );
    }

    public void SetProperty( string key, object value )
    {
      Properties[key] = value;
    }

    public object GetProperty( string key )
    {
      return Properties[key];
    }
  }
}