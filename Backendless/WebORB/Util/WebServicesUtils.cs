using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Xml;
using System.Web;
using System.Web.SessionState;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using Description = System.Web.Services.Description;

using Microsoft.CSharp;
using Weborb.Activation;
using Weborb.Types;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Handler;
using Weborb.Inspection;

namespace Weborb.Util
{
    class WebServicesUtils
    {
        internal static WebServiceInfo createServiceInfo(string wsdlURL)
          {
          try
            {
            XmlTextReader reader = new XmlTextReader(wsdlURL);
            ServiceDescription wsdl = ServiceDescription.Read(reader);
            Assembly assembly = generateAssembly(wsdl);
            Type serviceType = resolveServiceType(assembly);
            return new WebServiceInfo(serviceType, wsdl);
            }
          catch (Exception e)
            {
            // this isn't right wsdlURL resource
            if ( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log(LoggingConstants.ERROR, "Resource isn't right web service: " + e.Message);
            if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
              Log.log( LoggingConstants.EXCEPTION, e );

            return null;      
            }
          }

        internal static Assembly generateAssembly(ServiceDescription wsdl)
        {
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.AddServiceDescription(wsdl, "", "");
            CodeNamespace name = new CodeNamespace("");
            CodeCompileUnit code = new CodeCompileUnit();
            code.Namespaces.Add(name);
            ServiceDescriptionImportWarnings warnings = importer.Import(name, code);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();
            CompilerParameters parms = new CompilerParameters();
            parms.ReferencedAssemblies.Add("mscorlib.dll");
            parms.ReferencedAssemblies.Add("System.Data.dll");
            parms.ReferencedAssemblies.Add("System.XML.dll");
            parms.ReferencedAssemblies.Add("System.Web.Services.dll");
            //parms.GenerateInMemory = true;
            CompilerResults results = compiler.CompileAssemblyFromDom(parms, code);

            if (results.Errors.Count > 0 && Log.isLogging(LoggingConstants.DEBUG))
                foreach (CompilerError error in results.Errors)
                    Log.log(LoggingConstants.DEBUG, "Error: " + error);

            return results.CompiledAssembly;
        }


        internal static Type resolveServiceType(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
                if (type.BaseType.Equals(typeof(SoapHttpClientProtocol)))
                    return type;

            return null;
        }

        internal static object generateProxy(Type type)
        {
            try
            {
                HttpRequest request = (HttpRequest)ThreadContext.currentRequest();
                NameValueCollection parameters = request.QueryString;
                string[] values = parameters.GetValues("activate");
                IActivator activator = Activators.GetDefaultActivator();

                if (values != null)
                {
                    String activationMode = values[0];
                    activator = Activators.Get(activationMode);
                }

                object client = activator.Activate(type);
                return client;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Trace.WriteLine(exception);
                System.Diagnostics.Trace.WriteLine(exception.InnerException);
                throw exception;
            }
        }
    }
}
