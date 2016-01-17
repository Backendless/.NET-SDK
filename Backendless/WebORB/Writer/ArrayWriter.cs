using System;
using System.Collections;
using System.IO;
using Weborb;
using Weborb.Types;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Writer.Amf;

namespace Weborb.Writer
{
  public class ArrayWriter : ITypeWriter
  {
    private MultiDimArrayWriter multiDimArrayWriter = new MultiDimArrayWriter();
    private ITypeWriter referenceWriter = new ArrayReferenceWriter();

    #region ITypeWriter Members

    public void write( object obj, IProtocolFormatter writer )
    {
      Array arrayObj = null;

      if ( obj is IWebORBArray )
      {
        IEnumerator en = ( (IWebORBArray)obj ).GetEnumerator();
        arrayObj = new object[( (IWebORBArray)obj ).Count];

        int i = 0;
        while ( en.MoveNext() )
          arrayObj.SetValue( en.Current, i++ );
      }
      else
      {
        //TODO: test out this cast!
        arrayObj = (Array)obj;
      }

      if ( obj.GetType().IsAssignableFrom(typeof(Byte[])))
      {
        Byte[] byteArray = new byte[arrayObj.Length];
        
        for ( int i = 0; i < arrayObj.Length; i++ )
          byteArray[i] = (Byte)arrayObj.GetValue( i );

        writer.WriteByteArray(byteArray);
        return;
      }

      if ( arrayObj.Rank > 1 )
      {
        multiDimArrayWriter.write( arrayObj, writer );
        return;
      }

      writer.BeginWriteArray( arrayObj.Length );

      bool genericArray = arrayObj.GetType().GetElementType().BaseType == null;

      if ( genericArray )
      {
        for ( int i = 0; i < arrayObj.Length; i++ )
        {
          //Log.log( "REFCACHE", "WRITING ARRAY ELEMENT " + i );
          object value = arrayObj.GetValue( i );
          ITypeWriter typeWriter = MessageWriter.getWriter( value, writer );
          typeWriter.write( value, writer );
          //Log.log( "REFCACHE", "DONE WRITING ARRAY ELEMENT " + i );
        }
      }
      else
      {
        ITypeWriter typeWriter = null;
        ITypeWriter contextWriter = null;


        for ( int i = 0; i < arrayObj.Length; i++ )
        {
          object value = arrayObj.GetValue( i );

          if ( contextWriter == null )
          {
            typeWriter = MessageWriter.getWriter( value, writer );
            contextWriter = writer.getContextWriter();
          }
          else
          {
            writer.setContextWriter( contextWriter );
          }

          typeWriter.write( value, writer );
        }
      }

      writer.EndWriteArray();
    }

    #endregion

    #region ITypeWriter Members

    public ITypeWriter getReferenceWriter()
    {
      return referenceWriter;
    }

    #endregion
  }
}
