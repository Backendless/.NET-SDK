using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Weborb;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Types;

namespace Weborb.Reader
  {
  public class ArrayType : ICacheableAdaptingType
    {
    //private static Type listType = typeof( IList );
    //private static Type sortedListType = typeof( SortedList );
    //private static Type queueType = typeof( Queue );
    //private static Type stackType = typeof( Stack );
    //private static Type objectType = typeof( object );
    private object[] arrayObject;

    private Guid arrayTypeGuid;

    public ArrayType( object[] arrayObject )
      {
      this.arrayObject = arrayObject;
      this.arrayTypeGuid = Guid.NewGuid();
      }

    #region IAdaptingType Members

    public bool IsAdapting { get; set; }

    public Type getDefaultType()
      {
      return typeof( object );
      }

    public virtual object defaultAdapt()
      {
      return defaultAdapt( new ReferenceCache() );
      }

    public object defaultAdapt( ReferenceCache refCache )
      {

      if ( refCache.HasObject( this ) )
        return refCache.GetObject( this );

      int size = arrayObject.Length;

      if ( isHomogeneous() )
        {
        Array array = Array.CreateInstance( getComponentType(), size );
        //refCache[ this ] = array;

        refCache.AddObject( this, array );
        for ( int i = 0; i < size; i++ )
          {
          object obj = arrayObject[ i ];

          if ( obj != null )
            {
            if ( obj is IAdaptingType && refCache.HasObject( (IAdaptingType)obj ) )
              {
              obj = refCache.GetObject( (IAdaptingType)obj );
              }
            else if ( obj is ICacheableAdaptingType )
              {
              object value = ( (ICacheableAdaptingType)obj ).defaultAdapt( refCache );
              //refCache[ obj ] = value;
              refCache.AddObject( (IAdaptingType)obj, value );
              obj = value;
              }
            else if ( obj is IAdaptingType )
              {
              obj = ( (IAdaptingType)obj ).defaultAdapt();
              }
            }

          array.SetValue( obj, i );
          }

        return array;
        }
      else
        {
        object[] array = new object[ size ];
        //refCache[ this ] = array;

        refCache.AddObject( this, array );

        for ( int i = 0; i < size; i++ )
          {
          object obj = arrayObject[ i ];

          if ( obj != null )
            {
            if ( obj is IAdaptingType && refCache.HasObject( (IAdaptingType)obj ) )
              {
              //obj = refCache[ obj ];

              obj = refCache.GetObject( (IAdaptingType)obj );
              }
            else if ( obj is ICacheableAdaptingType )
              {
              object val = ( (ICacheableAdaptingType)obj ).defaultAdapt();
              //refCache[ obj ] = val;

              refCache.AddObject( (IAdaptingType)obj, val );

              obj = val;
              }
            else if ( obj is IAdaptingType )
              {
              obj = ( (IAdaptingType)obj ).defaultAdapt();
              }
            }

          array[ i ] = obj;
          }

        return array;
        }
      }

    public object adapt( Type type )
      {
      return adapt( type, new ReferenceCache() );
      }

    public object adapt( Type type, ReferenceCache refCache )
      {
      if ( refCache.HasObject( this, type ) )
        {
        /*object obj = refCache[ this ];

        if( type.IsAssignableFrom( obj.GetType() ) )
            return obj;*/

        return refCache.GetObject( this, type );
        }

      if ( Log.isLogging( LoggingConstants.DEBUG ) )
        Log.log( LoggingConstants.DEBUG, "ArrayType.adapt, adapting type: " + type.FullName );

      int size = arrayObject.Length;

      if ( type.Equals( typeof( IAdaptingType ) ) )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "type is an adapting type" );

        return this;
        }
      else if ( type.IsArray )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "type is array" );

        Type componentType = type.GetElementType();

        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          {
          if ( componentType != null )
            Log.log( LoggingConstants.DEBUG, "array element type is " + componentType.FullName );
          else
            Log.log( LoggingConstants.DEBUG, "array element type is null" );
          }

        Array newArray = Array.CreateInstance( componentType, size );
        //refCache[ this ] = newArray;

        refCache.AddObject( this, type, newArray );

        for ( int i = 0; i < size; i++ )
          newArray.SetValue( adaptArrayComponent( arrayObject[ i ], componentType, refCache ), i );

        return newArray;
        }
      else if ( type.IsGenericType && type.GetGenericArguments().Length == 2 )
        {
        Type constructedType = Types.Types.GetAbstractClassMapping( type );

        Type genericTypeDef = type.GetGenericTypeDefinition();
        Type[] genericArgs = type.GetGenericArguments();

        if ( constructedType == null )
          constructedType = genericTypeDef.MakeGenericType( genericArgs );

        object newDictionary = Activator.CreateInstance( constructedType );
        refCache.AddObject( this, type, newDictionary );
        Type keyValuePairType = typeof( KeyValuePair );
        MethodInfo addMethod = constructedType.GetMethod( "Add", genericArgs );
        object[] args = new object[ 2 ];

        for ( int i = 0; i < arrayObject.Length; i++ )
          {
          KeyValuePair kvPair = (KeyValuePair)( (IAdaptingType)arrayObject[ i ] ).adapt( keyValuePairType );

          args[ 0 ] = kvPair.key.adapt( genericArgs[ 0 ] );
          args[ 1 ] = kvPair.value.adapt( genericArgs[ 1 ] );

          addMethod.Invoke( newDictionary, args );
          }

        return newDictionary;
        }
      else if ( type.GetInterface( "ICollection`1", true ) != null )
        {
        Type genericCollection = type.GetInterface( "ICollection`1", true );
        Type elementType = genericCollection.GetGenericArguments()[ 0 ];
        object collection = ObjectFactories.CreateServiceObject( type );

        if( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "created service object which is a collection" );

        refCache.AddObject( this, type, collection );
        MethodInfo addMethod = type.GetMethod( "Add", new Type[] { elementType } );

        if ( addMethod == null )
          addMethod = collection.GetType().GetMethod( "Add", new Type[] { elementType } );

        object[] args = new object[ 1 ];

        for ( int i = 0; i < arrayObject.Length; i++ )
          {
          //args[ 0 ] = ((IAdaptingType) arrayObject[ i ]).adapt( elementType );
          args[ 0 ] = adaptArrayComponent( arrayObject[ i ], elementType, refCache );

          if( Log.isLogging( LoggingConstants.DEBUG ) )
            Log.log( LoggingConstants.DEBUG, "invoking method Add to add item to collection" );

          addMethod.Invoke( collection, args );

          if( Log.isLogging( LoggingConstants.DEBUG ) )
            Log.log( LoggingConstants.DEBUG, "item has been added" );
          }

        return collection;
        }
      else if ( typeof( IList ).IsAssignableFrom( type ) )
        {
#if FULL_BUILD
        if(!(type is WebORBArrayCollection) && this is ArrayCollectionType)
        {
          return ((ArrayCollectionType) this).GetArrayType().adapt(type);
        }
#endif
        IList listObject = (IList)ObjectFactories.CreateServiceObject( type );
        //refCache[ this ] = listObject;

        refCache.AddObject( this, type, listObject );

        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "will populate service objects with elements. got " + size + " items" );

        Type componentType = null;

        if ( type.IsGenericType )
          componentType = type.GetGenericArguments()[ 0 ];

        for ( int i = 0; i < size; i++ )
          {
          if ( Log.isLogging( LoggingConstants.DEBUG ) )
            Log.log( LoggingConstants.DEBUG, "element " + i + "   " + arrayObject[ i ] );

          listObject.Add( adaptArrayComponent( arrayObject[ i ], componentType, refCache ) );
          }

        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "returning list. list size is " + listObject.Count );

        return listObject;
        }
