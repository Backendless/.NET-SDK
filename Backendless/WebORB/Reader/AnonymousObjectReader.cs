using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Weborb.Protocols.Amf;
using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class AnonymousObjectReader : ITypeReader
	{
		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
      Dictionary<object, object> properties = new Dictionary<object, object>();
			AnonymousObject anonymousObject = new AnonymousObject( properties );
			parseContext.addReference( anonymousObject );

			while( true )
			{
				string propName = reader.ReadUTF();
				object obj = null;

				int dataType = reader.ReadByte();

				if( dataType.Equals( Datatypes.REMOTEREFERENCE_DATATYPE_V1 ) && !propName.Equals( "nc" ) )
					obj = 0d; // must be an instance of Flash's Number
				else
					obj = RequestParser.readData( dataType, reader, parseContext );

				if( obj == null )
					break;

				properties[ propName ] = obj;
				}

			if( properties.Count == 1 && properties.ContainsKey( "nc" ) )
				return (IAdaptingType) properties[ "nc" ];

			return anonymousObject;
		}
	}
}
