using System;
using System.Collections;
using System.Data;
using System.Text;
using System.ComponentModel;
using Weborb.Util;
using Weborb.Util.License;
using Weborb.Util.Logging;
using Weborb.Message;
using Weborb.Registry;
using Weborb.Data;
using Weborb.Types;
using Weborb.Reader;
using Weborb.Config;

namespace Weborb.V3Types
{
	public class DataMessage : V3Message
	{
		internal int _operation;
        internal IDictionary _identity;

		public int operation
		{
			get
			{
				return _operation;
			}
			set
			{
				_operation = value;
			}
		}

		public IDictionary identity
		{
			get
			{
				return _identity;
			}
			set
			{
				_identity = value;
			}
		}

		public override V3Message execute( Request message, RequestContext context )
		{
			try
			{
                //if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() && !NetUtils.RequestIsLocal( ThreadContext.currentRequest() ) )
                //    throw new Exception( "cannot process data management requests, this feature is available in WebORB Professional/Enterprise Editions" );

				switch( operation )
				{
					case 1: // fill					
						return HandleFillRequest();

					case 3: // update
						return HandleUpdate();

					case 4: // delete
						return HandleDelete();

					case 7: // transaction
						return HandleTransaction( message, context );

					case 8: // page
						return HandlePagingRequest();

					case 11: // create and sequence
						return HandleCreateAndSequence();

                    case 18: // collection release
                        return new AckMessage( messageId, clientId, null );
				}
			}
			catch( Exception exception )
			{
                if( Log.isLogging( LoggingConstants.ERROR ) )
                    Log.log( LoggingConstants.ERROR, "error in the data management processing request", exception );

				ErrMessage errMessage = new ErrMessage( messageId, exception );
                errMessage.destination = this.destination;
                return errMessage;
			}

			return null;
		}

		private AckMessage HandleFillRequest()
		{
			int pageSize = -1;
			int sequenceId = -1;
			int totalRecords = 0;
			IAdaptingType[] adaptedArgs = null;
			IList dataList = null;

			if( headers.Contains( "pageSize" ) )
				pageSize = (int) headers[ "pageSize" ];

			//Destination destination = (Destination) ThreadContext.getORBConfig().getDataServices().GetDestinationManager().GetDestination( this.destination );					
            Destination destination = (Destination) ORBConfig.GetInstance().GetDataServices().GetDestinationManager().GetDestination( this.destination );					
			object[] args = (object[]) body.body;

			if( args.Length == 0 )
			{
				adaptedArgs = new IAdaptingType[ 0 ];
			}
			else 
			{
				adaptedArgs = new IAdaptingType[ args.Length ];
				args.CopyTo( adaptedArgs, 0 );
			}

			IListSource listSource = destination.Adapter.HandleFill( destination, pageSize, -1, adaptedArgs, ref totalRecords );
			dataList = GetListFromSource( listSource, destination.GetFillMethodReturnType( adaptedArgs ) );
			sequenceId = destination.AcquireSequence( destination.Adapter.PageFromCache() ? dataList : null, adaptedArgs );

			if( pageSize != -1 )
			{
				IList page = dataList;

				if( destination.Adapter.PageFromCache() )
					page = GetSubList( dataList, pageSize, 0 );
						
				return new PagedMessage( this, page, sequenceId, totalRecords );
			}
			else
			{
				return new SeqMessage( this, dataList, sequenceId, totalRecords );
			}
		}

		private V3Message HandleUpdate()
		{
			object[] args = (object[]) body.body;
			string[] fieldNames = (string[]) ((IAdaptingType) args[ 0 ]).defaultAdapt();
			object oldObject = ((IAdaptingType) args[ 1 ]).defaultAdapt();
			object newObject = ((IAdaptingType) args[ 2 ]).defaultAdapt();
            //Destination destination = (Destination)ThreadContext.getORBConfig().getDataServices().GetDestinationManager().GetDestination( this.destination );
            Destination destination = (Destination) ORBConfig.GetInstance().GetDataServices().GetDestinationManager().GetDestination( this.destination );

			ChangeObject changeObject = destination.Adapter.HandleUpdate( destination, fieldNames, oldObject, newObject );
			args[ 0 ] = fieldNames;
			args[ 1 ] = changeObject.OldVersion;
			args[ 2 ] = changeObject.NewVersion;

			/*		
			if( headers == null )
				headers = new Hashtable();

			Hashtable ids = destination.GetSequenceIdMaps( identity );
			StringBuilder builder = new StringBuilder( "*" );
			IDictionaryEnumerator en = ids.GetEnumerator();

			while( en.MoveNext() )
			{
				builder.Append( en.Key );
				builder.Append( "*" );
			}

			en.Reset();
			headers[ "sequenceIds" ] = builder.ToString();
			headers[ "sequenceIdMap" ] = ids;
			*/
			return this;
		}

		private V3Message HandleDelete()
		{
			object[] args = (object[]) body.body;
			body.body = ((IAdaptingType) args[ 0 ]).defaultAdapt();
            //Destination destination = (Destination)ThreadContext.getORBConfig().getDataServices().GetDestinationManager().GetDestination( this.destination );
            Destination destination = (Destination) ORBConfig.GetInstance().GetDataServices().GetDestinationManager().GetDestination( this.destination );
            ChangeObject changeObject = destination.Adapter.HandleDelete( destination, body.body );
            IDictionary identity = destination.GetIdentity( body.body );
			Sequence[] seq = destination.GetSequences();

			foreach( Sequence sequence in seq )
				sequence.DeleteObject( identity );

			return this;
		}