#if (FULL_BUILD)
      else if ( typeof( Queue ).IsAssignableFrom( type ) )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "type is Queue" );

        Queue qObject = (Queue)ObjectFactories.CreateServiceObject( type );
        //refCache[ this ] = qObject;

        refCache.AddObject( this, type, qObject );

        for ( int i = 0; i < size; i++ )
          qObject.Enqueue( adaptArrayComponent( arrayObject[ i ], null, refCache ) );

        return qObject;
        }
      else if ( typeof( Stack ).IsAssignableFrom( type ) )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "type is Stack" );

        Stack stackObject = (Stack)ObjectFactories.CreateServiceObject( type );
        //refCache[ this ] = stackObject;

        refCache.AddObject( this, type, stackObject );

        for ( int i = 0; i < size; i++ )
          stackObject.Push( adaptArrayComponent( arrayObject[ i ], null, refCache ) );

        return stackObject;
        }
      else if ( typeof( SortedList ).IsAssignableFrom( type ) )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "type is SortedList" );

        SortedList sortedList = (SortedList)ObjectFactories.CreateServiceObject( type );
        //refCache[ this ] = sortedList;

        refCache.AddObject( this, type, sortedList );

        for ( int i = 0; i < size; i++ )
          {
          object obj = adaptArrayComponent( arrayObject[ i ], null, refCache );
          sortedList.Add( obj, obj );
          }

        return sortedList;
        }
#endif
      else if ( type.GetInterface( "Iesi.Collections.ISet", true ) != null || type.FullName.Equals( "Iesi.Collections.ISet" ) )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "type is Iesi.Collections.ISet" );

        Object iSet = ObjectFactories.CreateServiceObject( type );

        refCache.AddObject( this, type, iSet );
        MethodInfo addMethod = iSet.GetType().GetMethod( "Add", new Type[] { typeof( Object ) } );
        Object[] args = new Object[ 1 ];

        for ( int i = 0; i < size; i++ )
          {
          args[ 0 ] = adaptArrayComponent( arrayObject[ i ], null, refCache );
          addMethod.Invoke( iSet, args );
          }

        return iSet;
        }
#if (FULL_BUILD)
      // this is a hack for Flash Builder 4
      else if ( typeof( IDictionary ).IsAssignableFrom( type ) && ( arrayObject == null || arrayObject.Length == 0 ) )
        {
        return new Hashtable();
        }
