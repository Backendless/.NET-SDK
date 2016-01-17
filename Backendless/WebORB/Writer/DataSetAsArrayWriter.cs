using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Weborb.Writer
{
    class DataSetAsArrayWriter : AbstractUnreferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            DataSet dataSet = (DataSet) obj;

            if( dataSet.Tables.Count == 1 )
            {
                MessageWriter.writeObject( dataSet.Tables[ 0 ], formatter );
            }
            else
            {
                Hashtable dataSetData = new Hashtable();

                foreach( DataTable table in dataSet.Tables )
                    dataSetData.Add( table.TableName, table );

                MessageWriter.writeObject( dataSetData, formatter );
                //formatter.GetObjectSerializer().WriteObject( null, dataSetData, formatter );
            }
        }

        #endregion
    }
}
