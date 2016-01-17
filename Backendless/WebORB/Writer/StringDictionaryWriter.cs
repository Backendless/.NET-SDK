using System;
using System.Collections;
using System.Collections.Specialized;

using Weborb;
using Weborb.Util;

namespace Weborb.Writer
{
	/// <summary>
	/// Summary description for StringDictionaryWriter.
	/// </summary>
	public class StringDictionaryWriter : AbstractReferenceableTypeWriter
	{
        public override void write( object obj, IProtocolFormatter writer )
		{
			StringDictionary dictionary = (StringDictionary) obj;
            writer.BeginWriteObjectMap( dictionary.Count );

			IEnumerator enumerator = dictionary.GetEnumerator();

			while( enumerator.MoveNext() )
			{
				DictionaryEntry entry = (DictionaryEntry) enumerator.Current;
                writer.WriteFieldName( (string) entry.Key );
                writer.BeginWriteFieldValue();
				MessageWriter.writeObject( entry.Value, writer );
                writer.EndWriteFieldValue();
			}

            writer.EndWriteObjectMap();
		}
	}
}
