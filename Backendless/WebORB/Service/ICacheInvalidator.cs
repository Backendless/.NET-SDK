using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Weborb.Service
  {
  public delegate void CacheInvalidator( object target, MethodInfo method, object[] args );

  public interface ICacheInvalidator
    {
    event CacheInvalidator InvalidateCache;
    }
  }
