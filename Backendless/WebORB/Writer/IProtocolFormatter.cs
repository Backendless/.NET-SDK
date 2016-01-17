using System;
using System.Collections.Generic;
using System.Xml;
using Weborb.Message;
using System.IO;
namespace Weborb.Writer
{
	public abstract class IProtocolFormatter
	{
        private Dictionary<Type, ITypeWriter> cachedWriters = new Dictionary<Type, ITypeWriter>();
        private ITypeWriter contextWriter;

        // for caching purposes
        internal abstract void BeginSelectCacheObject();

        internal abstract object EndSelectCacheObject();

	      internal abstract void WriteCachedObject( object cached );

	      // type mapping override
        public abstract ITypeWriter getWriter( Type type );

        public void setContextWriter( ITypeWriter writer )
        {
            this.contextWriter = writer;
        }

        public ITypeWriter getContextWriter()
        {
            return contextWriter;
        }

        public ITypeWriter getCachedWriter( Type type )
        {
            ITypeWriter writer;

            if( cachedWriters.TryGetValue( type, out writer ) )
                return writer;
            else
                return null;
        }

        public void addCachedWriter( Type type, ITypeWriter typeWriter )
        {
            cachedWriters[ type ] = typeWriter;
        }

		// ********************** REFERENCE CACHE METHODS *******************

        public abstract ReferenceCache GetReferenceCache();

        public abstract void ResetReferenceCache();

        // ********************** LOWER LEVEL METHODS ***********************

        public virtual void DirectWriteBytes( byte[] b )
        {
        }

        public virtual void DirectWriteString( string str )
        {
        }

        public virtual void DirectWriteInt( int i )
        {
        }

        public virtual void DirectWriteBoolean( bool b )
        {
        }

        public virtual void DirectWriteShort( int s )
        {
        }

        // ********************** DATA TYPE SERIALIZATION ***********************

        public virtual void BeginWriteMessage( Request message )
        {
        }

        public virtual void EndWriteMessage()
        {
        }

        public abstract void WriteMessageVersion( float version );

        public virtual void BeginWriteBodyContent()
        {
        }

        public virtual void EndWriteBodyContent()
        {
        }

        public abstract void BeginWriteArray( int length );

        public virtual void EndWriteArray()
        {
        }

        public abstract void WriteBoolean( bool b );

        public abstract void WriteDate( DateTime datetime );

        public abstract void BeginWriteObjectMap( int size );

        public virtual void EndWriteObjectMap()
        {
        }

        public abstract void WriteFieldName( String s );

        public virtual void BeginWriteFieldValue()
        {
        }

        public virtual void EndWriteFieldValue()
        {
        }

        public abstract void WriteNull();

        public abstract void WriteDouble( double number );

        public abstract void WriteInteger( int number );

        public abstract void BeginWriteNamedObject( string objectName, int fieldCount );

        public virtual void EndWriteNamedObject()
        {
        }

        public abstract void BeginWriteObject( int fieldCount );

        public abstract void EndWriteObject();

        public abstract void WriteArrayReference( int refID );

        public abstract void WriteObjectReference( int refID );

        public abstract void WriteDateReference( int refID );

        public abstract void WriteStringReference( int refID );

        public abstract void WriteString( string s );

	      public abstract void WriteByteArray(byte[] array);

#if (FULL_BUILD)
        public abstract void WriteXML( XmlNode document );
#endif
        public abstract IObjectSerializer GetObjectSerializer();

		// **************** UTILITIES *************************************************

        public abstract ProtocolBytes GetBytes();

        public abstract void Cleanup();

        public abstract string GetContentType();
	}

    public class ProtocolBytes
    {
        public byte[] bytes;
        public int length;
    }
}
