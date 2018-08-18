using System;
using Tasky.BL;
using System.Collections.Generic;

namespace Tasky.DL
{
  /// <summary>
  /// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
  /// It contains methods for retrieval and persistance as well as db creation, all based on the 
  /// underlying ORM.
  /// </summary>
  public class TaskDatabase
  {
    public IEnumerable<T> GetItems<T>()
    {
      try
      {
        return BackendlessAPI.Backendless.Data.Of<T>().Find();
      }
      catch( BackendlessAPI.Exception.BackendlessException e )
      {
        if( e.FaultCode.Equals( "1009" ) )
        {
          var task = new Task();
          task = SaveItem<Task>( task );
          DeleteItem<Task>( task.ObjectId );
          return new List<T>();
        }
        else
          throw e;
      }
    }

    public T GetItem<T>( String id )
    {
      return BackendlessAPI.Backendless.Data.Of<T>().FindById( id );
    }

    public T SaveItem<T>( T item )
    {
      return BackendlessAPI.Backendless.Data.Of<T>().Save( item );
    }

    public void DeleteItem<T>( string id )
    {
      BackendlessAPI.Backendless.Data.Of<T>().Remove( $"objectId = '{id}'" );
    }
  }
}