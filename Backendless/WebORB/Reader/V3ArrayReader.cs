using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Protocols.Amf;
using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class V3ArrayReader : ITypeReader
	{
		#region ITypeReader Members

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int refId = reader.ReadVarInteger();

			if( (refId & 0x1) == 0 )
				return parseContext.getReference( refId >> 1 );

			int arraySize = refId >> 1;
			IAdaptingType adaptingType = null;
			object container = null;

			while( true )
			{
				string str = ReaderUtils.readString( reader, parseContext );

				if( str == null || str.Length == 0 )
					break;

				if( container == null )
				{
					container = new Dictionary<object, object>();
					adaptingType = new AnonymousObject( (IDictionary) container );
					parseContext.addReference( adaptingType );
				}

				object obj = RequestParser.readData( reader, parseContext );
				((IDictionary) container)[ str ] = obj;
			}

			if( adaptingType == null )
			{
				container = new object[ arraySize ];
				adaptingType = new ArrayType( (object[]) container );
				parseContext.addReference( adaptingType );

				for( int i = 0; i < arraySize; i++ )
					((object[]) container)[ i ] = RequestParser.readData( reader, parseContext );
			}
			else
			{
				for( int i = 0; i < arraySize; i++ )
				{
					object obj = RequestParser.readData( reader, parseContext );
					((IDictionary) container)[ i.ToString() ] = obj;
				}
			}

			return adaptingType;
		}

		#endregion
	}
}
