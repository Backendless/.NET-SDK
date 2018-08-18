using Weborb.Service;

namespace Tasky.BL
{
  /// <summary>
  /// Represents a Task.
  /// </summary>
  public class Task
  {
    [SetClientClassMemberName( "objectId" )]
    public string ObjectId { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    // new property
    public bool Done { get; set; }
  }
}

