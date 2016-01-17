using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    // this class is for older version of NHibernate
    //class HibernateProxyWriter : AbstractReferenceableTypeWriter
    //{
    //    #region ITypeWriter Members

    //    public override void write( object obj, IProtocolFormatter formatter )
    //      {
    //      LazyInitializer initializer = NHibernateProxyHelper.GetLazyInitializer( (INHibernateProxy)obj );
    //      Type persistentClass = initializer.PersistentClass;
    //      ISessionImplementor sessionImplementor = initializer.Session;
    //      IEntityPersister entityPersister = sessionImplementor.Factory.GetEntityPersister( persistentClass );
    //      IClassMetadata metadata = entityPersister.ClassMetadata;
    //      string idProp = metadata.IdentifierPropertyName;
    //      string[] propNames = metadata.PropertyNames;
    //      string className = persistentClass.FullName;

    //      Hashtable objectFields = new Hashtable();

    //      objectFields[ idProp ] = metadata.GetIdentifier( obj );

    //      foreach( string propName in propNames )
    //        objectFields[ propName ] = metadata.GetPropertyValue( obj, propName );

    //      formatter.GetObjectSerializer().WriteObject( className, objectFields, formatter );
    //      }

    //  #endregion
    //}
}