		private AckMessage HandleTransaction( Request request, RequestContext context )
		{
			if( body.body == null )
				body.body = new object[ 0 ];
			else if( !body.body.GetType().IsArray )
				body.body = new object[] { body.body };

			object[] args = (object[]) body.body;
			ArrayList responses = new ArrayList();
			AckMessage ackMessage = new AckMessage( messageId, clientId, responses );

			for( int i = 0; i < args.Length; i++ )
			{
				NamedObject namedObject = (NamedObject) args[ i ];
				V3Message message = (V3Message) namedObject.defaultAdapt();
				object result = message.execute( request, context );

				if( !(result is ICollection) )
				{
					ArrayList list = new ArrayList();
					list.Add( result );
					result = list;
				}

				IEnumerator en = ((ICollection) result).GetEnumerator();

				while( en.MoveNext() )
				{
					V3Message v3msg = (V3Message) en.Current;

                    if( !(v3msg is ErrMessage) )
                    {
                        v3msg.correlationId = ackMessage.correlationId;
                        v3msg.clientId = ackMessage.clientId;
                    }

					responses.Add( v3msg );
				}
			}

			return ackMessage;
		}	

		private AckMessage HandlePagingRequest()
		{
			int pageIndex = (int) headers[ "pageIndex" ];
			int sequenceId = (int) headers[ "sequenceId" ];
			int pageSize = (int) headers[ "pageSize" ];
			int totalRecords = 0;
			IList dataList;
            //Destination destination = (Destination)ThreadContext.getORBConfig().getDataServices().GetDestinationManager().GetDestination( this.destination );
            Destination destination = (Destination) ORBConfig.GetInstance().GetDataServices().GetDestinationManager().GetDestination( this.destination );
					
			if( destination.Adapter.PageFromCache() )
			{
				dataList = destination.GetSequenceData( sequenceId );
				totalRecords = dataList.Count;
				dataList = GetSubList( dataList, pageSize, pageIndex );
			}
			else
			{
				IAdaptingType[] args = destination.GetPseudoSequenceData( sequenceId );						
				IListSource listSource = destination.Adapter.HandleFill( destination, pageSize, pageIndex, args, ref totalRecords );
				dataList = GetListFromSource( listSource, destination.GetFillMethodReturnType( args ) );						
			}

			return new PagedMessage( this, dataList, sequenceId, totalRecords );
		}

		private V3Message HandleCreateAndSequence()
		{
			object[] args = (object[]) body.body;
			object newObject = ((IAdaptingType) args[ 0 ]).defaultAdapt();
            //Destination destination = (Destination)ThreadContext.getORBConfig().getDataServices().GetDestinationManager().GetDestination( this.destination );
            Destination destination = (Destination) ORBConfig.GetInstance().GetDataServices().GetDestinationManager().GetDestination( this.destination );
			ChangeObject changeObject = destination.Adapter.HandleCreate( destination, newObject );
			body.body = changeObject.NewVersion;
			identity = destination.GetIdentity( changeObject.NewVersion );
            Sequence[] seq = destination.GetSequences();

			V3CollectionMessage v3coll = new V3CollectionMessage();
			v3coll.AddMessage( new SeqMessage( this, new object[] { changeObject.NewVersion }, -1 ) );

			foreach( Sequence sequence in seq )
			{
				int totalRecords = 0;
				IListSource listSource = destination.Adapter.HandleFill( destination, -1, -1, sequence.RawArguments, ref totalRecords );
				IList dataList = GetListFromSource( listSource, destination.GetFillMethodReturnType( sequence.RawArguments ) );
				sequence.UpdateData( dataList );
				int index = sequence.GetObjectId( identity );

				if( index == -1 )
					continue;

				UpdateCollRange collRange = new UpdateCollRange( index, 0, new object[] { identity } );
				UpdateCollMessage updColl = new UpdateCollMessage( this.destination, messageId, clientId, new object[] { collRange }, sequence.Arguments );
				v3coll.AddMessage( updColl );
			}

			return v3coll;
		}

		private IList GetSubList( IList dataList, int pageSize, int pageIndex )
		{
			ArrayList page = new ArrayList( pageSize * (pageIndex + 1) >= dataList.Count ? dataList.Count - pageSize * pageIndex : pageSize );

			for( int i = 0; i < page.Capacity; i++ )
				page.Add( dataList[ i + pageIndex * pageSize ] );

			return page;
		}

		private IList GetListFromSource( IListSource listSource, string typeName )
		{
			if( listSource is DataTable )
				return GetListFromTable( (DataTable) listSource, typeName );
			else if( listSource is DataSet )
				return GetListFromTable( ((DataSet) listSource).Tables[ 0 ], typeName );
			else
				return listSource.GetList();
		}

		private IList GetListFromTable( DataTable table, string typeName )
		{
			ArrayList list = new ArrayList( table.Rows.Count );
			int columnCount = table.Columns.Count;
			string[] columnNames = new string[ columnCount ];

			for( int i = 0; i < columnCount; i++ )
				columnNames[ i ] = table.Columns[ i ].ColumnName;

			for( int i = 0; i < list.Capacity; i++ )
			{
				DataRow row = table.Rows[ i ];
				Hashtable rowData = new Hashtable();

				for( int k = 0; k < columnCount; k++ )
					rowData[ columnNames[ k ] ] = row[ columnNames[ k ] ];

				if( typeName == null )
					list.Add( rowData );
				else
					list.Add( new TypedObject( typeName, rowData ) );
			}

			return list;	
		}
	}
}
