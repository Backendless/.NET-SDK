using System;
using System.Data;

namespace Weborb.Writer
{
	public class DataTableWriter : AbstractUnreferenceableTypeWriter
	{
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
        {
            DataTable table = (DataTable) obj;
            DataSet dataSet = new DataSet();

			if( table.DataSet != null )
				dataSet.Tables.Add( table.Copy() );
			else
				dataSet.Tables.Add( table );

            MessageWriter.writeObject( dataSet, writer );
        }

        #endregion
    }
}
