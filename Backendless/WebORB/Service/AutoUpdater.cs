using System;
using System.Reflection;
using System.Collections;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Service
{
	public class AutoUpdater
	{
		public void commitAutoUpdate( string id, PropUpdate[] props )
		{
            //if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() && !NetUtils.RequestIsLocal( ThreadContext.currentRequest() ) )
			//	throw new Exception( "auto-update is disabled, this feature is available in WebORB Professional Edition" );

			AutoUpdateObjectWrapper objWrapper = (AutoUpdateObjectWrapper) ThreadContext.currentHttpContext().Cache[ id ];

			for( int i = 0; i < props.Length; i++ )
			{
				string propName = props[ i ].name;
				IAdaptingType updateData = props[ i ].update;

				if( updateData is ArrayType )
				{
					ArrayUpdateData[] arrayUpdateData = (ArrayUpdateData[]) updateData.adapt( typeof( ArrayUpdateData[] ) );
					updateArrayField( objWrapper.obj, propName, arrayUpdateData );
				}
				else
				{
					updateFieldOrProperty( objWrapper.obj, propName, updateData );
				}
			}

			if( objWrapper.handler != null )
				objWrapper.handler.ObjectChanged( objWrapper.obj );
		}

		private void updateArrayField( object obj, string propName, ArrayUpdateData[] arrayUpdateData )
		{
			MemberWrapper memberWrapper = getFieldOrProperty( obj, propName );
			IList list = (IList) memberWrapper.obj;
			Type elementType = list.GetType().GetElementType();

			for( int i = 0; i < arrayUpdateData.Length; i++ )
			{
				switch( arrayUpdateData[ i ].actionType )
				{
					case ArrayUpdateData.ADD:
						if( !list.IsFixedSize )
							list.Add( arrayUpdateData[ i ].newValue.adapt( elementType ) );
						break;

					case ArrayUpdateData.REMOVE:
						if( !list.IsFixedSize )
							list.RemoveAt( arrayUpdateData[ i ].index );						
						break;

					case ArrayUpdateData.REPLACE:
						list[ arrayUpdateData[ i ].index ] = arrayUpdateData[ i ].newValue.adapt( elementType );
						break;

					case ArrayUpdateData.REMOVEALL:
						list.Clear();
						break;
				}
			}
		}

		private void updateFieldOrProperty( object obj, string propName, IAdaptingType updateData )
		{
			MemberWrapper memberWrapper = getFieldOrProperty( obj, propName );

			if( memberWrapper == null )
			{
        if ( Log.isLogging( LoggingConstants.ERROR ) )
				  Log.log( LoggingConstants.ERROR, "unable to retrieve property holder for " + propName + ". Please report this problem to support@themidnightcoders.com" );
				return;
			}

			if( memberWrapper.memberInfo is FieldInfo )
			{
				FieldInfo field = (FieldInfo) memberWrapper.memberInfo;
				object val = updateData.adapt( field.FieldType );
				((FieldInfo) memberWrapper.memberInfo).SetValue( memberWrapper.obj, val );
			}
			else if( memberWrapper.memberInfo is PropertyInfo )
			{
				PropertyInfo property = (PropertyInfo) memberWrapper.memberInfo;
				object val = updateData.adapt( property.PropertyType );

				if( property.CanWrite )
					property.SetValue( memberWrapper.obj, val, null );
			}
			else if( memberWrapper.memberInfo is IDictionary )
			{
                ((IDictionary) memberWrapper.memberInfo)[ memberWrapper.obj ] = updateData.defaultAdapt();
			}
		}

		private MemberWrapper getFieldOrProperty( object obj, string propName )
		{
			int index = propName.IndexOf( '.' );
			string subPath = null;

			if( index != -1 )
			{
				subPath = propName.Substring( index + 1 );
				propName = propName.Substring( 0, index );
			}

			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
			PropertyInfo property = obj.GetType().GetProperty( propName, flags );

			if( property == null )
			{
				FieldInfo field = obj.GetType().GetField( propName, flags );

				if( field == null )
				{
					if( obj is IDictionary )
					{
						if( subPath != null ) 
							return getFieldOrProperty( ((IDictionary) obj)[ propName ], subPath );
						else
							return new MemberWrapper( obj, propName );						
					}
				}
				else
				{
					if( subPath != null )
						return getFieldOrProperty( field.GetValue( obj ), subPath );
					else
						return new MemberWrapper( field, field.FieldType.IsArray ? field.GetValue( obj ) : obj );
				}
			}
			else
			{
				if( subPath != null )
					return getFieldOrProperty( property.GetValue( obj, null ), subPath );
				else
					return new MemberWrapper( property, property.PropertyType.IsArray ? property.GetValue( obj, null ) : obj );
			}

			return null;
		}
	}

	public class ArrayUpdateDataObjectFactory : IArgumentObjectFactory
	{
		public object createObject( IAdaptingType argument )
		{
			AnonymousObject obj = (AnonymousObject) argument;
			ArrayUpdateData updateData = new ArrayUpdateData();
			updateData.index = (int) ((IAdaptingType) obj.Properties[ "i" ]).adapt( typeof( int ) );
			updateData.oldValue = (IAdaptingType) obj.Properties[ "o" ];
			updateData.newValue = (IAdaptingType) obj.Properties[ "n" ];
			updateData.actionType = (int) ((IAdaptingType) obj.Properties[ "f" ]).adapt( typeof( int ) );
			return updateData;
		}
	}

	public class ArrayUpdateData
	{
		public const int ADD = 0;
		public const int REMOVE = 6;
		public const int REPLACE = 8;
		public const int REMOVEALL = 10;
		public IAdaptingType oldValue;
		public IAdaptingType newValue;
		public int index;
		public int actionType;
	}

	public class PropUpdate
	{
		public string name;
		public IAdaptingType update;
	}

	public class MemberWrapper
	{
		public object memberInfo;
		public object obj;

		public MemberWrapper( object memberInfo, object obj )
		{
			this.memberInfo = memberInfo;
			this.obj = obj;
		}
	}
}
