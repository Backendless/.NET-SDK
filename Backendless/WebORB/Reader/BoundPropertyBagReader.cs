using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Weborb.Types;
using Weborb.Protocols.Amf;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class BoundPropertyBagReader : ITypeReader
	{
		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			//int capacity = reader.ReadInt32();
            int capacity = reader.ReadInteger();
            Dictionary<String, Object> properties = new Dictionary<String, Object>( capacity );
			AnonymousObject anonymousObject = new AnonymousObject( properties );
			parseContext.addReference( anonymousObject );

			while( true )
			{
				String propName = reader.ReadUTF();
				object obj =  RequestParser.readData( reader, parseContext );

				if( obj == null )
					break;

				properties[ propName ] = obj;
			}

			return anonymousObject;
		}
	}
}
