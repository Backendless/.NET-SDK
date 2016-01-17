using System;
using System.Collections;
using System.Text;
using System.Data;

namespace Weborb.Writer.Specialized
{
    class TypedDataTableWriter : AbstractUnreferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            TypedDataTable typedDataTable = (TypedDataTable) obj;
            DataTable table = typedDataTable.dataTable;
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

                list.Add( new TypedDictionary( rowObject, typedDataTable.clientType ) );
            }

            MessageWriter.writeObject( list, formatter );            
        }

        #endregion
    }
}
