using System;
using Weborb.Reader;

namespace Weborb.Reader.JsonRPC
{
  public class JsonNumberObject : NumberObject
  {
    private double jsonNumber;

    public JsonNumberObject( double data ) : base( data )
    {
      this.jsonNumber = data;
    }

    public override Object defaultAdapt()
    {
      if( jsonNumber == (int) jsonNumber )
        return Convert.ToDouble( jsonNumber );

      long longValue = Convert.ToInt64( jsonNumber );

      if( longValue >= int.MinValue && longValue <= int.MaxValue )
        return Convert.ToInt32( jsonNumber );
      else if( longValue >= long.MinValue && longValue <= long.MaxValue )
        return longValue;

      return jsonNumber;
    }
  }
}
