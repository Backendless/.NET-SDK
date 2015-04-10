using System;
using System.Collections.Generic;
using BackendlessAPI.File.Security;

namespace BackendlessAPI.File
{
  public class FilePermission
  {
    public readonly Read READ = new Read();
    public readonly Delete DELETE = new Delete();
    public readonly Write WRITE = new Write();
    public readonly Permission PERMISSION = new Permission();

    public class Read : AbstractFilePermission
    {
      protected override FileOperation GetOperation()
      {
        return FileOperation.READ;
      }
    }

    public class Delete : AbstractFilePermission
    {
      protected override FileOperation GetOperation()
      {
        return FileOperation.DELETE;
      }
    }

    public class Write : AbstractFilePermission
    {
      protected override FileOperation GetOperation()
      {
        return FileOperation.WRITE;
      }
    }

    public class  Permission : AbstractFilePermission
    {
      protected override FileOperation GetOperation()
      {
        return FileOperation.PERMISSION;
      }
    }
  }
}
