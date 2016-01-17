using System;
using System.Collections;
using System.Text;
using System.Data;

namespace Weborb.Writer.Specialized
{
    class TypedDataSetWriter : AbstractReferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            TypedDataSet typedDataSet = (TypedDataSet) obj;
            DataSet dataSet = typedDataSet.dataSet;
            
            Hashtable dataSetData = new Hashtable();

            foreach( DataTable table in dataSet.Tables )
                dataSetData.Add( table.TableName, new TypedDataTable( table, typedDataSet.clientType ) );

            formatter.GetObjectSerializer().WriteObject( null, dataSetData, formatter );
        }

        #endregion
    }
}
