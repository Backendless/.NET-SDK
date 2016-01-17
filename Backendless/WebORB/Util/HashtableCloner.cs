using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Util
  {
  public class HashtableCloner
    {
    public static Hashtable DeepCopy( Hashtable hashtable )
	    {
		  Hashtable hashtableCopy = new Hashtable(hashtable);

	    foreach( DictionaryEntry dictionaryEntry in hashtable )
	      {	    	   	
			  if (dictionaryEntry.Value is Hashtable)
			  {
          Hashtable hashtableValue = (Hashtable)dictionaryEntry.Value;
				  hashtableCopy[dictionaryEntry.Key] = DeepCopy(hashtableValue);
			  }
			
			  if (dictionaryEntry.Value is Hashtable[])
			  {
				  Hashtable[] hashtableArray = (Hashtable[])dictionaryEntry.Value;
          Hashtable[] hashMapArray = new Hashtable[ hashtableArray.Length ];
				  for(int i = 0; i < hashtableArray.Length; i++)
				  {
					  hashMapArray[i] = DeepCopy(hashtableArray[i]);
				  }
				  hashtableCopy[dictionaryEntry.Key] = hashMapArray;
			  }
		  }
		
		  return hashtableCopy;
	    }
    }
  }
