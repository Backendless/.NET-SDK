using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Service
  {
  public enum CacheScope { Global, Session, Instance }

  [AttributeUsage(AttributeTargets.Method)]
  public class WebORBCacheAttribute : Attribute
    {
    private long expirationTimespan = -1;

    // Global - cache is shared for all session and instances
    // Session - each session has its own cache
    // Instance - each instance has its own cache
    private CacheScope cacheScope = CacheScope.Global;

    public long ExpirationTimespan
      {
      get { return expirationTimespan; }
      set { expirationTimespan = value; }
      }

    public CacheScope CacheScope
      {
      get { return cacheScope; }
      set { cacheScope = value; }
      }
    }
  }
