using System;
using System.Collections;
using System.Reflection;

using Weborb.Util;
using Weborb.Util.Logging;

using NHibernate;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Metadata;

namespace Weborb.Writer
  {
  class Hibernate2ProxyWriter : AbstractReferenceableTypeWriter
    {
    #region ITypeWriter Members

    public override void write( object obj, IProtocolFormatter formatter )
      {
      INHibernateProxy proxy = obj as INHibernateProxy;
      ILazyInitializer initializer = proxy.HibernateLazyInitializer;      

      Type persistentClass = initializer.PersistentClass;
      ISessionImplementor sessionImplementor = initializer.Session;
      IEntityPersister entityPersister = sessionImplementor.Factory.GetEntityPersister( initializer.EntityName );
      IClassMetadata metadata = entityPersister.ClassMetadata;
      string idProp = metadata.IdentifierPropertyName;
      string[] propNames = metadata.PropertyNames;
      string className = persistentClass.FullName;

      Hashtable objectFields = new Hashtable();

      objectFields[ idProp ] = metadata.GetIdentifier( obj, EntityMode.Poco );

      if( !initializer.IsUninitialized )
        foreach( string propName in propNames )
          objectFields[ propName ] = metadata.GetPropertyValue( obj, propName, EntityMode.Poco );      

      formatter.GetObjectSerializer().WriteObject( className, objectFields, formatter );
      }

    #endregion
    }
  }
