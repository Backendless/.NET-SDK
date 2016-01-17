using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;

namespace Weborb.Util
{
    public class NetUtils
    {
        [NonSerialized()]
        private static IPAddress[] localHosts;

        static NetUtils()
        {
            localHosts = GetLocalHosts();		
        }

        public static IPAddress[] GetLocalHosts()
        {
            IPAddress[] addressList1 = Dns.GetHostEntry( "localhost" ).AddressList;
            IPAddress[] addressList2 = Dns.GetHostEntry( Dns.GetHostName() ).AddressList;

            IPAddress[] localHosts = new IPAddress[ addressList1.Length + addressList2.Length + 1 ];
            Array.Copy( addressList1, 0, localHosts, 0, addressList1.Length );
            Array.Copy( addressList2, 0, localHosts, addressList1.Length, addressList2.Length );
            localHosts[ addressList1.Length + addressList2.Length ] = IPAddress.Loopback;
            return localHosts;
        }

        public static bool RequestIsLocal( HttpRequest request )
        {
            return request.UserHostAddress == "127.0.0.1" ||
                ("127.0.0.1".Equals( request.ServerVariables[ "HTTP_X_FORWARDED_FOR" ] ) && request.UserHostAddress == request.ServerVariables[ "LOCAL_ADDR" ] );
        } 

        public static bool LocalHostCheck( string remoteHost )
        {
            if( remoteHost.ToLower().Equals( "localhost" ) )
                return true;

            if( remoteHost.ToLower().Equals( "127.0.0.1" ) )
                return true;

            IPHostEntry hostEntry;

            try
            {
                hostEntry = Dns.GetHostEntry( remoteHost );
            }
            catch( Exception )
            {
                return false;
            }

            IPAddress[] remoteAddreses = hostEntry.AddressList;

            foreach( IPAddress remoteAddr in remoteAddreses )
            {
                foreach( IPAddress addr in localHosts )
                    if( addr.Equals( remoteAddr ) )
                        return true;

                if( remoteAddr.Equals( IPAddress.Loopback ) )
                    return true;
            }

            return false;
        }
    }
}
