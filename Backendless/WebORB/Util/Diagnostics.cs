using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Reflection;
#if CLOUD
using Microsoft.WindowsAzure.ServiceRuntime;
using Weborb.Cloud;
#endif
using Weborb.Util.License;

namespace Weborb.Util
{
    class ConfigValue
    {
        public String label;
        public object value;

        public ConfigValue( String label, object value )
        {
            this.label = label;
            this.value = value;
        }
    }

    internal class Diagnostics
    {
        internal static void RunDiagnostics( HttpResponse response )
        {
            Dictionary<String, ConfigValue> sections = GetSections();

            response.ContentType = "text/html";
            HtmlTextWriter htmlWriter = new HtmlTextWriter( response.Output );
            htmlWriter.RenderBeginTag( HtmlTextWriterTag.Html );
            htmlWriter.RenderBeginTag( HtmlTextWriterTag.Head );
            htmlWriter.RenderBeginTag( HtmlTextWriterTag.Style );
            htmlWriter.Write( 

                
                "body, td {                   \n" +
                "    font-family: Tahoma;  \n" +
                "    font-size: 10pt;      \n" +
                "   }                     \n" +
                ".header {                \n" +
                "    margin-top:20px;     \n" +
                "    color:#ffffff;       \n" +
                "    width:100%;          \n" +
                "    background:#000088;  \n" +
                "    padding:3px;         \n" +
                "    font-weight:bold;    \n" +
                "   }                     \n" +
                ".row {                   \n" +
                "    border: solid 1px;   \n" +
                "    }                    \n" +
                ".altrow {                \n" +
                "    background: #eeeeee; \n" +
                "    }                    \n" +
                ".keycell {               \n" +
                "    width:30%;           \n" +
                "    vertical-align: top; \n" +
                "    text-align: left;    \n" +
                "}                        \n" +
                ".sectiontable {          \n" +
                "    width:100%;           \n" +
                "    border:solid 1px #cccccc;  \n" +
                "}                        \n" +
                ".wrongValue{             \n" +
                "    color:#ff0000;       \n" +
                "    font-weight:bold;    \n" +
                "}                        \n" +                

                ".valuecell {             \n" +
                "    width:70%            \n" +
                "    vertical-align: top; \n" +
                "    text-align: left;    \n" +
                " }\n" );
            htmlWriter.RenderEndTag();
            htmlWriter.RenderEndTag();

            htmlWriter.RenderBeginTag( HtmlTextWriterTag.Body );

            htmlWriter.RenderBeginTag( HtmlTextWriterTag.H3 );
            htmlWriter.Write( "WebORB Diagnostics Utility" );
            htmlWriter.RenderEndTag();

            htmlWriter.RenderBeginTag( HtmlTextWriterTag.P );
            htmlWriter.Write( "This utility checks for the most common deployment and configuration errors. It verifies presence of the configuration files, assemblies and security permissions required for the normal product operation. If a particular configuration is invalid, it will be reported in " );

            htmlWriter.AddAttribute( HtmlTextWriterAttribute.Class, "wrongValue" );
            htmlWriter.RenderBeginTag( HtmlTextWriterTag.Span );
            htmlWriter.Write( "red, bold typeface" );
            htmlWriter.RenderEndTag();
            htmlWriter.Write( "." );

            htmlWriter.RenderEndTag();

            foreach( String key in sections.Keys )
            {
                htmlWriter.AddAttribute( HtmlTextWriterAttribute.Class, "header" );
                htmlWriter.RenderBeginTag( HtmlTextWriterTag.Div );
                htmlWriter.Write( key );
                htmlWriter.RenderEndTag();

                ConfigValue configValue = sections[ key ];
                renderConfigValue( configValue, htmlWriter );
            }

            htmlWriter.RenderEndTag();
            htmlWriter.RenderEndTag();
            response.Flush();
            htmlWriter.Close();
        }

