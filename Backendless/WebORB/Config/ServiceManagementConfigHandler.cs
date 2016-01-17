using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Weborb.Config
{
    public class ServiceManagementConfigHandler : ORBConfigHandler
    {
        private List<String> excludedAssemblies = new List<String>();

        public override object Configure(object parent, object configContext, System.Xml.XmlNode section)
        {
            XmlNodeList excludedAssembliesNodeList = ((XmlElement)section).SelectNodes( "assemblies/exclude/assembly" );

            foreach( XmlNode node in excludedAssembliesNodeList )
                excludedAssemblies.Add( node.InnerText.Trim().ToLower() );
            
            return this;
        }

        public List<String> ExcludedAssemblies
        {
            get
            {
                return excludedAssemblies;
            }
        }
    }
}
