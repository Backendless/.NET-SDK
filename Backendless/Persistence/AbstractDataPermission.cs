using System;
using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Service;

namespace BackendlessAPI.Persistence
{
  public abstract class AbstractDataPermission
  {
    private const String PERMISSION_SERVICE = "com.backendless.services.persistence.permissions.ClientPermissionService";

    protected abstract PersistenceOperation getOperation();

    public void GrantForUser<T>( String userId, T dataObject )
    {
      GrantForUser( userId, dataObject, null );
    }

    public void GrantForUser<T>( String userId, T dataObject, AsyncCallback<T> responder )
    {
      String method = "updateUserPermission";
      Object[] args = buildArgs<T>( dataObject, userId, PermissionTypes.GRANT );
      serverCall( responder, method, args );
    }

    public void DenyForUser<T>( String userId, T dataObject )
    {
      DenyForUser( userId, dataObject, null );
    }

    public void DenyForUser<T>( String userId, T dataObject, AsyncCallback<T> responder )
    {
      String method = "updateUserPermission";
      Object[] args = buildArgs<T>( dataObject, userId, PermissionTypes.DENY );
      serverCall( responder, method, args );
    }

    public void GrantForRole<T>( String roleName, T dataObject )
    {
      GrantForRole( roleName, dataObject, null );
    }

    public void GrantForRole<T>( String roleName, T dataObject, AsyncCallback<T> responder )
    {
      String method = "updateRolePermission";
      Object[] args = buildArgs<T>( dataObject, roleName, PermissionTypes.GRANT );
      serverCall( responder, method, args );
    }

    public void DenyForRole<T>( String roleName, T dataObject )
    {
      DenyForRole( roleName, dataObject, null );
    }

    public void DenyForRole<T>( String roleName, T dataObject, AsyncCallback<T> responder )
    {
      String method = "updateRolePermission";
      Object[] args = buildArgs<T>( dataObject, roleName, PermissionTypes.DENY );
      serverCall( responder, method, args );
    }

    public void GrantForAllUsers<T>( T dataObject )
    {
      GrantForAllUsers( dataObject, null );
    }

    public void GrantForAllUsers<T>( T dataObject, AsyncCallback<T> responder )
    {
      String method = "updateAllUserPermission";
      Object[] args = buildArgs<T>( dataObject, null, PermissionTypes.GRANT );
      serverCall( responder, method, args );
    }

    public void DenyForAllUsers<T>( Object dataObject )
    {
      DenyForAllUsers( dataObject, null );
    }

    public void DenyForAllUsers<T>( T dataObject, AsyncCallback<T> responder )
    {
      String method = "updateAllUserPermission";
      Object[] args = buildArgs<T>( dataObject, null, PermissionTypes.DENY );
      serverCall( responder, method, args );
    }

    public void GrantForAllRoles<T>( T dataObject )
    {
      GrantForAllRoles( dataObject, null );
    }

    public void GrantForAllRoles<T>( T dataObject, AsyncCallback<T> responder )
    {
      String method = "updateAllRolePermission";
      Object[] args = buildArgs<T>( dataObject, null, PermissionTypes.GRANT );
      serverCall( responder, method, args );
    }

    public void DenyForAllRoles<T>( T dataObject )
    {
      DenyForAllRoles( dataObject, null );
    }

    public void DenyForAllRoles<T>( T dataObject, AsyncCallback<T> responder )
    {
      String method = "updateAllRolePermission";
      Object[] args = buildArgs<T>( dataObject, null, PermissionTypes.DENY );
      serverCall( responder, method, args );
    }

    private Object[] buildArgs<T>( T dataObject, String principal, PermissionTypes permissionType )
    {
      String tableName = PersistenceService.GetTypeName( dataObject.GetType() );
      String objectId = PersistenceService.GetEntityId<T>( dataObject );

      if( principal != null )
        return new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, principal, objectId, getOperation(), permissionType };
      else
        return new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, objectId, getOperation(), permissionType };
    }

    private void serverCall<T>( AsyncCallback<T> responder, String method, Object[] args )
    {
      if( responder != null )
        Invoker.InvokeAsync<T>( PERMISSION_SERVICE, method, args, responder );
      else
        Invoker.InvokeSync<T>( PERMISSION_SERVICE, method, args );
    }
  }
}
