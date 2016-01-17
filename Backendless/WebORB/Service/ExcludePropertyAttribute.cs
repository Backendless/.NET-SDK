using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Service
{
  [AttributeUsage( AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true )]
  public class ExcludePropertyAttribute : Attribute, IPropertyExclusionAttribute
  {
    private string propertyName;

    // this constructor is for the usage when the attribute is associated directly with the property
    public ExcludePropertyAttribute()
    {
    }

    public ExcludePropertyAttribute( string propertyName )
    {
      this.propertyName = propertyName;
    }

    public bool ExcludeProperty( object obj, string propName )
    {
      if( propName.Equals( propertyName ) )
        return true;

      if( propertyName.IndexOf( ',' ) != -1 )
      {
        if( new List<String>( propertyName.Split( ',' ) ).Contains( propertyName ) )
          return true;
      }

      return false;
    }
  }
}
