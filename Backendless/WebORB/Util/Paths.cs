using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using Weborb.Cloud;

namespace Weborb.Util
{
    public class Paths
    {
        private static String path;

        public static String getRemotingURL()
        {
            String absoluteUri = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            int index = absoluteUri.IndexOf("/console/codegen.aspx");
            String weborbUrl = absoluteUri;

            if (index != -1)
            {
                weborbUrl = absoluteUri.Substring(0, index) + "/weborb.aspx";
            }
            else
            {
                index = absoluteUri.IndexOf("/codegen.aspx");

                if (index != -1)
                    weborbUrl = absoluteUri.Substring(0, index) + "/weborb.aspx";
            }

            return weborbUrl;
        }

        public static void SetWebORBPath( String value )
        {
          path = value;
        }

        public static String GetWebORBPath()
        {
            if( path != null )
                return path;

            try
            {
                RegistryKey softwareKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( ORBConstants.SOFTWARE, false );
                RegistryKey midnightKey = softwareKey.OpenSubKey( ORBConstants.MIDNIGHT_CODERS );
                RegistryKey weborbKey = midnightKey.OpenSubKey( ORBConstants.WEBORB );
                path = (String) weborbKey.GetValue( ORBConstants.WEBORBPATH_KEY );
            }
            catch( Exception )
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }

            return path;
        }

        public static String GetUploadPath()
        {
            return GetWebORBPath() + Path.DirectorySeparatorChar + "weborbassets" + Path.DirectorySeparatorChar + "uploads" + Path.DirectorySeparatorChar;
        }

        internal static string GetApplicationsPath()
        {
#if CLOUD
            return Path.Combine( AzureUtil.LocalResourceRoot, "Applications" );
#else 
            return Path.Combine( GetWebORBPath(), "Applications" );
#endif
        }

        internal static string GetLicensesPath()
        {
            return Path.Combine( GetWebORBPath(), "Licenses" );
        }
    }
}
