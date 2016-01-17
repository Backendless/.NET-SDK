using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Weborb.V3Types.Core
{
    public class AbstractDestination : IDestination
    {
        private List<IMessageEventListener> listeners = new List<IMessageEventListener>();
        public Hashtable properties;
        public IServiceHandler serviceHandler;
        public String name;

        public void SetName( String name )
        {
            this.name = name;
        }

        public String GetName()
        {
            return name;
        }

        public void SetProperties( Hashtable properties )
        {
            this.properties = properties;
        }

        public String GetProperty( String name )
        {
            if (properties == null)
                return null;

            Hashtable props = properties;

            while ( true )
            {
              if ( props == null )
                return null;

              int index = name.IndexOf("/");

              if (index == -1)
                return props[name] == null ? null : (String) props[name];

              String propName = name.Substring(0, index);
              name = name.Substring(index + 1);
              props = (Hashtable) props[propName];
            }
        }

        public IServiceHandler GetServiceHandler()
        {
            return serviceHandler;
        }

        public void SetServiceHandler( IServiceHandler serviceHandler )
        {
            this.serviceHandler = serviceHandler;
        }

        public virtual bool SetConfigServiceHandler()
        {
            this.serviceHandler = null;
            return true;
        }

        public void messagePublished( String senderId, Object message )
          {
          foreach( IMessageEventListener messageEventListener in listeners )
            {
            messageEventListener.messageReceived( senderId, message );
            }
          }

        public void addMessageEventListener( IMessageEventListener listener )
          {
          listeners.Add( listener );
          }

        public void removeMessageEventListener( IMessageEventListener listener )
          {
          listeners.Remove( listener );
          }

        public void messageDelivered( Object message )
          {
          foreach ( IMessageEventListener messageEventListener in listeners )
            {
            messageEventListener.messageSend( message );
            }
          }
    }
}
