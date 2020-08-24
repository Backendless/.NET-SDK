using System;
using Weborb.Service;

namespace BackendlessAPI.File
{
  public class FileInfo
  {
    [SetClientClassMemberName("name")]
    public String Name{ get; set; }
    [SetClientClassMemberName("createdOn")]
    public long CreatedOn{ get; set; }
    [SetClientClassMemberName("publicUrl")]
    public String PublicURL{ get; set; }
    [SetClientClassMemberName("url")]
    public String URL{ get; set; }
    [SetClientClassMemberName("size")]
    public Int32 Size{ get; set; }
  }
}
