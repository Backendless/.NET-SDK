using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Examples.MessagingService.ToDoDemo
{
  public class ToDoList : ObservableCollection<ToDoEntityProxy>
  {
    public void AddAll( IEnumerable<ToDoEntity> entities )
    {
      foreach( var toDoEntity in entities )
        Add(new ToDoEntityProxy(toDoEntity));
    }
  }
}