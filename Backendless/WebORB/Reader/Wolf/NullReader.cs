using System;
using System.Xml;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.Wolf
{
	public class NullReader : IXMLTypeReader
	{
        public IAdaptingType read(XmlElement element, ParseContext parseContext)
        {
            return new NullType();
        }
	}
}
