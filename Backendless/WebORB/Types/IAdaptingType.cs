using System;
using System.Collections;
using System.Collections.Generic;

namespace Weborb.Types
{
	public interface IAdaptingType
	{
		Type getDefaultType();

		object defaultAdapt();
		object adapt( Type type );

		bool canAdaptTo( Type formalArg );

    // this method is necessary to avoid cyclic references during object comparision
	  bool Equals( object obj, Dictionary<DictionaryEntry, bool> visitedPairs );
	}
}
