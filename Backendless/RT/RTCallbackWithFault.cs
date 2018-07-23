using System;
using BackendlessAPI.Async;
using Weborb.Types;

namespace BackendlessAPI.RT
{
  public class RTCallbackWithFault : IRTCallback
  {
    public Type Type => throw new NotImplementedException();

    public object UsersCallback => throw new NotImplementedException();

    public ResponseHandler<IAdaptingType> responseHandler => throw new NotImplementedException();

    public ErrorHandler errorHandler => throw new NotImplementedException();
  }
}
