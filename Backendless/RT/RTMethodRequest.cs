using System;
namespace BackendlessAPI.RT
{
  public class RTMethodRequest : AbstractRequest
  {
    public RTMethodRequest( MethodTypes methodType, IRTCallback callback ) : base( callback ) 
    {
      MethodType = methodType;
    }

    public override String Name {
      get {
        return Enum.GetName( typeof( MethodTypes ), MethodType );
      }
    }

    public MethodTypes MethodType {
      get;
      protected set;
    }
  }
}