        private static Dictionary<String, ConfigValue> GetSections()
        {
            Dictionary<String, ConfigValue> sections = new Dictionary<String, ConfigValue>();

            // DEPLOYMENT SUMMARY
            Dictionary<String, ConfigValue> depSumSection = new Dictionary<String, ConfigValue>();
            depSumSection[ "weborbDllVersion" ] = new ConfigValue( "weborb assembly version", null );
            depSumSection[ "weborbAssemblyPath1" ] = new ConfigValue( "weborb assembly deployment location", null );
            depSumSection[ "weborbAssemblyPath2" ] = new ConfigValue( "weborb assembly location used by ASP.NET", null );            
            depSumSection[ "weborbConfig" ] = new ConfigValue( "weborb.config is deployed", "yes" );
            depSumSection[ "flexFolders" ] = new ConfigValue( "WEB-INF/flex folder/files are deployed", "yes" );
            depSumSection[ "weborbHandler" ] = new ConfigValue( "weborb.aspx handler is registered", "yes" );
            depSumSection[ "codegenHandler" ] = new ConfigValue( "codegen.aspx handler is registered", "yes" );
            depSumSection[ "hasApplicationsFolder" ] = new ConfigValue( "Applications folder is present for messaging apps", null );
            depSumSection[ "hasLogFolder" ] = new ConfigValue( "/logs directory exists", "yes" );
            depSumSection[ "loadedAssemblies" ] = new ConfigValue( "assemblies loaded in the application domain", null );
            sections[ "WebORB Deployment Summary" ] = new ConfigValue( "", depSumSection );

            // CONFIGURATION - weborb.config
            Dictionary<String, ConfigValue> configSection = new Dictionary<String, ConfigValue>();
            configSection[ "weborbConfigVersion" ] = new ConfigValue( "weborb.config version number", "4.0" );
            configSection[ "weborbConfigParsable" ] = new ConfigValue( "weborb.config parsing errors", "none" );
            sections[ "weborb.config Summary" ] = new ConfigValue( "", configSection );

            // PERMISSIONS 
            Dictionary<String, ConfigValue> secSection = new Dictionary<String, ConfigValue>();
            secSection[ "logWriter" ] = new ConfigValue( "WebORB can write log files", "yes" );
            secSection[ "codeGenWriter" ] = new ConfigValue( "WebORB can generate client code", "yes" );
            secSection[ "weborbConfigWriter" ] = new ConfigValue( "Changes from console can be saved in weborb.config", "yes" );
            sections[ "WebORB Permissions Summary" ] = new ConfigValue( "", secSection );

            // MACHINE INFO
            Dictionary<String, ConfigValue> boxInfoSection = new Dictionary<String, ConfigValue>();
            //boxInfoSection[ "physCPU" ] = new ConfigValue( "Number of physical CPUs", null );
            //boxInfoSection[ "logCPU" ] = new ConfigValue( "Number of logical CPUs", null );
            //boxInfoSection[ "htStatus" ] = new ConfigValue( "Hyper-threading status", null );
            boxInfoSection[ "machineName" ] = new ConfigValue( "Machine name", null );
            sections[ "Computer Configuration Summary" ] = new ConfigValue( "", boxInfoSection );
            return sections;
        }

