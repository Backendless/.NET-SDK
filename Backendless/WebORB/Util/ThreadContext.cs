using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

#if (FULL_BUILD)
using System.Web;
using System.Web.SessionState;
using Weborb.Security;
#endif

using Weborb.Config;

namespace Weborb.Util
{
	public class ThreadContext
	{
#if (FULL_BUILD)
        [ThreadStatic]
        private static HttpContext context;

		[ThreadStatic] 
		private static HttpSessionState currentHttpSession;

		[ThreadStatic] 
		private static HttpRequest currentHttpRequest;
#endif
		[ThreadStatic] 
		private static IDictionary writerCache;

        [ThreadStatic]
        private static IDictionary runtimeConfig;

        //[ThreadStatic]
        //private static ORBConfig config;

        [ThreadStatic]
        private static IDictionary properties;

        public static IDictionary currentWriterCache() 
		{ 
			return writerCache; 
		}

        public static void setWriterCache( IDictionary currentWriterCache ) 
		{ 
			writerCache = currentWriterCache; 
		}

#if (FULL_BUILD)
		public static void setCallerCredentials( Credentials currentCallerCredentials, IPrincipal principal ) 
		{
            if( currentCallerCredentials == null )
                currentHttpSession.Remove( "credentials" );
            else
                currentHttpSession[ "credentials" ] = currentCallerCredentials;

            if( principal == null )
                currentHttpSession.Remove( "principal" );
            else
                currentHttpSession[ "principal" ] = principal;
		}

        public static Credentials currentCallerCredentials() 
		{
            return (Credentials)currentHttpSession[ "credentials" ]; 
		}

		public static HttpSessionState currentSession() 
		{
            return currentHttpSession;
		}

		public static void setCurrentSession( HttpSessionState httpSession ) 
		{ 
			currentHttpSession = httpSession; 
		}

        public static void setCurrentHttpContext( HttpContext httpContext )
        {
            context = httpContext;
        }

        public static HttpContext currentHttpContext()
        {
            return context;
        }

		public static HttpRequest currentRequest() 
		{ 
			return currentHttpRequest; 
		}

		public static void setCurrentRequest( HttpRequest httpRequest ) 
		{ 
			currentHttpRequest = httpRequest; 
		}
#endif

        public static IDictionary getRuntimeConfig()
        {
            if( runtimeConfig == null )
                runtimeConfig = new Dictionary<Object, Object>();

            return runtimeConfig;
        }

        public static IDictionary getProperties()
        {
            if( properties == null )
                properties = new Dictionary<Object, Object>();

            return properties;
        }
	}
}
