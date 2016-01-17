using System;
using System.Reflection;
using System.Data.SqlTypes;

namespace Weborb.Writer
{
	public class GenericSqlTypeHandler : AbstractUnreferenceableTypeWriter
	{
		#region ITypeWriter Members

		public override void write(object obj, IProtocolFormatter writer)
		{
            if (obj is INullable && ((INullable)obj).IsNull )
            {
                MessageWriter.writeObject( null, writer);
                return;
            }

			Type type = obj.GetType();
			FieldInfo field = type.GetField( "Value", BindingFlags.Public | BindingFlags.Instance );

            if (field != null)
                MessageWriter.writeObject( field.GetValue( obj ), writer);
            else
            {
                PropertyInfo prop = type.GetProperty( "Value", BindingFlags.Public | BindingFlags.Instance );
                MessageWriter.writeObject( prop.GetValue( obj, null ), writer);
            }
		}

		#endregion
	}
}
