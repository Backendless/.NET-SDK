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
#if !(FULL_BUILD)
      IAdaptingType body = (IAdaptingType)asyncMessage.body;
#else
      IAdaptingType body = (IAdaptingType)( (object[])( asyncMessage.body.body ) )[0];
#endif
      object adaptedMessage = body.defaultAdapt();
      base.ResponseHandler(adaptedMessage);
    }
  }
}
