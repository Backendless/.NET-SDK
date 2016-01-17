using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Weborb.ProxyGen.Core;
using Weborb.ProxyGen.Core.Interceptor;
using Weborb.ProxyGen.DynamicProxy;

namespace ProxyGenTest
{
    class Program
    {
        static void Main( string[] args )
        {
            ProxyGenerator proxyGen = new ProxyGenerator();
            IFoo foo = (IFoo)proxyGen.CreateInterfaceProxyWithoutTarget( typeof( IFoo ), new TestInterceptor() );
            System.Console.WriteLine( "method returned - " + foo.foo() );
        }
    }

    class TestInterceptor : IInterceptor
    {
        #region IInterceptor Members

        public void Intercept( IInvocation invocation )
        {
            System.Console.WriteLine( "intercepted invocation - " + invocation.Method.Name );
            invocation.ReturnValue = "123";
            //invocation.Proceed();
            
        }

        #endregion
    }

    public interface IFoo
    {
        String foo();
        int bar( String s );
    }
}
