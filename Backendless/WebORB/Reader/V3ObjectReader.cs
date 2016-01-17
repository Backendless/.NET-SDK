using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Protocols.Amf;
using Weborb.Types;
using Weborb.Util;
using Weborb.Util.IO;
using Weborb.Util.Logging;

namespace Weborb.Reader
{
	public class V3ObjectReader : ITypeReader
	{
		#region ITypeReader Members

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int refId = reader.ReadVarInteger();

			if( (refId & 0x1) == 0 )
				return (IAdaptingType) parseContext.getReference( refId >> 1 );

			ClassInfo classInfo = getClassInfo( refId, reader, parseContext );

		  Type mappedType = null;
      if ( !string.IsNullOrEmpty( classInfo.className ) ) 
        mappedType = Types.Types.getServerTypeForClientClass( classInfo.className );

      if ( classInfo.externalizable || typeof( IExternalizable ).IsAssignableFrom( mappedType ) )
            {
                Type type = Types.Types.getServerTypeForClientClass( classInfo.className );
                object extobj = null;

                if( type != null )
                    extobj = ObjectFactories.CreateServiceObject( type );
                else
                    extobj = ObjectFactories.CreateServiceObject( classInfo.className );

                if( !(extobj is IExternalizable) )
                {
                    throw new Exception( "object must implement IExternalizable" );
                }
                else
                {
                    CacheableAdaptingTypeWrapper wrapper = new CacheableAdaptingTypeWrapper();
                    parseContext.addReference( wrapper );

                    IAdaptingType returnValue = null;
                    extobj = ((IExternalizable)extobj).readExternal( reader, parseContext );

                    if( extobj is IAdaptingType )
                        returnValue = (IAdaptingType) extobj;
                    else
                        returnValue = new ConcreteObject( extobj );

                    wrapper.setType( returnValue );
                    return returnValue;
                }
            }
            else 
            {
                Dictionary<String, IAdaptingType> props = new Dictionary<String, IAdaptingType>();
                AnonymousObject anonObj = new AnonymousObject();
                IAdaptingType returnValue = anonObj;

                if( classInfo.className != null && classInfo.className.Length > 0 )
                    returnValue = new NamedObject( classInfo.className, anonObj );

                parseContext.addReference( returnValue );
                int propCount = classInfo.getPropertyCount();

                for( int i = 0; i < propCount; i++ )
                {
                    if( Log.isLogging( LoggingConstants.DEBUG ) )
                        Log.log( LoggingConstants.DEBUG, "reading object property " + classInfo.getProperty( i ) );

                    props[ classInfo.getProperty( i ) ] = RequestParser.readData( reader, parseContext );
                }

                if( classInfo.looseProps )
                    while( true )
                    {
                        string propName = ReaderUtils.readString( reader, parseContext );

                        if( propName == null || propName.Length == 0 )
                            break;

                        props[ propName ] = RequestParser.readData( reader, parseContext );
                    }

                anonObj.Properties = props;
                return returnValue;
            }
		}

		#endregion

		private ClassInfo getClassInfo( int refId, FlashorbBinaryReader reader, ParseContext parseContext )
		{
			if( (refId & 0x3) == 1 )
				return (ClassInfo) parseContext.getClassInfoReference( refId >> 2 );

			ClassInfo classInfo = new ClassInfo();
            classInfo.externalizable = (refId & 0x4) == 4; 
			classInfo.looseProps = (refId & 0x8) == 8;			
			classInfo.className = ReaderUtils.readString( reader, parseContext );
			int propsCount = refId >> 4;

			for( int i = 0; i < propsCount; i++ )
				classInfo.addProperty( ReaderUtils.readString( reader, parseContext ) );

			parseContext.addClassInfoReference( classInfo );
			return classInfo;
		}
	}

	class ClassInfo
	{
		internal bool looseProps;
        internal string className;
        internal bool externalizable;
        List<String> props = new List<String>();

		public void addProperty( string propName )
		{
			props.Add( propName );
		}

		public int getPropertyCount()
		{
			return props.Count;
		}

		public string getProperty( int index )
		{
			return props[ index ];
		}
	}
}
