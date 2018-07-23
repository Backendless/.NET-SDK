using System;
using System.Collections.Generic;

namespace BackendlessAPI.RT
{
  public interface IRTRequest
  {
    String Id {
      get;
    }

    String Name {
      get;
    }

    Dictionary<String, Object> Options {
      get;
    }

    IRTCallback Callback {
      get;
    }

    Dictionary<String, Object> ToArgs();
  }
}
