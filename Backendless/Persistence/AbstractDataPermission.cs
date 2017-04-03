using System;
using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Service;

namespace BackendlessAPI.Persistence
{
  public abstract class AbstractDataPermission
  {
    private const String PERMISSION_SERVICE = "com.backendless.services.persistence.permissions.ClientPermissionService";

    protected abstract PersistenceOperation GetOperation();

    public void GrantForUser<T>( String userId, T dataObject )
    {
      GrantForUser( userId, dataObject, null );
    }

    public void GrantForUser<T>( String userId, T dataObject, AsyncCallback<Object> responder )
    {
      String method = "updateUserPermission";
      Object[] args = BuildArgs<T>( dataObject, userId, PermissionTypes.GRANT );
      ServerCall( responder, method, args );
    }

    public void DenyForUser<T>( String userId, T dataObject )
    {
      DenyForUser( userId, dataObject, null );
    }

    public void DenyForUser<T>( String userId, T dataObject, AsyncCallback<Object> responder )
    {
      String method = "updateUserPermission";
      Object[] args = BuildArgs<T>( dataObject, userId, PermissionTypes.DENY );
      ServerCall( responder, method, args );
    }

    public void GrantForRole<T>( String roleName, T dataObject )
    {
      GrantForRole( roleName, dataObject, null );
    }

    public void GrantForRole<T>( String roleName, T dataObject, AsyncCallback<Object> responder )
    {
      String method = "updateRolePermission";
      Object[] args = BuildArgs<T>( dataObject, roleName, PermissionTypes.GRANT );
      ServerCall( responder, method, args );
    }

    public void DenyForRole<T>( String roleName, T dataObject )
    {
      DenyForRole( roleName, dataObject, null );
    }

    public void DenyForRole<T>( String roleName, T dataObject, AsyncCallback<Object> responder )
    {
      String method = "updateRolePermission";
      Object[] args = BuildArgs<T>( dataObject, roleName, PermissionTypes.DENY );
      ServerCall( responder, method, args );
    }

    public void GrantForAllUsers<T>( T dataObject )
    {
      GrantForAllUsers( dataObject, null );
    }

    public void GrantForAllUsers<T>( T dataObject, AsyncCallback<Object> responder )
    {
      String method = "updateAllUserPermission";
      Object[] args = BuildArgs<T>( dataObject, null, PermissionTypes.GRANT );
      ServerCall( responder, method, args );
    }

    public void DenyForAllUsers<T>( Object dataObject )
    {
      DenyForAllUsers( dataObject, null );
    }

    public void DenyForAllUsers<T>( T dataObject, AsyncCallback<Object> responder )
    {
      String method = "updateAllUserPermission";
      Object[] args = BuildArgs<T>( dataObject, null, PermissionTypes.DENY );
      ServerCall( responder, method, args );
    }

    public void GrantForAllRoles<T>( T dataObject )
    {
      GrantForAllRoles( dataObject, null );
    }

    public void GrantForAllRoles<T>( T dataObject, AsyncCallback<Object> responder )
    {
      String method = "updateAllRolePermission";
      Object[] args = BuildArgs<T>( dataObject, null, PermissionTypes.GRANT );
      ServerCall( responder, method, args );
    }

    public void DenyForAllRoles<T>( T dataObject )
    {
      DenyForAllRoles( dataObject, null );
    }

    public void DenyForAllRoles<T>( T dataObject, AsyncCallback<Object> responder )
    {
      String method = "updateAllRolePermission";
      Object[] args = BuildArgs<T>( dataObject, null, PermissionTypes.DENY );
      ServerCall( responder, method, args );
    }

    private Object[] BuildArgs<T>( T dataObject, String principal, PermissionTypes permissionType )
    {
      String tableName = PersistenceService.GetTypeName( dataObject.GetType() );
      String objectId = PersistenceService.GetEntityId<T>( dataObject );

      if( principal != null )
        return new Object[] { tableName, principal, objectId, GetOperation(), permissionType };
      else
        return new Object[] { tableName, objectId, GetOperation(), permissionType };
    }

    private void ServerCall( AsyncCallback<Object> responder, String method, Object[] args )
    {
      if( responder != null )
        Invoker.InvokeAsync<Object>( PERMISSION_SERVICE, method, args, responder );
      else
        Invoker.InvokeSync<Object>( PERMISSION_SERVICE, method, args );
    }
  }
}
