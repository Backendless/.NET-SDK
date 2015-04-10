using System;
using System.Collections.Generic;
using BackendlessAPI.Persistence;

namespace BackendlessAPI.File.Security
{
  abstract class BasePermission
  {
    public String folder { get; set; }
    public PermissionTypes access { get; set; }
    public FileOperation operaiton { get; set; }
  }
}
