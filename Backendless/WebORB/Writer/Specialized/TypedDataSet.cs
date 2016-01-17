using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Weborb.Writer.Specialized
{
    internal class TypedDataSet
    {
        internal DataSet dataSet;
        internal String clientType;

        public TypedDataSet( DataSet dataSet, String clientType )
        {
            this.dataSet = dataSet;
            this.clientType = clientType;
        }
    }
}
