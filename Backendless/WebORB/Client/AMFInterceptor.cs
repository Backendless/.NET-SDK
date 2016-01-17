using System;
using System.Threading;
using System.Reflection;

using Weborb.ProxyGen.Core.Interceptor;

namespace Weborb.Client
{
    public class AMFInterceptor : IInterceptor
    {
        private String className;
        private Weborb.Client.WeborbClient weborbClient;

        public AMFInterceptor( String className, Weborb.Client.WeborbClient weborbClient )
        {
            this.className = className;
            this.weborbClient = weborbClient;
        }

        #region IInterceptor Members

        public void Intercept( IInvocation invocation )
        {
            try
            {
                Type returnType = invocation.Method.ReturnType;

                if( returnType.IsGenericType )
                {
                    if( !returnType.GetGenericTypeDefinition().Equals( typeof( AsyncToken<> ) ) )
                        throw new Exception( "Remote invocation return type must be Weborb.Client.AsyncToken. This is required due to asynchronous nature of the remote invocations. Change the return type to AsyncToken" );
                }
                else
                {
                }

                Type methodReturnType;

                if ( returnType.GetGenericArguments().Length > 0 )
                  methodReturnType = returnType.GetGenericArguments()[0];
                else
                  methodReturnType = typeof (object);

                String methodName = invocation.Method.Name;
                object[] arguments = invocation.Arguments;

                Type asyncType = typeof( AsyncToken<> );
                Type[] argType = { methodReturnType };
                Type constructed = asyncType.MakeGenericType( argType );
#if (FULL_BUILD || PURE_CLIENT_LIB)
                object asyncTokenObject = Activator.CreateInstance( constructed, new object[] { invocation, null } );
#else
                object asyncTokenObject = Activator.CreateInstance( constructed, new object[] { invocation, weborbClient.uiControl } );
#endif

                weborbClient.HandleInvocation( invocation, className, methodName, arguments, null, asyncTokenObject );
            }
            catch( Exception exception )
            {
                String str = exception.ToString();
            }
        }

        #endregion
    }
}
