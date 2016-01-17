using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Weborb.Util;
using Weborb.Types;

namespace Weborb.Reader
  {
  public class NamedObject : ICacheableAdaptingType
    {
    private static Type dictionaryType = typeof( IDictionary );
    private string objectName;
    private IAdaptingType typedObject;
    private Type mappedType;

    public NamedObject( string objectName, IAdaptingType typedObject )
      {
      this.objectName = objectName;
      this.typedObject = typedObject;
      this.mappedType = Types.Types.getServerTypeForClientClass( objectName );
      }

    #region IAdaptingType Members

    public bool IsAdapting { get; set; }

    public IAdaptingType TypedObject
    {
      get
      {
        return typedObject;
      }
    }

    public Type getDefaultType()
      {
      if ( mappedType != null )
        return mappedType;
      else
        return typedObject.getDefaultType();
      }

    public object defaultAdapt()
      {
      return defaultAdapt( new ReferenceCache() );
      }

    public object defaultAdapt( ReferenceCache refCache )
      {
      if ( mappedType != null )
        {
        if ( typedObject is ICacheableAdaptingType )
          return ( (ICacheableAdaptingType)typedObject ).adapt( mappedType, refCache );
        else
          return typedObject.adapt( mappedType );
        }
      else
        {
        if ( typedObject is ICacheableAdaptingType )
          return ( (ICacheableAdaptingType)typedObject ).defaultAdapt( refCache );
        else
          return typedObject.defaultAdapt();
        }
      }

    public object adapt( Type type )
      {
      return adapt( type, new ReferenceCache() );
      }

    public object adapt( Type type, ReferenceCache refCache )
      {
      if ( mappedType != null && type.IsAssignableFrom( mappedType ) )
        {
        if ( typedObject is ICacheableAdaptingType )
          return ( (ICacheableAdaptingType)typedObject ).adapt( mappedType, refCache );
        else
          return typedObject.adapt( mappedType );
        }
      else
        {
        if ( typedObject is ICacheableAdaptingType )
          return ( (ICacheableAdaptingType)typedObject ).adapt( type, refCache );
        else
          return typedObject.adapt( type );
        }
      }

    public IAdaptingType getCacheKey()
      {
        return typedObject;
      }

    public bool canAdaptTo( Type formalArg )
      {
      if ( dictionaryType.IsAssignableFrom( formalArg ) )
        return true;
      else if ( mappedType != null )
        return ( ( ( formalArg.IsInterface || formalArg.IsAbstract ) && formalArg.IsAssignableFrom( mappedType ) ) || formalArg.IsAssignableFrom( mappedType ) );
      else if ( formalArg.Name.Equals( objectName ) )
        return true;
      else if ( typedObject.canAdaptTo( formalArg ) )
        return true;
      else
        {
        try
          {
          return TypeLoader.LoadType( objectName ).IsAssignableFrom( formalArg );
          }
        catch ( Exception )
          {
          }

        // if we got here, the type used by the client cannot be found on the server.
        // the last resort is to check if there is an argument factory
        return ObjectFactories.GetArgumentObjectFactory( formalArg.FullName ) != null;
        }
      }

    #endregion

    public override string ToString()
      {
      StringBuilder strBuilder = new StringBuilder();
      strBuilder.Append( "Typed object.\n\tObject name - " ).Append( objectName ).Append( "\n\tMapped type - " ).Append( mappedType ).Append( "\n" );
      strBuilder.Append( "\t\tValue - " + typedObject );
      return strBuilder.ToString();
      }

    public override bool Equals( object _obj )
      {
      return Equals( _obj, new Dictionary<DictionaryEntry, bool>() );
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {      
      NamedObject obj = _obj as NamedObject;

      if ( obj == null )
        return false;

      if ( Object.ReferenceEquals( this, _obj ) )
        return true;

      return objectName.Equals( obj.objectName ) && typedObject.Equals( obj.typedObject, visitedPairs );
      }

    public override int GetHashCode()
      {
      return objectName.GetHashCode() ^ typedObject.GetHashCode();
      }
    }
  }
