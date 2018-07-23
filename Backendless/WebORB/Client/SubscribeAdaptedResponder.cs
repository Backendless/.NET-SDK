using Weborb.Types;
using Weborb.V3Types;

namespace Weborb.Client
{
  public class SubscribeAdaptedResponder : Responder<object>, ISubscribeResponder
  {
    public SubscribeAdaptedResponder( ResponseHandler<object> responseHandler, ErrorHandler errorHandler )
      : base( responseHandler, errorHandler )
    {
    }

    public new void ResponseHandler( AsyncMessage asyncMessage )
    {
      IAdaptingType[] bodys = asyncMessage.GetBody();
      foreach (IAdaptingType adaptingType in bodys)
      {
        object message = adaptingType.defaultAdapt();
        base.ResponseHandler( message );
      }
      return;
    }
  }
}