        private static void renderConfigValue( ConfigValue configValue, HtmlTextWriter htmlWriter )
        {
            if( configValue.value is IDictionary<String, ConfigValue> )
            {
                htmlWriter.AddAttribute( HtmlTextWriterAttribute.Class, "sub" );                
                htmlWriter.RenderBeginTag( HtmlTextWriterTag.Div );                
                IDictionary<String, ConfigValue> config = (IDictionary<String, ConfigValue>) configValue.value;
                //int[] cpuInfo = LicenseManager.GetInstance().getCPUInfo();

                htmlWriter.AddAttribute( HtmlTextWriterAttribute.Class, "sectiontable" );                
                htmlWriter.RenderBeginTag( HtmlTextWriterTag.Table );

                bool altRow = true;
                Assembly weborbAssembly = AppDomain.CurrentDomain.Load( "weborb" );
                //LicenseManager lm = LicenseManager.GetInstance();

                foreach( String key in config.Keys )
                {
                    object value = null;

                    try
                    {
                        switch (key)
                        {
                            case "weborbDllVersion":
                                value = weborbAssembly.GetName().Version.ToString();
                                break;

                            case "loadedAssemblies":
                                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                                if (assemblies != null && assemblies.Length > 0)
                                {
                                    value = "<ul>";
                                    foreach (Assembly assembly in assemblies)
                                        value += "<li><b>" + assembly.FullName +  "</b>, Location=" +  assembly.Location + "</li>";
                                    value += "</ul>";
                                }
                                else
                                {
                                    value = "none";
                                }
                                break;

                            case "weborbAssemblyPath1":
                                value = weborbAssembly.CodeBase;
                                break;

                            case "weborbAssemblyPath2":
                                value = weborbAssembly.Location;
                                break;

                            case "weborbConfig":
                                value = File.Exists(Path.Combine(Paths.GetWebORBPath(), "weborb.config")) ? "yes" : "no";
                                break;

                            case "flexFolders":
                                value = checkFlexFolders();
                                break;
                            case "weborbHandler":
                                value = checkHttpHandler("Weborb.ORBHttpHandler");
                                break;
                            case "codegenHandler":
                                value = checkHttpHandler("Weborb.Management.CodeGen.CodegeneratorHttpHandler");
                                break;

                            case "weborbConfigParsable":
                                try
                                {
                                  Weborb.Config.ORBConfig conf = Weborb.Config.ORBConfig.GetInstance();
                                    value = "none";
                                }
                                catch (Exception exception)
                                {
                                    value = exception;
                                }
                                break;

                            case "hasLogFolder":
                                value = Directory.Exists(Path.Combine(Paths.GetWebORBPath(), "logs")) ? "yes" : "no";
                                break;

                            case "hasApplicationsFolder":
#if CLOUD
                                value = Directory.Exists( Path.Combine( AzureUtil.LocalResourceRoot, 
                                                                                  "Applications" ) ) ? "yes" : "no";
#else
                                value = Directory.Exists(Path.Combine(Paths.GetWebORBPath(), 
                                                                                     "Applications")) ? "yes" : "no";
#endif
                                break;

                            // CONFIGURATION - weborb.config
                            case "weborbConfigVersion":

                                XmlTextReader reader = null;

                                try
                                {
                                    reader = new XmlTextReader(Path.Combine(Paths.GetWebORBPath(), "weborb.config"));
                                    XPathDocument xpathDoc = new XPathDocument(reader);
                                    XPathNavigator navigator = xpathDoc.CreateNavigator();
                                    XPathNodeIterator nodeIterator = navigator.Select("/configuration/weborb[@version]");
                                    if (nodeIterator.Count == 0)
                                        value = "pre 3.0";
                                    else
                                    {
                                        nodeIterator.MoveNext();
                                        value = nodeIterator.Current.GetAttribute("version", "");
                                    }
                                }
                                catch (Exception exception)
                                {
                                    value = exception;
                                }
                                finally
                                {
                                    if (reader != null)
                                        reader.Close();
                                }

                                break;

                            case "weborbConfigWriter":
                                FileStream stream = null;

                                try
                                {
#if CLOUD
                                    var blob = AzureUtil.GetBlob( "weborb.config" );
                                    value = blob.Exists() ? "yes" : "no";
#else
                                    stream = File.OpenWrite(Path.Combine(Paths.GetWebORBPath(), "weborb.config"));
                                    value = "yes";
#endif
                                }
                                catch (Exception exception)
                                {
                                    value = exception;
                                }
                                finally
                                {
                                    if (stream != null)
                                        stream.Close();
                                }

                                break;

                            // MACHINE INFO
                            /*
                            case "physCPU":
                                value = cpuInfo[ 1 ];
                                break;

                            case "logCPU":
                                value = cpuInfo[ 0 ];
                                break;

                            case "htStatus":
                                switch( cpuInfo[ 2 ] )
                                {
			   	                case 0:
			   	                   value = "HT is not available";
				                   break;
            			
			                   case 2:
				                   value = "HT is disabled";
				                   break;
                			
			                   case 1:
				                   value = "HT is enabled";
				                   break;
                			
			                   case 3:
				                   value = "HT is available but disabled";
				                   break;
                			
			                   case 4:
				                   value = "HT cannot be detected";
                                   break;
                            }
                            break;
                             */

                            case "machineName":
                                value = Environment.MachineName;
                                break;

                            case "logWriter":
                                value = testFileWrite("logs");
                                break;

                            case "codeGenWriter":
                                value = testFileWrite("weborbassets" + Path.DirectorySeparatorChar + "codegen");
                                break;
                        }
                    }
                    catch { }

                    if( altRow )
                        htmlWriter.AddAttribute( HtmlTextWriterAttribute.Class, "altrow" );
                        
                    altRow = !altRow;

                    htmlWriter.RenderBeginTag( HtmlTextWriterTag.Tr );

                        ConfigValue confV = config[ key ];

                        htmlWriter.AddAttribute( HtmlTextWriterAttribute.Class, "keycell" );
                        htmlWriter.RenderBeginTag( HtmlTextWriterTag.Td );
                            htmlWriter.Write( confV.label );
                        htmlWriter.RenderEndTag();

                        String className = "valuecell";

                        if( confV.value != null && !confV.value.Equals( value ) )
                            className += " wrongValue";

                        htmlWriter.AddAttribute( HtmlTextWriterAttribute.Class, className );
                        htmlWriter.RenderBeginTag( HtmlTextWriterTag.Td );                        

                            if( value is Exception )
                                value = value.ToString().Replace( "\n", "<br>" );

                            htmlWriter.Write( value );

                        htmlWriter.RenderEndTag();

                    htmlWriter.RenderEndTag();
                }

                htmlWriter.RenderEndTag();
                htmlWriter.RenderEndTag();
            }
        }

