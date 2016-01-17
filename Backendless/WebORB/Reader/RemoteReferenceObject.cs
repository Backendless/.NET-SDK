using System;
using System.Collections;
using System.Web.SessionState;
using Weborb.Util;
using Weborb.Types;
using System.Collections.Generic;

namespace Weborb.Reader
  {
  /// <summary>
  /// 
  /// </summary>
  public class RemoteReferenceObject : IAdaptingType
    {
    private object reference;

    public RemoteReferenceObject( object reference )
      {
      this.reference = reference;
      }

    public static RemoteReferenceObject createReference( object referencedObject )
      {
      string referenceId = referencedObject.GetType().FullName + "_proxy_" + Guid.NewGuid().ToString();
      RemoteReferenceObject proxy = new RemoteReferenceObject( referenceId );
      HttpSessionState httpSession = (HttpSessionState)ThreadContext.currentSession();
      httpSession.Add( referenceId, referencedObject );
      return proxy;
      }

    #region IAdaptingType Members

    public Type getDefaultType()
      {
      throw new ApplicationException( "TBD" );
      }

    public object defaultAdapt()
      {
      return reference;
      }

    public object adapt( Type type )
      {
      return ObjectFactories.CreateServiceObject( type );
      }

    public bool canAdaptTo( Type formalArg )
      {
      return !formalArg.IsArray && !formalArg.IsValueType;
      }

    #endregion

    public object getServiceID()
      {
      return reference;
      }

    public override bool Equals( object _obj )
      {
      return Equals( _obj, new Dictionary<DictionaryEntry, bool>() );
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      RemoteReferenceObject obj = _obj as RemoteReferenceObject;

      if ( obj == null )
        return false;

      if ( Object.ReferenceEquals( this, _obj ) )
        return true;

      if ( obj.reference is IAdaptingType )
        return ((IAdaptingType)obj.reference).Equals( reference, visitedPairs );
      else
        return obj.reference.Equals( reference );
      }

    public override int GetHashCode()
      {
      return reference.GetHashCode();
      }
    }
  }
