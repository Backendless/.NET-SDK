using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acquaint.Abstractions;
using Acquaint.Models;
using Acquaint.Util;
using BackendlessAPI;
using BackendlessAPI.Persistence;

namespace Acquaint.Data
{
  public class BackendlessDataSource : IDataSource<Acquaintance>
  {
    string _DataPartitionId => GuidUtility.Create( Settings.DataPartitionPhrase ).ToString().ToUpper();

    public BackendlessDataSource()
    {
      Backendless.InitApp( Settings.BackendlessAPPID, Settings.BackendlessAPIKEY );
    }
    public async Task<bool> AddItem( Acquaintance item )
    {
      try
      {
        item.DataPartitionId = _DataPartitionId;
        await Backendless.Data.Of<Acquaintance>().SaveAsync( item );
        return true;
      }
      catch
      {
        return false;
      }
    }

    public async Task<Acquaintance> GetItem( string id )
    {
      await EnsureDataIsSeededAsync();
      return await Backendless.Data.Of<Acquaintance>().FindByIdAsync( id );
    }

    public async Task<IEnumerable<Acquaintance>> GetItems()
    {
      await EnsureDataIsSeededAsync();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetPageSize( 100 );
      queryBuilder.SetWhereClause( $"DataPartitionId = '{_DataPartitionId}'" );
      List<String> sortByList = new List<string>();
      sortByList.Add( "LastName" );
      queryBuilder.SetSortBy( sortByList );

      IList<Acquaintance> items = await Backendless.Data.Of<Acquaintance>().FindAsync( queryBuilder );

      if( items.Count == 0 )
      {
        Settings.DataIsSeeded = false;
        await EnsureDataIsSeededAsync();
        return await GetItems();
      }

      return items;
    }

    public async Task<bool> RemoveItem( Acquaintance item )
    {
      try
      {
        await Backendless.Data.Of<Acquaintance>().RemoveAsync( item );
        return true;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> UpdateItem( Acquaintance item )
    {
      try
      {
        await Backendless.Data.Of<Acquaintance>().SaveAsync( item );
        return true;
      }
      catch
      {
        return false;
      }
    }

    async Task EnsureDataIsSeededAsync()
    {
      if( Settings.DataIsSeeded )
        return;

      int count = 0;

      try
      {
        await Backendless.Data.Of<Acquaintance>().GetObjectCountAsync( DataQueryBuilder.Create().SetWhereClause( $"DataPartitionId = '{_DataPartitionId}'" ) );
      }
      catch( Exception )
      {

      }

      if( count > 0 )
        Settings.DataIsSeeded = true;

      if( !Settings.DataIsSeeded )
      {
        var newItems = SeedData.Get( _DataPartitionId );

        await Backendless.Data.Of<Acquaintance>().CreateAsync( newItems );

        Settings.DataIsSeeded = true;
      }
    }
  }
}
