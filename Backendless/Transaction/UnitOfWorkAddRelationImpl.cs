using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendlessAPI.Transaction.Operations;
namespace BackendlessAPI.Transaction
{
  class UnitOfWorkAddRelationImpl : UnitOfWorkAddRelation
  {
    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, string[] childrenObjectIds )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E>( string parentTable, Dictionary<string, object> parentObject, string columnName, E[] childrenInstance )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, OpResult children )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, string whereClauseForChildren )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, string[] childrenObjectIds )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E>( string parentTable, string parentObjectId, string columnName, E[] childrenInstances )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, OpResult children )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, string whereClauseForChildren )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, string[] childrenObjectIds )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E, U>( E parentObject, string columnName, U[] childrenInstances )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, OpResult children )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, string whereClauseForChildren )
    {
      throw new NotImplementedException();
    }

    public OpResult addToRelation( OpResult parentObject, string columnName, string[] childrenObjectIds )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E>( OpResult parentObject, string columnName, E[] childrenInstances )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, OpResult children )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, string whereClauseForChildren )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, string[] childrenObjectIds )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation<E>( OpResultValueReference parentObject, string columnName, E[] childrenInstances )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, OpResult children )
    {
      throw new NotImplementedException();
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, string whereClauseForChildren )
    {
      throw new NotImplementedException();
    }
  }
}
