using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BackendlessAPI;

namespace Examples.MessagingService.ToDoDemo
{
  public class ToDoEntity : IToDoEntity
  {
    public string objectId { get; set; }
    public string DeviceId { get; set; }
    public bool Done { get; set; }
    public string Text { get; set; }
  }
}