#endif
      else
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "will do default adaptation" );

        return defaultAdapt( refCache );
        }
      }

    public IAdaptingType getCacheKey()
    {
      return this;
    }

    public bool canAdaptTo( Type formalArg )
      {
      return typeof( IAdaptingType ).IsAssignableFrom( formalArg ) ||
      typeof( IList ).IsAssignableFrom( formalArg ) ||
      formalArg.IsArray ||
#if (FULL_BUILD)
 typeof( Queue ).IsAssignableFrom( formalArg ) ||
      typeof( Stack ).IsAssignableFrom( formalArg ) ||
      typeof( SortedList ).IsAssignableFrom( formalArg ) ||
#endif
 ( formalArg.IsGenericType && canAdaptToGeneric( formalArg ) ) ||
      formalArg.FullName.Equals( "Iesi.Collections.ISet" ) ||
      formalArg.GetInterface( "Iesi.Collections.ISet", true ) != null;
      }

    #endregion

    private bool canAdaptToGeneric( Type formalArg )
      {
      Type[] args = formalArg.GetGenericArguments();

      if ( args.Length == 1 )
        return true;

      return args.Length == 2 &&
              ( arrayObject == null ||
               arrayObject.Length == 0 ||
              ( (IAdaptingType)arrayObject[ 0 ] ).canAdaptTo( typeof( KeyValuePair ) ) );
      }

    private bool isHomogeneous()
      {
      int size = arrayObject.Length;

      if ( size == 0 )
        return true;

      Type componentType = arrayObject[ 0 ].GetType();

      for ( int i = 0; i < size; i++ )
        {
        object obj = arrayObject[ i ];

        if ( !componentType.Equals( obj.GetType() ) )
          return false;
        }

      return true;
      }

    // valid only for the homogeneous arrays
    private Type getComponentType()
      {
      if ( arrayObject.Length == 0 )
        {
        return getDefaultType();
        }
      else
        {
        object obj = arrayObject[ 0 ];

        if ( typeof( IAdaptingType ).IsAssignableFrom( obj.GetType() ) )
          return ( (IAdaptingType)obj ).getDefaultType();
        else
          return obj.GetType();
        }
      }

    private object adaptArrayComponent( object obj, Type componentType, ReferenceCache refCache )
      {
      if ( Log.isLogging( LoggingConstants.DEBUG ) )
        {
        if ( componentType == null )
          Log.log( LoggingConstants.DEBUG, "adapting array component. Component type is unknown, will use default type adaptation" );
        else
          Log.log( LoggingConstants.DEBUG, "adapting array component. Component type is " + componentType.FullName );
        }

      object result;

      if ( obj is ICacheableAdaptingType )
        {
        if ( componentType == null )
          result = ( (ICacheableAdaptingType)obj ).defaultAdapt( refCache );
        else
          result = ( (ICacheableAdaptingType)obj ).adapt( componentType, refCache );
        }
      else if ( obj is IAdaptingType )
        {
        if ( componentType == null )
          result = ( (IAdaptingType)obj ).defaultAdapt();
        else
          result = ( (IAdaptingType)obj ).adapt( componentType );
        }
      else
        {
        //Vector support
        try
          {
#if FULL_BUILD
          result = Convert.ChangeType( obj, componentType );
#else
        result = Convert.ChangeType( obj, componentType, null );
#endif
          }
        catch ( Exception e )
          {
          throw new Exception( "array element is not adapting type", e );
          }
        }

      if ( Log.isLogging( LoggingConstants.DEBUG ) )
        {
        if ( result != null )
          {
          Log.log( LoggingConstants.DEBUG, "array component has been adapted to type " + result.GetType().FullName );
          Log.log( LoggingConstants.DEBUG, "array component value is " + result );
          }
        else
          {
          Log.log( LoggingConstants.DEBUG, "array component has been adapted to null value" );
          }
        }

      return result;
      }

    public object getArray()
      {
      return arrayObject;
      }

    public override string ToString()
      {
      StringBuilder strBuilder = new StringBuilder();
      strBuilder.Append( "Array type. Size - " ).Append( arrayObject.Length ).Append( ". Elements:\n" );

      for ( int i = 0; i < arrayObject.Length; i++ )
        strBuilder.Append( "[" ).Append( i ).Append( "] - " ).Append( arrayObject[ i ] );

      return strBuilder.ToString();
      }

    public override bool Equals( object _obj )
      {
      return Equals( _obj, new Dictionary<DictionaryEntry, bool>() );
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry,bool> visitedPairs )
      {
      ArrayType obj = _obj as ArrayType;

      if ( obj == null )
        return false;

      if ( Object.ReferenceEquals( this, _obj ) )
        return true;

      return this.arrayTypeGuid.Equals(obj.arrayTypeGuid);
        /*
      if ( arrayObject.Length != obj.arrayObject.Length )
        return false;

      for ( int i = 0; i < arrayObject.Length; i++ )
        if( !((IAdaptingType)arrayObject[ i ]).Equals( obj.arrayObject[ i ], visitedPairs ) )
          return false;
      
      return true; */
      }

    public override int GetHashCode()
      {
      return arrayObject.Length;

      //return arrayObject[ 0 ].GetHashCode();
      }
    }
  }