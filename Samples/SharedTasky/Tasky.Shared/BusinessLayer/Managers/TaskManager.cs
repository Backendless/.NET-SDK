using System;
using System.Collections.Generic;
using Tasky.BL;

namespace Tasky.BL.Managers
{
  public static class TaskManager
  {
    static TaskManager()
    {
    }

    public static Task GetTask( string id )
    {
      return DAL.TaskRepository.GetTask( id );
    }

    public static IList<Task> GetTasks()
    {
      return new List<Task>( DAL.TaskRepository.GetTasks() );
    }

    public static void SaveTask( Task item )
    {
      DAL.TaskRepository.SaveTask( item );
    }

    public static void DeleteTask( string id )
    {
      DAL.TaskRepository.DeleteTask( id );
    }

  }
}