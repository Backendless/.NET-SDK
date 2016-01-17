using System;
using System.Xml;

namespace Flashorb.Config
{
	public class ConsoleConfigHandler : ORBConfigHandler
	{
        private string hostName = null;

        public override object Configure( object parent, object configContext, XmlNode section )
        {
            foreach( XmlNode node in section.ChildNodes )
            {
                if( !node.Name.Equals( "hostName" ) )
                    continue;

                string configHostName = node.InnerText.Trim();

                if( configHostName.Length > 0 )
                    this.hostName = configHostName;
            }

            return this;
        }

        public string getHostName()
        {
            return hostName;
        }
	}
}
