using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using BackendlessAPI.Data;

namespace BackendlessAPI.Data
{
	public static class BackendlessV3DataExtensionMethods
	{
		public static DataQueryBuilder BackendlessDataQueryToDataQueryBuilder( BackendlessDataQuery dataQuery )
		{
			DataQueryBuilder dataQueryBuilder = DataQueryBuilder.Create();
			
			if ( !string.IsNullOrEmpty( dataQuery.WhereClause ) ) 
				dataQueryBuilder.SetWhereClause( dataQuery.WhereClause );

			if ( dataQuery.Offset > 0 )
				dataQueryBuilder.SetOffset( dataQuery.Offset );

			if ( dataQuery.PageSize > 0 )
				dataQueryBuilder.SetPageSize( dataQuery.PageSize );

			if ( dataQuery.Properties != null )
				dataQueryBuilder.SetProperties( dataQuery.Properties );

			if ( dataQuery.QueryOptions != null )
			{
				if ( dataQuery.QueryOptions.Offset > 0 )
					dataQueryBuilder.SetOffset( dataQuery.QueryOptions.Offset );

				if ( dataQuery.QueryOptions.PageSize > 0 )
					dataQueryBuilder.SetPageSize( dataQuery.QueryOptions.PageSize );

				if ( dataQuery.QueryOptions.Related != null )
					dataQueryBuilder.SetRelated( dataQuery.QueryOptions.Related );

				if ( dataQuery.QueryOptions.RelationsDepth > 0 )
					dataQueryBuilder.SetRelationsDepth( dataQuery.QueryOptions.RelationsDepth );

				if (dataQuery.QueryOptions.SortBy != null )
					dataQueryBuilder.SetSortBy( dataQuery.QueryOptions.SortBy );
			}
			
			return dataQueryBuilder;
		}
		
		public static IList<T> Find<T>( this IDataStore<T> dataStore, BackendlessDataQuery dataQuery )
		{
			return Backendless.Data.Find<T>( BackendlessDataQueryToDataQueryBuilder(dataQuery) );
		}

		public static void Find<T>( this IDataStore<T> dataStore, BackendlessDataQuery dataQuery, AsyncCallback<IList<T>> responder )
		{
			Backendless.Data.Find<T>( BackendlessDataQueryToDataQueryBuilder(dataQuery), responder );
		}
		
		public static IList<Dictionary<string, object>> Find( this IDataStore<Dictionary<String, Object>> dataStore, BackendlessDataQuery dataQuery )
		{
			return Backendless.Data.Find<Dictionary<string, object>>( BackendlessDataQueryToDataQueryBuilder( dataQuery ) );
		}
	
		public static void Find( this IDataStore<Dictionary<String, Object>> dataStore, BackendlessDataQuery dataQuery, AsyncCallback<IList<Dictionary<string, object>>> callback )
		{
			Backendless.Data.Find<Dictionary<string, object>>( BackendlessDataQueryToDataQueryBuilder( dataQuery ), callback );
		}
	}
}
