using Weborb.V3Types;

namespace Weborb.Client
{
  public interface ISubscribeResponder
  {
    void ResponseHandler(AsyncMessage asyncMessage);
  }
}
