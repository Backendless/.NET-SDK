using System;
using System.Data;
using System.IO;
using System.Collections;

using Weborb.Config;
using Weborb.Reader.Dataset;
using Weborb.Util;

namespace Weborb.Writer
{
	/// <summary>
	/// Summary description for DataSetWriter.
	/// </summary>
	public class DataSetWriter : AbstractUnreferenceableTypeWriter
	{
		#region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
		{
			DataSet dataSet = (DataSet) obj;

            //DatasetConfigPathHandler dataSetConfig = (DatasetConfigPathHandler) ThreadContext.getORBConfig().GetConfig( "weborb/datasets" );
            DatasetConfigPathHandler dataSetConfig = (DatasetConfigPathHandler) ORBConfig.GetInstance().GetConfig( "weborb/datasets" );
            bool useLegacyFormat = dataSetConfig.LegacySerialization;

            if( useLegacyFormat )
            {
                if( dataSet.Tables.Count == 1 )
                {
                    RemotingDataSet remotingDataSet = new RemotingDataSet( dataSet.Tables[ 0 ] );
                    MessageWriter.writeObject( remotingDataSet.getDataSetInfo(), writer );
                }
                else
                {
                    ArrayList arrayOfTables = new ArrayList( dataSet.Tables.Count );

                    for( int i = 0; i < dataSet.Tables.Count; i++ )
                    {
                        RemotingDataSet remotingDataSet = new RemotingDataSet( dataSet.Tables[ i ] );
                        arrayOfTables.Add( remotingDataSet.getDataSetInfo() );
                    }

                    MessageWriter.writeObject( arrayOfTables, writer );
                }
            }
            else
            {
                    Hashtable tables = new Hashtable();

                    for( int i = 0; i < dataSet.Tables.Count; i++ )
                    {
                        RemotingDataSet remotingDataSet = new RemotingDataSet( dataSet.Tables[ i ] );
                        tables.Add( dataSet.Tables[ i ].TableName, remotingDataSet.getDataSetInfo() );
                    }

                    MessageWriter.writeObject( tables, writer );
            }
		}

        #endregion		
	}
}
