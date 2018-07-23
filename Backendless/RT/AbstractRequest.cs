using System;
using System.Collections.Generic;

namespace BackendlessAPI.RT
{
  public abstract class AbstractRequest : IRTRequest
  {
    protected AbstractRequest( IRTCallback callback )
    {
      this.Id = Guid.NewGuid().ToString();
      this.Callback = callback;
      this.Options = new Dictionary<string, object>();
    }

    public string Id
    {
      get;
      set;
    }

    public virtual string Name
    {
      get;
      set;
    }

    public Dictionary<String, Object> Options
    {
      get;
      set;
    }

    public IRTCallback Callback
    {
      get;
      set;
    }

    internal void PutOption( String key, Object value )
    {
      Options[ key ] = value;
    }

    public Object GetOption( String key )
    {
      return Options[ key ];
    }

    public Dictionary<string, object> ToArgs()
    {
      Dictionary<String, Object> args = new Dictionary<string, object>();

      args[ "id" ] = Id; ;
      args[ "name" ] = Name;
      args[ "options" ] = Options;

      return args;
    }
  }
}
