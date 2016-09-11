using System;
using System.Collections.Generic;
using System.Text;

using Weborb.Config;
using Weborb.Message;
using Weborb.Reader;
using Weborb.Util;
#if (!UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
using Weborb.V3Types.Core;
#endif
using Weborb.Util.Logging;
using Weborb.Types;
using System.Collections;

namespace Weborb.V3Types
{
#if (FULL_BUILD)
    [Serializable()]
#endif
    public class AsyncMessage : V3Message
    {
#if (FULL_BUILD)
        public override V3Message execute( Request message, RequestContext context )
        {
            String dsId = (String)this.headers["DSId"];
            String woId = (String)this.headers["WebORBClientId"];
            IDestination destObj = ORBConfig.GetInstance().GetDataServices().GetDestinationManager().GetDestination(destination);

            if (Log.isLogging(LoggingConstants.INFO))
                Log.log(LoggingConstants.INFO, "Delivering message to destination " + destination);

            if (messageId == null)
                messageId = Guid.NewGuid().ToString();

            if (destObj == null)
            {
                String error = "Unknown destination - " + destination + ". Make sure the destination is properly configured.";

                if (Log.isLogging(LoggingConstants.ERROR))
                    Log.log(LoggingConstants.ERROR, error);

                return new ErrMessage(messageId, new Exception(error));
            }

            Object[] bodyParts = (Object[])this.body.body;

            if( bodyParts != null && bodyParts.Length > 0 )
            {
              for (int i = 0; i < bodyParts.Length; i++)
              {
                if (bodyParts[i] is IAdaptingType )
                {
                  bodyParts[i] = ((IAdaptingType) bodyParts[i]).defaultAdapt();
                }
                else if (bodyParts[i].GetType().IsArray)
                {
                  Object[] arrayPart = (Object[]) bodyParts[i];

                  for (int j = 0; j < arrayPart.Length; j++)
                  {
                    if (arrayPart[j] is IAdaptingType )
                    arrayPart[j] = ((IAdaptingType) arrayPart[j]).defaultAdapt();
                  }
                }
              }
              destObj.messagePublished(woId, bodyParts[0]);
              destObj.GetServiceHandler().AddMessage( (Hashtable)this.headers, this );
            }

            return new AckMessage(messageId, clientId, null, new Hashtable());
        }
#else
        public override V3Message execute(Request message, RequestContext context)
        {
            return null;
        }
        // only for client usage, for internal usage use _body
        public new object body
        {
          set { base.body = new BodyHolder { body = value }; }
          get { return base.body.body; }
        }
#endif
      public IAdaptingType[] GetBody()
      {
        if(_body.body is ArrayType)
        {
          object[] array = (object[])((ArrayType) _body.body).getArray();
          List<IAdaptingType> adaptingTypes = new List<IAdaptingType>();
          
          foreach (object o in array)
          {
            adaptingTypes.Add(o as IAdaptingType);
          }
          return adaptingTypes.ToArray();
        }

        if ( _body.body.GetType().IsArray )
        {
          List<IAdaptingType> adaptingTypes = new List<IAdaptingType>();

          foreach ( object o in (object[])_body.body )
          {
            adaptingTypes.Add( (IAdaptingType)o );
          }
          return adaptingTypes.ToArray();
        }
        
        if(_body.body is IAdaptingType)
        {
          return new IAdaptingType[] { (IAdaptingType)_body.body };
        }

        return null;
      }
    }
}
