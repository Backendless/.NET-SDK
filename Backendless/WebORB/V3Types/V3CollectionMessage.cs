using System;
using System.Collections;
using Weborb.Message;
using Weborb.Util;

namespace Weborb.V3Types
{
	public class V3CollectionMessage : V3Message, ICollection
	{
		private ArrayList messages = new ArrayList();

		public void AddMessage( V3Message message )
		{
			messages.Add( message );
		}
		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return messages.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return messages.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			messages.CopyTo( array, index );
		}

		public object SyncRoot
		{
			get
			{
				return messages.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return messages.GetEnumerator();
		}

		#endregion

		public override V3Message execute( Request message, RequestContext context )
		{
			throw new Exception( "this message should never be invoked" );
		}
	}
}
