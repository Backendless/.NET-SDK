using System;
using System.Collections;

namespace Weborb.Types
{
	public class TypedObject
	{
		private string _typeName;
		private IDictionary _dictionary;

		public TypedObject( string typeName, IDictionary dictionary )
		{
			this._typeName = typeName;
			this._dictionary = dictionary;
		}

		public string typeName
		{
			get
			{
				return _typeName;
			}
		}

		public IDictionary objectData
		{
			get
			{
				return _dictionary;
			}
		}
	}
}