        private static Object testFileWrite( String folder )
        {
            String logsFolder = Path.Combine( Paths.GetWebORBPath(), folder );

            if( Directory.Exists( logsFolder ) )
            {
                String tempFile = Path.Combine( logsFolder, Guid.NewGuid().ToString() );
                FileStream stream = null;

                try
                {
                    stream = File.OpenWrite( tempFile );
                    stream.WriteByte( 1 );
                    stream.Flush();
                    return "yes";
                }
                catch( Exception exception )
                {
                    return exception;
                }
                finally
                {
                    if( stream != null )
                        stream.Close();

                    try { File.Delete( tempFile ); }
                    catch( Exception ) { }
                }
            }
            else
            {
                return "/" + folder + " directory does not exist";
            }                            
        }

        private static Object checkHttpHandler( String handler )
        {
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader( Path.Combine( Paths.GetWebORBPath(), "web.config" ) );
                XPathDocument xpathDoc = new XPathDocument( reader );
                XPathNavigator navigator = xpathDoc.CreateNavigator();
                XPathNodeIterator nodeIterator = navigator.Select( "/configuration/system.web/httpHandlers/add[@type='" + handler + "']" );
                return nodeIterator.Count == 0 ? "no" : "yes";
            }
            catch( Exception exception )
            {
                return exception;
            }
            finally
            {
                if( reader != null )
                    reader.Close();
            }                            
        }

        private static String checkFlexFolders()
        {
            string webInfPath = Path.Combine( Paths.GetWebORBPath(), "WEB-INF" );

            if( !Directory.Exists( webInfPath ) )
                return "/WEB-INF is missing";

            string flexPath = Path.Combine( webInfPath, "flex" );

            if( !Directory.Exists( flexPath ) )
                return "/WEB-INF/flex is missing";

            if( !File.Exists( Path.Combine( flexPath, "services-config.xml" ) ) )
                return "/WEB-INF/flex/services-config.xml is missing";

            if( !File.Exists( Path.Combine( flexPath, "remoting-config.xml" ) ) )
                return "/WEB-INF/flex/remoting-config.xml is missing";

            if( !File.Exists( Path.Combine( flexPath, "messaging-config.xml" ) ) )
                return "/WEB-INF/flex/messaging-config.xml is missing";

            if( !File.Exists( Path.Combine( flexPath, "data-management-config.xml" ) ) )
                return "/WEB-INF/flex/data-management-config.xml is missing";
            return "yes";
        }
    }
}
