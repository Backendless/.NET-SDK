using System;
using System.Collections;
using System.Text;
using System.Data;

using Weborb.Writer.Specialized;

namespace Weborb.Writer
{
    public class DataTableAsListWriter : AbstractUnreferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            DataTable table = (DataTable)obj;
            DataColumnCollection columns = table.Columns;
            ArrayList list = new ArrayList();

            foreach( DataRow row in table.Rows )
            {
                Hashtable rowObject = new Hashtable();

                foreach( DataColumn column in columns )
                {
                    Object dbObject = row[ column ];

                    if( dbObject is DBNull )
                        dbObject = null;

                    rowObject[ column.ColumnName ] = dbObject;
                }

                list.Add( rowObject );
            }

            MessageWriter.writeObject( list.ToArray(), formatter );
        }

        #endregion
    }
}
