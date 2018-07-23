using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Weborb.Protocols.JsonRPC
  {

  [Serializable]
  public sealed class JsonTokenClass : IObjectReference
    {
    public static readonly JsonTokenClass Null = new JsonTokenClass( "Null" );
    public static readonly JsonTokenClass Boolean = new JsonTokenClass( "Boolean", Superclass.Scalar );
    public static readonly JsonTokenClass Number = new JsonTokenClass( "Number", Superclass.Scalar );
    public static readonly JsonTokenClass String = new JsonTokenClass( "String", Superclass.Scalar );
    public static readonly JsonTokenClass Array = new JsonTokenClass( "Array" );
    public static readonly JsonTokenClass EndArray = new JsonTokenClass( "EndArray", Superclass.Terminator );
    public static readonly JsonTokenClass Object = new JsonTokenClass( "Object" );
    public static readonly JsonTokenClass EndObject = new JsonTokenClass( "EndObject", Superclass.Terminator );
    public static readonly JsonTokenClass Member = new JsonTokenClass( "Member" );
    public static readonly JsonTokenClass BOF = new JsonTokenClass( "BOF", Superclass.Terminator );
    public static readonly JsonTokenClass EOF = new JsonTokenClass( "EOF", Superclass.Terminator );

    public static readonly ICollection All = new JsonTokenClass[] { BOF, EOF, Null, Boolean, Number, String, Array, EndArray, Object, EndObject, Member };

    private readonly string _name;
    [NonSerialized]
    private readonly Superclass _superclass;

    private enum Superclass
      {
      Unspecified,
      Scalar,
      Terminator
      }

    private JsonTokenClass( string name ) :
      this( name, Superclass.Unspecified ) { }

    private JsonTokenClass( string name, Superclass superclass )
      {
      Debug.Assert( name != null );
      Debug.Assert( name.Length > 0 );

      _name = name;
      _superclass = superclass;
      }

    public string Name
      {
      get { return _name; }
      }

    internal bool IsTerminator
      {
      get { return _superclass == Superclass.Terminator; }
      }

    internal bool IsScalar
      {
      get { return _superclass == Superclass.Scalar; }
      }

    public override int GetHashCode()
      {
      return Name.GetHashCode();
      }

    public override string ToString()
      {
      return Name;
      }

    object IObjectReference.GetRealObject( StreamingContext context )
      {
      foreach ( JsonTokenClass clazz in All )
        {
        if ( string.CompareOrdinal( clazz.Name, Name ) == 0 )
          return clazz;
        }

      throw new SerializationException( string.Format( "{0} is not a valid {1} instance.", Name, typeof( JsonTokenClass ).FullName ) );
      }
    }
  }