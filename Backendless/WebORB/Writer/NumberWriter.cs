using System;
using System.IO;
using System.Globalization;

using Weborb;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
	public class NumberWriter : AbstractUnreferenceableTypeWriter
	{
		#region ITypeWriter Members

    public override void write( object obj, IProtocolFormatter writer )
    {
      //If obj is float Convert.ToDouble( obj ) adds extra numbers
      double d = obj is float ? double.Parse( obj.ToString() ) : Convert.ToDouble( obj );
      writer.WriteDouble(d);
    }

	  #endregion
	}
}
