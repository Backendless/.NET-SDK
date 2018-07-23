using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.JsonRPC
{
  public sealed class ArrayReader : IJsonReader
  {
    public IAdaptingType read( JsonReader reader, ParseContext parseContext )
    {
      List<IAdaptingType> list = new List<IAdaptingType>();
      ArrayType arrayType = new ArrayType();
      parseContext.addReference( arrayType );

      // skip '{' and move to first element
      reader.Read();

      while( reader.TokenClass != JsonTokenClass.EndArray )
      {
        IAdaptingType type = RequestParser.Read( reader, parseContext );
        list.Add( type );
      }

      if( list.Count > 1 )
      {
        IAdaptingType lastObject = list[ list.Count - 1 ];

        if( lastObject is CacheableAdaptingTypeWrapper )
        {
          IAdaptingType wrappedType = ((CacheableAdaptingTypeWrapper) lastObject).getType();

          if( wrappedType is AnonymousObject )
          {
            IDictionary properties = ((AnonymousObject) wrappedType).Properties;

            if( properties.Contains( ObjectReader.DATESMETA ) )
            {
              ArrayType datesArray = (ArrayType) properties[ ObjectReader.DATESMETA ];
              Object[] dates = (Object[]) datesArray.defaultAdapt();

              foreach( Object date in dates )
              {
                String dateStr = (String) date;

                for( int i = 0; i < list.Count; i++ )
                {
                  IAdaptingType arrayElement = list[ i ];

                  if( arrayElement is StringType )
                  {
                    StringType stringObj = (StringType) arrayElement;

                    if( stringObj.Value.Equals( dateStr ) )
                      list[ i ] = new DateType( new DateTime( long.Parse( dateStr ) * TimeSpan.TicksPerMillisecond ) );
                  }
                }
              }

              list.RemoveAt( list.Count - 1 );
            }
          }
        }
      }
      // skip '}'
      reader.Read();

      return new ArrayType( list.ToArray() );
    }
  }
}