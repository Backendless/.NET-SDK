using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.Persistence;
using BackendlessAPI.Property;
using Weborb.Client;
using Weborb.Service;
using Weborb.Types;
using Weborb.Writer;
using BackendlessAPI.IO;
using BackendlessAPI.Utils;

namespace BackendlessAPI.Service
{
  public class PersistenceService
  {
    private const string PERSISTENCE_MANAGER_SERVER_ALIAS = "com.backendless.services.persistence.PersistenceService";
    private const string DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE = "objectId";
    private const string DEFAULT_CREATED_FIELD_NAME_JAVA_STYLE = "created";
    private const string DEFAULT_UPDATED_FIELD_NAME_JAVA_STYLE = "updated";
    private const string DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE = "ObjectId";
    private const string DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE = "Created";
    private const string DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE = "Updated";
    public const string LOAD_ALL_RELATIONS = "*";

    public PersistenceService()
    {
      Types.AddClientClassMapping( "com.backendless.services.persistence.BackendlessDataQuery",
                                   typeof( BackendlessDataQuery ) );
      Types.AddClientClassMapping( "com.backendless.services.persistence.BackendlessCollection",
                                   typeof( BackendlessCollection<> ) );
      Types.AddClientClassMapping( "com.backendless.services.persistence.ObjectProperty", typeof( ObjectProperty ) );
      Types.AddClientClassMapping( "com.backendless.services.persistence.QueryOptions", typeof( QueryOptions ) );
    }
    #region Save
    public T Save<T>( T entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      CheckEntityStructure<T>();

      //return GetEntityId( entity ) == null ? Create( entity ) : Update( entity );
      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "save",
                                    new object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity }, true );
    }

    public void Save<T>( T entity, AsyncCallback<T> callback )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      CheckEntityStructure<T>();

      /*if( GetEntityId( entity ) == null )
        Create( entity, callback );
      else
        Update( entity, callback );*/
      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "save",
                           new object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity }, true, callback );
    }
    #endregion
    #region Create
    internal T Create<T>( T entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "create",
                                    new object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity }, true );
    }

    internal void Create<T>( T entity, AsyncCallback<T> callback )
    {
      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "create",
                           new object[] {Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity}, true, callback );
    }
    #endregion
    #region Update
    internal T Update<T>( T entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      AddWeborbPropertyMapping<T>();
      T result = Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "update",
                                    new Object[] {Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity}, true );
      return result;
    }

    internal void Update<T>( T entity, AsyncCallback<T> callback )
    {
      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "update",
                           new object[] {Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity}, true, callback );
    }
    #endregion
    #region Remove by Object
    internal long Remove<T>( T entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove",
                                       new Object[] {Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity} );
    }

    internal void Remove<T>( T entity, AsyncCallback<long> callback )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove",
                           new Object[] {Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity}, callback );
    }
    #endregion 
    #region Remove by objectId
    internal long Remove<T>( string objectId )
    {
      if( string.IsNullOrEmpty( objectId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove",
                                       new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), objectId } );
    }

    internal void Remove<T>( string objectId, AsyncCallback<long> callback )
    {
      if( string.IsNullOrEmpty( objectId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove",
                           new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), objectId }, callback );
    }
    #endregion 
    #region FindById with Id
    internal T FindById<T>( string id, IList<string> relations )
    {
      return FindById<T>( id, relations, 0 );
    }

    internal T FindById<T>( string id, IList<string> relations, int relationsDepth )
    {
      if( id == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById",
                                    new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), id, relations, relationsDepth }, true );
    }

    internal void FindById<T>( string id, IList<string> relations, AsyncCallback<T> callback )
    {
      FindById<T>( id, relations, 0, callback );
    }

    internal void FindById<T>( string id, IList<string> relations, int relationsDepth, AsyncCallback<T> callback )
    {
      if( id == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById",
                           new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), id, relations, relationsDepth }, true, callback );
    }
    #endregion
    #region FindById with Object
    internal T FindById<T>( T entity )
    {
      return FindById<T>( entity, null, 0 );
    }

    internal T FindById<T>( T entity, int relationsDepth )
    {
      return FindById<T>( entity, null, relationsDepth );
    }

    internal T FindById<T>( T entity, IList<string> relations )
    {
      return FindById<T>( entity, relations, 0 );
    }

    internal T FindById<T>( T entity, IList<string> relations, int relationsDepth )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById",
                           new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity, relations, relationsDepth }, true );
    }

    internal void FindById<T>( T entity, AsyncCallback<T> responder )
    {
      FindById<T>( entity, null, 0, responder );
    }

    internal void FindById<T>( T entity, int relationsDepth, AsyncCallback<T> responder )
    {
      FindById<T>( entity, null, relationsDepth, responder );
    }

    internal void FindById<T>( T entity, IList<string> relations, AsyncCallback<T> responder )
    {
      FindById<T>( entity, relations, 0, responder );
    }

    internal void FindById<T>( T entity, IList<string> relations, int relationsDepth, AsyncCallback<T> responder )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById",
                           new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity, relations, relationsDepth }, true, responder );
    }
    #endregion
    #region LoadRelations
    internal void LoadRelations<T>( T entity, IList<string> relations )
    {
      LoadRelations<T>( entity, relations, 0 );
    }

    internal void LoadRelations<T>( T entity, int relationsDepth )
    {
      LoadRelations<T>( entity, null, relationsDepth );
    }

    internal void LoadRelations<T>( T entity, IList<string> relations, int relationsDepth )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      //String id = GetEntityId( entity );

      //if( id == null )
      //  throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      if( relations == null )
        relations = new List<String>();

      Object[] methodArgs = null;

      if( relationsDepth == 0 )
        methodArgs = new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity, relations };
      else
        methodArgs = new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity, relations, relationsDepth };
        
      var loadedRelations = Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "loadRelations", methodArgs, true );
      LoadRelationsToEntity( entity, loadedRelations, relations );
    }

    internal void LoadRelations<T>( T entity, int relationsDepth, AsyncCallback<T> callback )
    {
      LoadRelations<T>( entity, null, relationsDepth, callback );
    }

    internal void LoadRelations<T>( T entity, IList<string> relations, AsyncCallback<T> callback )
    {
      LoadRelations<T>( entity, relations, 0, callback );
    }

    internal void LoadRelations<T>( T entity, IList<string> relations, int relationsDepth, AsyncCallback<T> callback )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      //String id = GetEntityId( entity );

      //if( id == null )
      //  throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      var responder = new AsyncCallback<T>( response =>
        {
          LoadRelationsToEntity( entity, response, relations );
          if( callback != null )
            callback.ResponseHandler.Invoke( response );
        }, fault =>
          {
            if( callback != null )
              callback.ErrorHandler.Invoke( fault );
          } );
      Invoker.InvokeAsync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "loadRelations",
                              new Object[] {Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), entity, relations, relationsDepth },
                              true,
                              responder );
    }

    private void LoadRelationsToEntity<T>( T entity, T response, IList<string> relations )
    {
      if( typeof( T ).Equals( typeof( BackendlessUser ) ) )
      {
        object source = entity;
        object updated = response;
        BackendlessUser userWithRelations = (BackendlessUser) updated;
        BackendlessUser sourceUser = (BackendlessUser) source;
        sourceUser.PutProperties( userWithRelations.Properties );
      }
      else
      {
        FieldInfo[] fields = typeof( T ).GetFields();
        foreach( var fieldInfo in fields )
        {
          if( !relations.Contains( fieldInfo.Name ) )
            continue;

          fieldInfo.SetValue( entity, fieldInfo.GetValue( response ) );
        }
      }
    }
    #endregion
    #region Describe
    public List<ObjectProperty> Describe( string className )
    {
      if( string.IsNullOrEmpty( className ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY_NAME );

      return Invoker.InvokeSync<List<ObjectProperty>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "describe",
                                                       new Object[]
                                                         {Backendless.AppId, Backendless.VersionNum, className} );
    }

    public void Describe( string className, AsyncCallback<List<ObjectProperty>> callback )
    {
      if( string.IsNullOrEmpty( className ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY_NAME );

      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "describe",
                           new Object[] {Backendless.AppId, Backendless.VersionNum, className}, callback );
    }
    #endregion
    #region Find
    public BackendlessCollection<T> Find<T>( BackendlessDataQuery dataQuery )
    {
      CheckPageSizeAndOffset( dataQuery );
      AddWeborbPropertyMapping<T>();
      var result = Invoker.InvokeSync<BackendlessCollection<T>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "find",
                                                                 new object[]
                                                                   {
                                                                     Backendless.AppId, Backendless.VersionNum,
                                                                     GetTypeName( typeof( T ) ), dataQuery
                                                                   }, true );
      result.Query = dataQuery;

      return result;
    }

    public void Find<T>( BackendlessDataQuery dataQuery, AsyncCallback<BackendlessCollection<T>> callback )
    {
      var responder = new AsyncCallback<BackendlessCollection<T>>( r =>
        {
          r.Query = dataQuery;

          if( callback != null )
            callback.ResponseHandler.Invoke( r );
        }, f =>
          {
            if( callback != null )
              callback.ErrorHandler.Invoke( f );
            else
              throw new BackendlessException( f );
          } );

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "find",
                           new object[] {Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), dataQuery},
                           true,
                           responder );
    }
    #endregion
    #region First
    public T First<T>()
    {
      return First<T>( null, 0 );
    }

    public T First<T>( int relationsDepth )
    {
      return First<T>( null, relationsDepth );
    }

    public T First<T>( IList<String> relations )
    {
      return First<T>( relations, 0 );
    }

    public T First<T>( IList<String> relations, int relationsDepth )
    {
      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "first",
                                    new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), relations, relationsDepth },
                                    true );
    }

    public void First<T>( AsyncCallback<T> callback )
    {
      First<T>( null, 0, callback );
    }

    public void First<T>( int relationsDepth, AsyncCallback<T> callback )
    {
      First<T>( null, relationsDepth, callback );
    }

    public void First<T>( IList<String> relations, AsyncCallback<T> callback )
    {
      First<T>( relations, 0, callback );
    }

    public void First<T>( IList<String> relations, int relationsDepth, AsyncCallback<T> callback )
    {
      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "first",
                           new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), relations, relationsDepth }, 
                           true,
                           callback );
    }
    #endregion 
    #region Last
    public T Last<T>()
    {
      return Last<T>( null, 0 );
    }

    public T Last<T>( int relationsDepth )
    {
      return Last<T>( null, relationsDepth );
    }

    public T Last<T>( IList<String> relations )
    {
      return Last<T>( relations, 0 );
    }

    public T Last<T>( IList<String> relations, int relationsDepth )
    {
      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "last",
                                    new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), relations, relationsDepth },
                                    true );
    }

    public void Last<T>( AsyncCallback<T> callback )
    {
      Last<T>( null, 0, callback );
    }

    public void Last<T>( int relationsDepth, AsyncCallback<T> callback )
    {
      Last<T>( null, relationsDepth, callback );
    }

    public void Last<T>( IList<String> relations, AsyncCallback<T> callback )
    {
      Last<T>( relations, 0, callback );
    }

    public void Last<T>( IList<String> relations, int relationsDepth, AsyncCallback<T> callback )
    {
      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "last",
                           new Object[] { Backendless.AppId, Backendless.VersionNum, GetTypeName( typeof( T ) ), relations, relationsDepth }, 
                           true,
                           callback );
    }
    #endregion

    public IDataStore<T> Of<T>()
    {
      return DataStoreFactory.CreateDataStore<T>();
    }

    public void MapTableToType( string tableName, Type type )
    {
      Weborb.Types.Types.AddClientClassMapping( tableName, type );
    }

    private static string GetEntityId<T>( T entity )
    {
      try
      {
        Type entityType = entity.GetType();

        if( entityType.Equals( typeof( BackendlessUser ) ) )
        {
          object entityObject = entity;
          return ( (BackendlessUser) entityObject ).UserId;
        }

        PropertyInfo objectIdProp = entityType.GetProperty( DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE );

        if( objectIdProp == null )
          objectIdProp = entityType.GetProperty( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );

        if( objectIdProp != null )
          return objectIdProp.GetValue( entity, null ) as string;

        IDictionary<string, object> underflow = UnderflowStore.GetObjectUnderflow( entity );

        if( underflow != null && underflow.ContainsKey( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE ) )
          return (string) underflow[ DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE ];
      }
      catch( System.Exception e )
      {
      }

      return null;
    }

    private static void CheckEntityStructure<T>()
    {
      Type entityClass = typeof( T );

      if( entityClass.IsArray || entityClass.IsAssignableFrom( typeof( IEnumerable ) ) )
        throw new ArgumentException( ExceptionMessage.WRONG_ENTITY_TYPE );

      try
      {
        entityClass.GetConstructor( new Type[ 0 ] );
      }
      catch( System.Exception )
      {
        throw new ArgumentException( ExceptionMessage.ENTITY_MISSING_DEFAULT_CONSTRUCTOR );
      }

      /*
      // OBJECTID
      PropertyInfo objectIdProp = entityClass.GetProperty( DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE );
      Type objectIdType = null;

      if( objectIdProp == null )
        objectIdProp = entityClass.GetProperty( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );

      if( objectIdProp != null )
      {
        objectIdType = objectIdProp.PropertyType;
      }
      else
      {
        FieldInfo objectIdField = entityClass.GetField( DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE );

        if( objectIdField == null )
          objectIdField = entityClass.GetField( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );

        if( objectIdField != null )
          objectIdType = objectIdField.FieldType;
      }

      if( objectIdType != null && objectIdType != typeof( string ) )
        throw new ArgumentException( ExceptionMessage.ENTITY_WRONG_OBJECT_ID_FIELD_TYPE );

      // CREATED FIELD/PROPERTY
      Type createdType = null;
      PropertyInfo createdProp = entityClass.GetProperty( DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE );

      if( createdProp == null )
        createdProp = entityClass.GetProperty( DEFAULT_CREATED_FIELD_NAME_JAVA_STYLE );

      if( createdProp != null )
      {
        createdType = createdProp.PropertyType;
      }
      else
      {
        FieldInfo createdField = entityClass.GetField( DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE, BindingFlags.Public | BindingFlags.Instance );

        if( createdField == null )
          createdField = entityClass.GetField( DEFAULT_CREATED_FIELD_NAME_JAVA_STYLE, BindingFlags.Instance | BindingFlags.Public );

        if( createdField != null )
          createdType = createdField.FieldType;
      }

      if( createdType != null && createdType != typeof( DateTime ) && createdType != typeof( DateTime? ) )
        throw new ArgumentException( ExceptionMessage.ENTITY_WRONG_CREATED_FIELD_TYPE );


      // UPDATED FIELD/PROPERTY
      Type updatedType = null;
      PropertyInfo updatedProp = entityClass.GetProperty( DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE );

      if( updatedProp == null )
        updatedProp = entityClass.GetProperty( DEFAULT_UPDATED_FIELD_NAME_JAVA_STYLE );

      if( updatedProp != null )
      {
        updatedType = updatedProp.PropertyType;
      }
      else
      {
        FieldInfo updatedField = entityClass.GetField( DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE, BindingFlags.Public | BindingFlags.Instance );

        if( updatedField == null )
          updatedField = entityClass.GetField( DEFAULT_UPDATED_FIELD_NAME_JAVA_STYLE, BindingFlags.Instance | BindingFlags.Public );

        if( updatedField != null )
          updatedType = updatedField.FieldType;
      }

      if( updatedType != null && updatedType != typeof( DateTime ) && updatedType != typeof( DateTime? ) )
        throw new ArgumentException( ExceptionMessage.ENTITY_WRONG_UPDATED_FIELD_TYPE );
       */ 
    }

    private static void AddWeborbPropertyMapping<T>()
    {
      var entityType = typeof( T );
      var entityProperties = entityType.GetProperties();

      foreach( var entityProperty in entityProperties )
        switch( entityProperty.Name )
        {
          case DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE:
            if(
              string.IsNullOrEmpty( PropertyRenaming.GetRenamingRule( entityType,
                                                                      DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE ) ) )
              PropertyRenaming.AddRenamingRule( entityType, DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE,
                                                DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );
            break;

          case DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE:
            if(
              string.IsNullOrEmpty( PropertyRenaming.GetRenamingRule( entityType,
                                                                      DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE ) ) )
              PropertyRenaming.AddRenamingRule( entityType, DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE,
                                                DEFAULT_UPDATED_FIELD_NAME_JAVA_STYLE );
            break;

          case DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE:
            if(
              string.IsNullOrEmpty( PropertyRenaming.GetRenamingRule( entityType,
                                                                      DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE ) ) )
              PropertyRenaming.AddRenamingRule( entityType, DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE,
                                                DEFAULT_CREATED_FIELD_NAME_JAVA_STYLE );
            break;
        }
    }

    private static void CheckPageSizeAndOffset( IBackendlessQuery dataQuery )
    {
      if( dataQuery == null )
        return;

      if( dataQuery.Offset < 0 )
        throw new ArgumentException( ExceptionMessage.WRONG_OFFSET );

      if( dataQuery.PageSize < 0 )
        throw new ArgumentException( ExceptionMessage.WRONG_PAGE_SIZE );
    }

    private static string GetTypeName( Type type )
    {
      if( type.Equals( typeof( BackendlessUser ) ) )
        return "Users";
      else
        return type.Name;
    }
  }
}