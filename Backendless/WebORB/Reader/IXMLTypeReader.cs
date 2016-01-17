using System;
using Weborb.Types;
using System.Xml;

namespace Weborb.Reader
{
	public interface IXMLTypeReader
	{
        IAdaptingType read( XmlElement element, ParseContext parseContext );
	}
}
