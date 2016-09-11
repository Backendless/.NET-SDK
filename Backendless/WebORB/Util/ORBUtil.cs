using System;
using System.IO;
using System.Reflection;
#if( !UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
using System.Web;
#endif
using System.Text;
using System.Collections;
using Weborb.Message;
using Weborb.Reader;
using Weborb.Writer;

namespace Weborb.Util
{
    public delegate void WeborbDisposedHandler();
    /// <summary>
    /// Summary description for ORBUtil.
    /// </summary>
    public class ORBUtil
    {
        public static event WeborbDisposedHandler WeborbDisposed;

        public static void InvokeWeborbDisposed()
        {
          WeborbDisposedHandler handler = WeborbDisposed;
          if (handler != null) handler();
        }

        private ORBUtil()
        {
        }

        public static bool IsRemotingRequest( string contentType )
        {
            return contentType != null && contentType.ToLower().Equals( "application/x-amf" );
        }

        public static void SerializeResponse( Request message, Stream stream )
        {
            IProtocolFormatter formatter = message.GetFormatter();
            MessageWriter.writeObject( message, formatter );
            ProtocolBytes protocolBytes = formatter.GetBytes();
            formatter.Cleanup();
            stream.Write( protocolBytes.bytes, 0, protocolBytes.length );
            stream.Flush();
        }
#if( !UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
        public static string[] GetActivatorQueryStringValues()
        {
            string[] values = null;
            HttpRequest request = (HttpRequest) ThreadContext.currentRequest();

            // request will be null when not hosted in IIS/ASP.Net
            if( request != null )
                values =  request.QueryString.GetValues( "activate" );

            return values;
        }
#endif
        public static string GetConsoleHtml( string absoluteUri, string hostAddress )
        {
            string consoleAddress = "/console/orbconsole.swf";

            StringBuilder gwURL = new StringBuilder();
            gwURL.Append( ORBConstants.GATEWAYURL_EQUALS );
            gwURL.Append( absoluteUri );
            gwURL.Append( ORBConstants.ANDPERSANDHOSTADDRESS );
            gwURL.Append( hostAddress );

            StringBuilder consoleHTML = new StringBuilder();
            consoleHTML.Append( "<HTML>\n" );
            consoleHTML.Append( " <HEAD>\n" );
            consoleHTML.Append( "   <meta http-equiv=Content-Type content=\"text/html;  charset=ISO-8859-1\">\n" );
            consoleHTML.Append( "   <TITLE>WebORB Management Console</TITLE>\n" );
            consoleHTML.Append( " </HEAD>\n" );
            consoleHTML.Append( " <BODY bgcolor=\"#000000\">\n" );
            consoleHTML.Append( "   <table width=\"100%\" height=\"100%\"");
            consoleHTML.Append( "border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n" );
            consoleHTML.Append( "     <tr>\n");
            consoleHTML.Append( "       <td align=\"center\">\n");
            consoleHTML.Append( "         <OBJECT classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" \n");
            consoleHTML.Append( "           codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0\" \n");
            consoleHTML.Append( "           WIDTH=\"832\" HEIGHT=\"662\" id=\"orbconsole\" ALIGN=\"\">\n");
            consoleHTML.Append( "           <PARAM NAME=movie VALUE=" + consoleAddress + ">\n");
            consoleHTML.Append( "           <PARAM NAME=quality VALUE=high>\n");
            consoleHTML.Append( "           <PARAM NAME=FlashVars VALUE=" + gwURL.ToString() + ">\n");
            consoleHTML.Append( "           <PARAM NAME=bgcolor VALUE=#3F4C5B>\n");
            consoleHTML.Append( "           <EMBED src=" + consoleAddress.ToString() + "\n" );
            consoleHTML.Append( "             quality=high bgcolor=#3F4C5B  WIDTH=\"831\" HEIGHT=\"662\" \n" );
            consoleHTML.Append( "             flashvars=\"" + gwURL.ToString() + "\" \n" );
            consoleHTML.Append( "             NAME=\"orbconsole\" ALIGN=\"\" \n" );
            consoleHTML.Append( "             TYPE=\"application/x-shockwave-flash\" \n" );
            consoleHTML.Append( "             PLUGINSPAGE=\"http://www.macromedia.com/go/getflashplayer\"> \n" );
            consoleHTML.Append( "           </EMBED>\n" );
            consoleHTML.Append( "         </Object>\n");
            consoleHTML.Append( "       </td>\n");
            consoleHTML.Append( "     </tr>\n");
            consoleHTML.Append( "   </table>\n");
            consoleHTML.Append( "</HTML>" );

            return consoleHTML.ToString();
        }

        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long currentTimeMillis()
        {
            return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
        }
#if( !UNIVERSALW8 && !SILVERLIGHT  && !WINDOWS_PHONE8)
        public static int GetMajorNETVersionOfWeborb()
          {
          AssemblyName[] assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

          // check for NET 4 (ImageRuntimeVersion could starts only from v4 or v2)
          if (Assembly.GetExecutingAssembly().ImageRuntimeVersion.StartsWith("v4")) 
            return 4;

          // check for NET 3 
          foreach (AssemblyName assemblyName in assemblies)
            {
            if (assemblyName.Name == "System.ServiceModel")
                return 3;
            }

          // otherwise NET 2
          return 2;          
          }
#endif
    }
}

