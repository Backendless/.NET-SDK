﻿using BackendlessAPI.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  class TransactionHelper
  {
    private static String LAST_LOGIN_COLUM_NAME = "lastLogin";
    private static String PASSWORD_KEY = "password";
    private static String SOCIAL_ACCOUNT_COLUMN_NAME = "socialAccount";
    private static String USER_STATUS_COLUMN_NAME = "userStatus";

    static void RemoveSystemField( Dictionary<String, Object> changes )
    {
      changes.Remove( LAST_LOGIN_COLUM_NAME );
      changes.Remove( PASSWORD_KEY );
      changes.Remove( SOCIAL_ACCOUNT_COLUMN_NAME );
      changes.Remove( USER_STATUS_COLUMN_NAME );
      changes.Remove( "objectId" );
      changes.Remove( "created" );
      changes.Remove( "updated" );
    }

    static internal OpResult MakeOpResult( String tableName, String operationResultId, OperationType operationType )
    {
      return new OpResult( tableName, operationResultId, operationType );
    }

    static List<Object> ConvertMapsToObjectIds( List<Dictionary<String, Object>> objectMaps )
    {
      List<Object> objectIds = new List<Object>();

      foreach( Dictionary<String, Object> objectMap in objectMaps )
        objectIds.Add( ConvertObjectMapToObjectIdOrLeaveReference( objectMap ) );

      return objectIds;
    }
    
    internal static Dictionary<String, Object> ConvertInstanceToMaps<E>( E instance )
    {
      if( instance == null )
        throw new ArgumentException( ExceptionMessage.NULL_INSTANCE );

      Dictionary<String, Object> entity = new Dictionary<String, Object>();

      foreach( FieldInfo field in instance.GetType().GetFields( BindingFlags.Public ) )
      {
        Dictionary<String, Object> tempItem = new Dictionary<string, object>();
        entity[ field.Name ] = field.GetValue( instance );
      }

      return entity;
    }

    static String ConvertObjectMapToObjectId( Dictionary<String, Object> objectMap )
    {
      if( objectMap == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      Object maybeObjectId = objectMap[ "objectId" ];

      if( !( maybeObjectId is String ) )
        throw new ArgumentException( ExceptionMessage.NULL_OBJECT_ID_IN_OBJECT_MAP );

      return (String) maybeObjectId;
    }

    static Object ConvertObjectMapToObjectIdOrLeaveReference( Dictionary<String, Object> objectMap )
    {
      if( objectMap == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      if( objectMap.ContainsKey( UnitOfWork.REFERENCE_MARKER ) )
      {
        objectMap[ UnitOfWork.PROP_NAME ] = "objectId";
        return objectMap;
      }

      Object maybeObjectId = objectMap[ "objectId" ];
      if( !( maybeObjectId is String ) )
        throw new ArgumentException( ExceptionMessage.NULL_OBJECT_ID_IN_OBJECT_MAP );

      return maybeObjectId;
    }

    static Dictionary<String, Object> ConvertCreateBulkOrFindResultIndexToObjectId( OpResultValueReference parentObject )
    {
      Dictionary<String, Object> referenceToObjectId;

      if( OperationType.supportCollectionEntityDescriptionType.Contains( parentObject.GetOpResult().GetOperationType() ) )
        referenceToObjectId = parentObject.ResolveTo( "objectId" ).MakeReference();
      else if( OperationType.supportListIdsResultType.Contains( parentObject.GetOpResult().GetOperationType() ) )
        referenceToObjectId = parentObject.MakeReference();
      else
        throw new ArgumentException( ExceptionMessage.REF_TYPE_NOT_SUPPORT );

      return referenceToObjectId;
    }

    internal static void MakeReferenceToValueFromOpResult( Dictionary<String, Object> map )
    {
      foreach( KeyValuePair<String, Object> kvp in map )
      {
        Dictionary<String, Object> entry = new Dictionary<String, Object>( map );

        if( kvp.Value is OpResult )
          if( OperationType.supportIntResultType.Contains( ( (OpResult) kvp.Value ).GetOperationType() ) )
            entry[ kvp.Key ] = ( (OpResult) kvp.Value ).MakeReference();
          else
            throw new ArgumentException( ExceptionMessage.OP_RESULT_FROM_THIS_OPERATION_NOT_SUPPORT_IN_THIS_PLACE );

        if( kvp.Value is OpResultValueReference )
        {
          OpResultValueReference reference = (OpResultValueReference) kvp.Value;

          if( CreateUpdatePropName( reference ) || CreateBulkResultIndex( reference ) || FindPropNameResultIndex( reference ) )
            entry[ kvp.Key ] = reference.MakeReference();
          else
            throw new ArgumentException( ExceptionMessage.OP_RESULT_FROM_THIS_OPERATION_NOT_SUPPORT_IN_THIS_PLACE );      
        }
      }
    }

    public static void MakeReferenceToObjecIdtFromOpResult( List<Object> listObjectIds )
    {
      IEnumerator<Object> iterator = listObjectIds.GetEnumerator();

      while( iterator.MoveNext() )
      {
        Object temp = iterator.MoveNext();

        if( temp is OpResult )
          throw new ArgumentException( ExceptionMessage.OP_RESULT_FROM_THIS_OPERATION_NOT_SUPPORT_IN_THIS_PLACE );

        if( temp is OpResultValueReference )
        {
          OpResultValueReference reference = (OpResultValueReference) temp;

          if( CreateUpdateObjectId( reference ) || CreateBulkResultIndex( reference ) || FindPropNameResultIndex( reference ) )
            temp = reference.MakeReference();
          else
            throw new ArgumentException( ExceptionMessage.OP_RESULT_FROM_THIS_OPERATION_NOT_SUPPORT_IN_THIS_PLACE );
        }
      }
    }

    private static Boolean CreateUpdateObjectId( OpResultValueReference reference )
    {
      return CreateUpdatePropName( reference ) && reference.GetPropName().Equals( "objectId" );
    }

    private static Boolean CreateUpdatePropName( OpResultValueReference reference )
    {
      return OperationType.supportEntityDescriptionResultType.Contains( reference.GetOpResult().GetOperationType() ) &&
                                                                      reference.GetPropName() == null &&
                                                                      reference.GetResultIndex() != null;
    }

    private static Boolean CreateBulkResultIndex( OpResultValueReference reference )
    {
      return OperationType.CREATE_BULK.Equals( reference.GetOpResult().GetOperationType() ) &&
            reference.GetPropName() == null &&
            reference.GetResultIndex() != null;
    }

    private static Boolean FindResultIndexObjectId( OpResultValueReference reference )
    {
      return FindPropNameResultIndex( reference ) && reference.GetPropName().Equals( "objectId" );
    }

    private static Boolean FindPropNameResultIndex( OpResultValueReference reference )
    {
      return OperationType.FIND.Equals( reference.GetOpResult().GetOperationType() ) &&
            reference.GetPropName() != null &&
            reference.GetResultIndex() != null;
    }

  }
}
