using System.Reflection;

namespace Weborb.Service
  {
  interface IMethodAmbiguityResolver
    {
    MethodInfo ResolveMethod(MethodInfo[] methods, object[] arguments);
    }
  }
