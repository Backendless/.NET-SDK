using System;
using System.IO;
using System.Collections;

using Weborb;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Reader;

namespace Weborb.Writer
{
	/// <summary>
	/// 
	/// </summary>
	public class RemoteReferenceWriter : AbstractReferenceableTypeWriter
	{
		#region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
		{
			RemoteReferenceObject remote = (RemoteReferenceObject) obj;
            //writer.BeginWriteNamedObject( "NetServiceProxy" );
	
			Hashtable props = new Hashtable();
			props.Add( "_namedType", "NetServiceProxy" );
			props.Add( "nc", null );
			props.Add( "serviceName", remote.getServiceID() );
			props.Add( "client", null );
			//base.writeProperties( props, writer );
            //writer.EndWriteNamedObject();
			writer.GetObjectSerializer().WriteObject( null, props, writer );
		}

		#endregion
	}
}
