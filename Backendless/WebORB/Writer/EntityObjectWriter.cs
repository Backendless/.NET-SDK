#if NET_35 || NET_40
using System;
using System.Collections;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using Weborb.Util.Logging;
using System.Data;
using System.Collections.Generic;

namespace Weborb.Writer
{
  class EntityObjectWriter : ObjectWriter
  {

    protected override void onWriteObject( object obj, string className, IDictionary objectFields, IProtocolFormatter writer )
    {
      EntityObject entityObject = (EntityObject) obj;
      handleEntity( entityObject, objectFields, writer );

      objectFields.Remove( "EntityKey" );
      objectFields.Remove( "EntityKeyPropertyName" );
      objectFields.Remove( "EntityState" );

      if( !objectFields.Contains( "References" ) )
        objectFields[ "References" ] = null;

      base.onWriteObject( obj, className, objectFields, writer );
    }

    private void handleEntity( EntityObject entityObject, IDictionary objectFields, IProtocolFormatter writer )
    {
      PropertyInfo[] properties = entityObject.GetType().GetProperties( BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance );

      foreach( PropertyInfo prop in properties )
      {
        object[] dataMemberAttributes = prop.GetCustomAttributes( typeof( DataMemberAttribute ), false );

        object propValue = prop.GetValue( entityObject, null );

        if( propValue is EntityObject )
        {
          objectFields[ prop.Name ] = propValue;
          continue;
        }

        if( propValue == null )
        {
          objectFields[ prop.Name ] = propValue;
          continue;
        }

        if( typeof( EntityReference ).IsAssignableFrom( propValue.GetType() ) )
        {

          EntityReference reference = (EntityReference) propValue;

          if( reference.EntityKey != null )
          {
            EntityKeyMember[] keyValues = reference.EntityKey.EntityKeyValues;

            if( !objectFields.Contains( "References" ) )
              objectFields[ "References" ] = new Hashtable();

            Hashtable references = (Hashtable) objectFields[ "References" ];

            foreach( EntityKeyMember keyValue in keyValues )
              references[ reference.TargetRoleName + "_" + keyValue.Key ] = keyValue.Value;
          }

          objectFields.Remove( prop.Name );

          continue;
        }

        if( typeof( IRelatedEnd ).IsAssignableFrom( propValue.GetType() ) )
        {
          // check if lazy loading is enabled
#if NET_40
          IRelatedEnd relatedEnd = (IRelatedEnd) propValue;

          ObjectQuery objectQuery = (ObjectQuery)relatedEnd.CreateSourceQuery();
          if (objectQuery != null && objectQuery.Context.ContextOptions.LazyLoadingEnabled)
          {
            objectFields.Remove( prop.Name );
            continue;
          }
#endif
        }

        objectFields[ prop.Name ] = propValue;
      }
    }
  }
}
#endif