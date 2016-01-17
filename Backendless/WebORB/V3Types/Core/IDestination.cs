using System;
using System.Collections;
using System.Text;

namespace Weborb.V3Types.Core
{
    public interface IDestination
    {
        String GetName();
        void SetName( String name );
        IServiceHandler GetServiceHandler();
        void SetProperties( Hashtable properties );
        String GetProperty( String name );
        void SetServiceHandler(IServiceHandler serviceHandler);
        bool SetConfigServiceHandler();
        void messagePublished( String senderId, Object message );
        void messageDelivered( Object message );
        void addMessageEventListener( IMessageEventListener listener );
        void removeMessageEventListener( IMessageEventListener listener );
    }
}
