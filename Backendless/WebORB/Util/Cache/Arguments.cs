using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Weborb.Util.Cache
  {
  class Arguments
    {
    public object[] arguments;

    public Arguments()
      {
      
      }
    public Arguments( object[] arguments )
      {
      this.arguments = arguments;
      }

    public override bool Equals(object _obj)
      {
      Arguments obj = _obj as Arguments;

      if ( obj == null )
        return false;

      if ( arguments.Length != obj.arguments.Length )
        return false;

      for ( int i = 0; i < arguments.Length; i++ )
        if ( !arguments[ i ].Equals( obj.arguments[ i ] ) )
          return false;

      return true;
      }

    public override int GetHashCode()
      {
      if ( arguments.Length == 0 )
        return 0;

      return arguments[ 0 ].GetHashCode();
      }
    }
  }
