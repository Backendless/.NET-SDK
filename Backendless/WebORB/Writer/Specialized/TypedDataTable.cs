using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Weborb.Writer.Specialized
{
    internal class TypedDataTable
    {
        internal DataTable dataTable;
        internal String clientType;

        public TypedDataTable( DataTable dataTable, String clientType )
        {
            this.dataTable = dataTable;
            this.clientType = clientType;
        }
    }
}
