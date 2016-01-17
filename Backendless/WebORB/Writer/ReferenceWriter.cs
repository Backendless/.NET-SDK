using System;
using Weborb.Util;
using Weborb.Util.Logging;
namespace Weborb.Writer
{
	public class ReferenceWriter : AbstractUnreferenceableTypeWriter
	{
		#region ITypeWriter Members

		public override void write( object obj, IProtocolFormatter writer )
		{
            ReferenceCache cache = writer.GetReferenceCache();
            ushort refId = (ushort) cache.GetId( obj );

            if( StringUtil.IsString( obj ) )
            {
                //Log.startLogging( "REFCACHE" );
                //Log.log( "REFCACHE", refId + "\t" + obj );
                writer.WriteStringReference( refId );
            }
            else if( obj.GetType().IsArray )
                writer.WriteArrayReference( refId );
            else
                writer.WriteObjectReference( refId );
		}

        #endregion
	}
}
