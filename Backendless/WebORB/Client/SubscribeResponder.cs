using Weborb.Types;
using Weborb.V3Types;

namespace Weborb.Client
{
  public class SubscribeResponder : Responder<AsyncMessage>, ISubscribeResponder
  {
    public SubscribeResponder(ResponseHandler<AsyncMessage> responseHandler, ErrorHandler errorHandler) : base(responseHandler, errorHandler)
    {
    }

    public new void ResponseHandler( AsyncMessage asyncMessage )
    {
      base.ResponseHandler( asyncMessage );
    }
  }
}
