using System;
using System.Reflection;
using System.Collections;
using System.Data;

using Weborb.Util;
using Weborb.Types;
using Weborb.Writer;
using Weborb.Writer.Specialized;

namespace Weborb.Service
{
    public class ReturnTypeAttribute : Attribute, IWebORBAttribute
	{
        private string clientType;
        private string serverType;

		public ReturnTypeAttribute( string clientType )
		{
            this.clientType = clientType;
		}

        public string GetClientType()
        {
            return clientType;
        }
        
        #region IWebORBAttribute Members

        public void HandlePreInvoke( MethodInfo method, object obj, object[] arguments )
        {            
        }

        public Hashtable HandlePostInvoke( MethodInfo method, object obj, object[] arguments, ref object returnType, bool isException )
        {
            if( !isException )
            {
                if( returnType != null )
                {
                    if (returnType is DataSet)
                        returnType = new TypedDataSet((DataSet)returnType, clientType);
                    else if (returnType is DataTable)
                        returnType = new TypedDataTable((DataTable)returnType, clientType);
                    else if (returnType is IDictionary)
                        returnType = new TypedDictionary((IDictionary)returnType, clientType);
                }
            }

            return null;
        }

        #endregion
    }
}
