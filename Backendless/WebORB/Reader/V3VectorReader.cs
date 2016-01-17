using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Protocols.Amf;
using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
  {
  class V3VectorReader<T> : ITypeReader
    {
    private ITypeReader objectReader = new V3ObjectReader();

    public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
      {
      int handle = reader.ReadVarInteger();
      bool inline = ( ( handle & 1 ) != 0 ); 
      handle = handle >> 1;
      if ( inline )
        {
        object[] array = new object[ handle ];

        ArrayType ar = new ArrayType( array );
        parseContext.addReference( ar );

        // whether vector is readonly
        int @fixed = reader.ReadVarInteger();

        if ( !typeof( T ).IsValueType )
          {
          // type name of the vector's elements
          string elementTypeName = ReaderUtils.readString( reader, parseContext );
          }

        for ( int i = 0; i < handle; i++ )
          {
          if ( typeof( T ) == typeof( int ) ) 
            array[ i ] = reader.ReadInteger();
          else if ( typeof( T ) == typeof( uint ) )
            array[ i ] = reader.ReadUInteger();
          else if ( typeof( T ) == typeof( double ) )
            array[ i ] = reader.ReadDouble();
          else
            {
            array[ i ] = RequestParser.readData( reader, parseContext );
            //array[ i ] = objectReader.read( reader, parseContext );
            }
          }        
        
        return ar;
        }
      else
        {
        return parseContext.getReference( handle );
        }
      }
    }
  }
