using System;

namespace Weborb.Util
{
	public class Value
	{
		private object obj;
		private string author;

		public Value()
		{
		}

		public Value( object Object )
		{
			this.Object = Object;
		}

		public virtual object Object
		{
			get
			{
				return obj;
			}
			set
			{
				obj = value;
			}
		}

		public void setAuthor( string author )
		{
			this.author = author;
		}

		public string getAuthor()
		{
			return this.author;
		}
	}
}
