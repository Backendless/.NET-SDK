using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Util.Cache
  {
  class CachedValue
    {
    private object value;
    private DateTime expirationTime;

    public CachedValue( object value, DateTime dateTime )
      {
      this.value = value;
      this.expirationTime = dateTime;
      }

    public object Value
      {
      get { return value; }
      set { this.value = value; }
      }

    public DateTime ExpirationTime
      {
      get { return expirationTime; }
      set { expirationTime = value; }
      }
    }
  }
