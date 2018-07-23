using System;
namespace BackendlessAPI.RT
{
  public enum RTErrorType
  {
    OBJECTCREATED,
    OBJECTUPDATED,
    OBJECTDELETED,
    BULKDELETE,
    BULKUPDATE,
    COMMAND,
    MESSAGELISTENER,
    USERSTATUSLISTENER
  }
}
