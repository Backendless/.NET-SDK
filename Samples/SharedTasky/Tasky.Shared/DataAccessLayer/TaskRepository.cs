using System;
using System.Collections.Generic;
using System.IO;
using Tasky.BL;

namespace Tasky.DAL
{
  public class TaskRepository
  {
    DL.TaskDatabase db = null;
    protected static string dbLocation;
    protected static TaskRepository me;

    static TaskRepository()
    {
      me = new TaskRepository();
    }

    protected TaskRepository()
    {
      // instantiate the database	
      db = new Tasky.DL.TaskDatabase();
    }

    public static Task GetTask( string id )
    {
      return me.db.GetItem<Task>( id );
    }

    public static IEnumerable<Task> GetTasks()
    {
      return me.db.GetItems<Task>();
    }

    public static void SaveTask( Task item )
    {
      me.db.SaveItem<Task>( item );
    }

    public static void DeleteTask( string id )
    {
      me.db.DeleteItem<Task>( id );
    }
  }
}

