using System;
using System.Collections.Generic;
using BackendlessAPI;
using Weborb.Util;
using Weborb.Types;
using Weborb.Reader;

namespace BackendlessAPI.IO
{
  class BackendlessUserFactory : IArgumentObjectFactory
  {
    public object createObject( IAdaptingType argument )
    {
      if( argument is NamedObject )
        argument = ( (NamedObject) argument ).TypedObject;

      if( argument is NullType )
        return null;

      Dictionary<string, object> props = (Dictionary<string, object>) argument.adapt( typeof( Dictionary<string, object> ) );
      BackendlessUser backendlessUser = new BackendlessUser();
      backendlessUser.PutProperties( props );
      return backendlessUser;
    }
  }
}
