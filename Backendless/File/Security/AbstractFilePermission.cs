using System;
using BackendlessAPI.Persistence;
using BackendlessAPI.Engine;
using BackendlessAPI.Async;

namespace BackendlessAPI.File.Security
{
  public abstract class AbstractFilePermission
  {
    private const String FILEPERMISSION_SERVICE = "com.backendless.services.file.FileService";

    protected abstract FileOperation GetOperation();

    public void GrantForUser( String userId, String fileOrDirURL )
    {
      GrantForUser( userId, fileOrDirURL, null );
    }

    public void GrantForUser( String userId, String fileOrDirURL, AsyncCallback<Object> responder )
    {
      String method = "updateUserPermission";
      Object[] args = BuildArgs( fileOrDirURL, userId, false, PermissionTypes.GRANT );
      ServerCall( responder, method, args );
    }

    public void DenyForUser( String userId, String fileOrDirURL )
    {
      DenyForUser( userId, fileOrDirURL, null );
    }

    public void DenyForUser( String userId, String fileOrDirURL, AsyncCallback<Object> responder )
    {
      String method = "updateUserPermission";
      Object[] args = BuildArgs( fileOrDirURL, userId, false, PermissionTypes.DENY );
      ServerCall( responder, method, args );
    }

    public void GrantForRole( String roleName, String fileOrDirURL )
    {
      GrantForRole( roleName, fileOrDirURL, null );
    }

    public void GrantForRole( String roleName, String fileOrDirURL, AsyncCallback<Object> responder )
    {
      String method = "updateRolePermission";
      Object[] args = BuildArgs( fileOrDirURL, roleName, true, PermissionTypes.GRANT );
      ServerCall( responder, method, args );
    }

    public void DenyForRole( String roleName, String fileOrDirURL )
    {
      DenyForRole( roleName, fileOrDirURL, null );
    }

    public void DenyForRole( String roleName, String fileOrDirURL, AsyncCallback<Object> responder )
    {
      String method = "updateRolePermission";
      Object[] args = BuildArgs( fileOrDirURL, roleName, true, PermissionTypes.DENY );
      ServerCall( responder, method, args );
    }

    public void GrantForAllUsers( String fileOrDirURL )
    {
      GrantForAllUsers( fileOrDirURL, null );
    }

    public void GrantForAllUsers( String fileOrDirURL, AsyncCallback<Object> responder )
    {
      String method = "updatePermissionForAllUsers";
      Object[] args = BuildArgs( fileOrDirURL, null, false, PermissionTypes.GRANT );
      ServerCall( responder, method, args );
    }

    public void DenyForAllUsers( String fileOrDirURL )
    {
      DenyForAllUsers( fileOrDirURL, null );
    }

    public void DenyForAllUsers( String fileOrDirURL, AsyncCallback<Object> responder )
    {
      String method = "updatePermissionForAllUsers";
      Object[] args = BuildArgs( fileOrDirURL, null, false, PermissionTypes.DENY );
      ServerCall( responder, method, args );
    }

    public void GrantForAllRoles( String fileOrDirURL )
    {
      GrantForAllRoles( fileOrDirURL, null );
    }

    public void GrantForAllRoles( String fileOrDirURL, AsyncCallback<Object> responder )
    {
      String method = "updateRolePermissionsForAllRoles";
      Object[] args = BuildArgs( fileOrDirURL, null, false, PermissionTypes.GRANT );
      ServerCall( responder, method, args );
    }

    public void DenyForAllRoles( String fileOrDirURL )
    {
      DenyForAllRoles( fileOrDirURL, null );
    }

    public void DenyForAllRoles( String fileOrDirURL, AsyncCallback<Object> responder )
    {
      String method = "updateRolePermissionsForAllRoles";
      Object[] args = BuildArgs( fileOrDirURL, null, true, PermissionTypes.DENY );
      ServerCall( responder, method, args );
    }

    private Object[] BuildArgs( String fileOrDirURL, String principal, bool isRole, PermissionTypes permissionType )
    {
      FileOperation operation = GetOperation();
      BasePermission permission;

      if( isRole )
        permission = new FileRolePermission();
      else
        permission = new FileUserPermission();

      permission.folder = fileOrDirURL;
      permission.access = permissionType;
      permission.operaiton = operation;

      if( principal != null )
        return new Object[] { principal, permission };
      else
        return new Object[] { permission };
    }

    private void ServerCall( AsyncCallback<Object> responder, String method, Object[] args )
    {
      if( responder != null )
        Invoker.InvokeAsync<Object>( FILEPERMISSION_SERVICE, method, args, responder );
      else
        Invoker.InvokeSync<Object>( FILEPERMISSION_SERVICE, method, args );
    }
  }
}
