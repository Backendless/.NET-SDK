using System;
using System.Collections.Generic;

using Weborb.Types;

namespace Weborb.Reader
{
	public class ParseContext
	{
		private List<IAdaptingType> references;
        private List<String> stringReferences;
        private List<Object> classInfos;
		private int version;
        private Dictionary<int, ParseContext> cachedContext;

    public List<IAdaptingType> parsedObjects = new List<IAdaptingType>();
    public List<RefObject> RefObjects = new List<RefObject>();
     

		public ParseContext()
		{
            references = new List<IAdaptingType>();
            stringReferences = new List<String>();
			classInfos = new List<Object>();
		}

		public ParseContext( int version ) : this()
		{
			this.version = version;
		}

        public ParseContext getCachedContext( int version )
        {
            if( cachedContext == null )
                cachedContext = new Dictionary<int, ParseContext>();

            if( !cachedContext.ContainsKey( version ) )
                cachedContext[ version ] = new ParseContext( version );

            return cachedContext[ version ];
        }

    public void addReference( IAdaptingType type )
		{
			references.Add( type );
		}

		public IAdaptingType getReference( int pointer )
		{
			return (IAdaptingType) references[ pointer ];
		}

        public void addReference( IAdaptingType adaptingType, int index )
        {
            references.Capacity = index + 1;
            references[ index ] = adaptingType;
        }

		public void addStringReference( string refStr )
		{
			stringReferences.Add( refStr );
		}

		public string getStringReference( int index )
		{
			return (string) stringReferences[ index ];
		}

		public void addClassInfoReference( object val )
		{
			classInfos.Add( val );
		}

		public object getClassInfoReference( int index )
		{
			return classInfos[ index ];
		}

		public int getVersion()
		{
			return version;
		}

    public int addParsedObject( IAdaptingType obj )
    {
      parsedObjects.Add( obj );
      return parsedObjects.Count - 1;
    }

    public void setParsedObject( int index, IAdaptingType obj )
    {
      parsedObjects[ index ] = obj;
    }

    public IAdaptingType getParsedObject( int index )
    {
      try 
      { 
        return parsedObjects[ index ]; 
      }
      catch( Exception e ) 
      {
        throw new Exception( "Wrong parsedObjects index: " + index, e );
      }
    }

    public void setRefObjects()
    {
      foreach( RefObject refObject in RefObjects )
      {
        try
        {
          refObject.Object = getParsedObject( refObject.Id - 1 );
        }
        catch( Exception e )
        {
          throw new Exception( "Wrong object reference: " + refObject.Id, e );
        }
      }
    }
	}
}
