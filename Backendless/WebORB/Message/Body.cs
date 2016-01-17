using System;

using Weborb.Types;
using Weborb.Reader;

namespace Weborb.Message
{
	public class Body
	{
		internal string serviceURI;
        internal string responseURI;
        internal object dataObject;
        internal object responseDataObject;

    public Body()
    {      
    }

		public Body( string serviceURI, string responseURI, int length, object dataObject )
		{
			this.serviceURI = serviceURI;
			this.responseURI = responseURI;

			if( dataObject is IAdaptingType )
			{
				IAdaptingType adaptingType = (IAdaptingType) dataObject;

				if( adaptingType is ArrayType )
					this.dataObject = ((ArrayType) adaptingType).getArray();
				else
					this.dataObject = new Object[] { dataObject };
			}
			else
			{
				this.dataObject = dataObject;
			}
		}

		public string ServiceUri
		{
			get
			{
				return serviceURI != null ? serviceURI : "null";
			}
		}

		public string ResponseUri
		{
			get
			{
				return responseURI != null ? responseURI : "null";
			}
		}

	  public object DataObject
	  {
	    get
	    {
	      return dataObject;
	    }
	    set
	    {
	      dataObject = value;
	    }
	  }

	  public object ResponseDataObject
        {
            get
            {
                return responseDataObject;
            }
        }
	}
}
