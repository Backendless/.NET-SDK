using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Weborb.Types;

namespace Weborb.Reader
  {
  /// <summary>
  /// 
  /// </summary>
  public class XmlType : IAdaptingType
    {
    private System.Xml.XmlDocument document;

    public XmlType( System.Xml.XmlDocument document )
      {
      this.document = document;
      }

    #region IAdaptingType Members

    public Type getDefaultType()
      {
      return typeof( System.Xml.XmlDocument );
      }

    public object defaultAdapt()
      {
      return document;
      }

    public object adapt( Type type )
      {
      if ( typeof( System.Xml.XmlDocument ).IsAssignableFrom( type ) )
        return document;
      else if ( typeof( XmlElement ).IsAssignableFrom( type ) || typeof( XmlNode ).IsAssignableFrom( type ) )
        return document.DocumentElement;
      else
        throw new ApplicationException( "unable to adapt type " + type + " to xml" );
      }

    public bool canAdaptTo( Type formalArg )
      {
      return false;
      }

    #endregion

    public override string ToString()
      {
      return "XML Type. Value - " + document.InnerText;
      }

    public override bool Equals( object _obj )
      {
      XmlType obj = _obj as XmlType;

      if ( obj == null )
        return false;

      if ( Object.ReferenceEquals( this, _obj ) )
        return true;

      return obj.document.Equals( document );
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      return Equals( _obj );
      }

    public override int GetHashCode()
      {
      return document.GetHashCode();
      }
    }
  }
