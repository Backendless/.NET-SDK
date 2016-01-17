using System;
using System.IO;
using System.Text;
using System.Collections;

using Weborb;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
	public class BoundPropertyBagWriter : AbstractReferenceableTypeWriter
	{
		#region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
		{
            //IDictionary propertyBag = (IDictionary) obj;
            IDictionary props = (IDictionary) obj;
            string clientClass = GetClientClass(props);
#if FULL_BUILD
            props = new SortedList(props);
#endif
            writer.GetObjectSerializer().WriteObject( clientClass, props, writer );
		}

        protected string GetClientClass( IDictionary dictionary )
        {
            Type type = dictionary.GetType();
            string className = type.IsGenericType && type.FullName != null
                             ? type.FullName.Substring( 0, type.FullName.IndexOf( "`" ) )
                             : type.FullName;
#if (FULL_BUILD)
            string clientClass = null;
            string mappingClassName = ORBConstants.CLIENT_MAPPING + className;
            IDictionary props = ThreadContext.getProperties();

            if( props.Contains( className ) )
                clientClass = (string) props[ mappingClassName ];
#else
            string clientClass = null;
#endif
            if( clientClass == null )
            {
                clientClass = Types.Types.getClientClassForServerType( className );

                if( clientClass == null && type.IsGenericType )
                {
                    Type keyType = type.GetGenericArguments()[ 0 ];

                    if( !keyType.IsPrimitive && !StringUtil.IsStringType( keyType ) )
                        clientClass = "flash.utils.Dictionary";
                }

                return clientClass;
            }
            else
            {
                return clientClass;
            }
        }

		#endregion
	}
}
