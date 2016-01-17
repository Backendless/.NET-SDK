using System;
using System.Collections;
using System.Reflection;
using Weborb.Util;

namespace Weborb.Service
{
    public class EnableAutoUpdateAttribute : System.Attribute, IWebORBAttribute
    {
        private IUpdateHandler handler;

        public EnableAutoUpdateAttribute( Type updateHandler )
        {
            if( !typeof( IUpdateHandler ).IsAssignableFrom( updateHandler ) )
                throw new Exception( "EnableAutoUpdate argument must be a type assignable from IUpdateHandler" );

            this.handler = (IUpdateHandler) Activator.CreateInstance( updateHandler );
        }

        #region IWebORBAttribute Members

        public void HandlePreInvoke( MethodInfo method, object obj, object[] arguments )
        {
            // TODO:  Add EnableAutoUpdateAttribute.HandlePreInvoke implementation
        }

        public Hashtable HandlePostInvoke( MethodInfo method, object obj, object[] arguments, ref object returnType, bool isException )
        {
            if( isException )
                return null;

            string id = Guid.NewGuid().ToString();
            Hashtable metadata = new Hashtable();
            metadata[ "_orbid_" ] = id;
            ThreadContext.currentHttpContext().Cache.Insert( id, new AutoUpdateObjectWrapper( handler, returnType ) );
            return metadata;
        }

        #endregion
    }
}
