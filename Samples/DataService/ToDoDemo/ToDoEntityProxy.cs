using ToDoDemo;

namespace Examples.MessagingService.ToDoDemo
{
  public class ToDoEntityProxy : IToDoEntity
  {
    private bool _done;
    private string _text;

    public ToDoEntityProxy( IToDoEntity toDoEntity )
    {
      objectId = toDoEntity.objectId;
      DeviceId = toDoEntity.DeviceId;
      _done = toDoEntity.Done;
      _text = toDoEntity.Text;
    }

    public string objectId { get; set; }

    public string DeviceId { get; set; }

    public bool Done
    {
      get { return _done; }
      set
      {
        _done = value;
        MainPage.DataStore.Save( GetToDoEntity(), null );
      }
    }

    public string Text
    {
      get { return _text; }
      set
      {
        _text = value;
        MainPage.DataStore.Save( GetToDoEntity(), null );
      }
    }

    private ToDoEntity GetToDoEntity()
    {
      return new ToDoEntity {DeviceId = DeviceId, Done = Done, objectId = objectId, Text = Text};
    }
  }
}