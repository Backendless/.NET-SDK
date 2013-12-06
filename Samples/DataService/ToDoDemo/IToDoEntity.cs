using BackendlessAPI;

namespace Examples.MessagingService.ToDoDemo
{
  public interface IToDoEntity
  {
    string objectId { get; set; }

    string DeviceId { get; set; }

    bool Done { get; set; }

    string Text { get; set; }
  }
}