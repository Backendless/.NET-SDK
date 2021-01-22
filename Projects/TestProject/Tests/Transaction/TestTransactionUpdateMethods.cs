using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using BackendlessAPI.Transaction;
using System.Collections.Generic;

namespace TestProject.Tests.Transaction
{
  [Collection( "Tests" )]
  public class TestTransactionUpdateMethods : IDisposable
  {
    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestUpdateSingleObject_Dictionary()
    {
      Dictionary<String, Object> objData = new Dictionary<String, Object>();
      objData[ "age" ] = 17;
      Backendless.Data.Of( "Person" ).Save( objData );

      objData.Clear();
      objData[ "age" ] = 35;
      objData[ "objectId" ] = Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ];

      UnitOfWork uow = new UnitOfWork();
      OpResult updatePerson = uow.Update( "Person", objData );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.Null( transactionResult[ "name" ] );
      Assert.True( Comparer.IsEqual( objData[ "age" ], transactionResult[ "age" ] ) );
    }

    [Fact]
    public void TestUpdateSingleObject_Dictionary_Callback()
    {
      Dictionary<String, Object> objData = new Dictionary<String, Object>();
      objData[ "age" ] = 17;
      Backendless.Data.Of( "Person" ).Save( objData );

      objData.Clear();
      objData[ "age" ] = 35;
      objData[ "objectId" ] = Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ];

      UnitOfWork uow = new UnitOfWork();
      OpResult updatePerson = uow.Update( "Person", objData );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        Dictionary<String, OperationResult> result = uowResult.Results;
        OperationResult operationResult = result[ updatePerson.OpResultId ];
        Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

        Assert.Null( transactionResult[ "name" ] );
        Assert.True( Comparer.IsEqual( objData[ "age" ], transactionResult[ "age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public void TestUpdateSingleObject_Class()
    {
      Person person = new Person();
      person.name = "Joe";
      person.age = 20;
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;
      person.name = "Tommy";

      UnitOfWork unitOfWork = new UnitOfWork();
      OpResult updatePerson = unitOfWork.Update( person );
      UnitOfWorkResult uowResult = unitOfWork.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.True( (String) transactionResult[ "name" ] == "Tommy" );
      Assert.True( (String) transactionResult[ "objectId" ] == person.objectId );
    }

    [Fact]
    public void TestUpdateSingleObject_Class_Callback()
    {
      Person person = new Person();
      person.name = "Joe";
      person.age = 20;
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;
      person.name = "Tommy";

      UnitOfWork unitOfWork = new UnitOfWork();
      OpResult updatePerson = unitOfWork.Update( person );
      unitOfWork.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        Dictionary<String, OperationResult> result = uowResult.Results;
        OperationResult operationResult = result[ updatePerson.OpResultId ];
        Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

        Assert.True( (String) transactionResult[ "name" ] == "Tommy" );
        Assert.True( (String) transactionResult[ "objectId" ] == person.objectId );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public void TestUpdateSingleObject_OpResultValueRef_Dictionary()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> objectData = new Dictionary<String, Object>();
      Dictionary<String, Object> wrongObject = new Dictionary<String, Object>();

      wrongObject[ "age" ] = 1212;
      objectData[ "age" ] = 12;
      objectsForCreate.Add( objectData );
      objectsForCreate.Add( wrongObject );
      IList<String> createResult = Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      DataQueryBuilder dQB = DataQueryBuilder.Create();
      dQB.SetWhereClause( "age = 12" );
      dQB.SetPageSize( 1 );

      UnitOfWork uow = new UnitOfWork();
      OpResult findResult = uow.Find( "Person", dQB );
      OpResultValueReference objectRef = findResult.ResolveTo( 0 );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 100500;

      OpResult updatePerson = uow.Update( objectRef, changes );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.True( Comparer.IsEqual( transactionResult[ "age" ], changes[ "age" ] ) );
      Assert.Equal( transactionResult[ "objectId" ], createResult[ 0 ] );
    }

    [Fact]
    public void TestUpdateSingleObject_OpResultValueRef_Dictionary_Callback()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> objectData = new Dictionary<String, Object>();
      Dictionary<String, Object> wrongObject = new Dictionary<String, Object>();

      wrongObject[ "age" ] = 1212;
      objectData[ "age" ] = 12;
      objectsForCreate.Add( objectData );
      objectsForCreate.Add( wrongObject );
      IList<String> createResult = Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      DataQueryBuilder dQB = DataQueryBuilder.Create();
      dQB.SetWhereClause( "age = 12" );
      dQB.SetPageSize( 1 );

      UnitOfWork uow = new UnitOfWork();
      OpResult findResult = uow.Find( "Person", dQB );
      OpResultValueReference objectRef = findResult.ResolveTo( 0 );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 100500;

      OpResult updatePerson = uow.Update( objectRef, changes );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        Dictionary<String, OperationResult> result = uowResult.Results;
        OperationResult operationResult = result[ updatePerson.OpResultId ];
        Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

        Assert.True( Comparer.IsEqual( transactionResult[ "age" ], changes[ "age" ] ) );
        Assert.Equal( transactionResult[ "objectId" ], createResult[ 0 ] );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public void TestUpdateSingleObject_CheckError()
    {
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "Joee";
      changes[ "age" ] = 18;

      UnitOfWork uow = new UnitOfWork();
      uow.Update( "Wrong table name", changes );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.False( uowResult.Success );
      Assert.Null( uowResult.Results );
    }

    [Fact]
    public void TestUpdateSingleObject_CheckError_Callback()
    {
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "Joee";
      changes[ "age" ] = 18;

      UnitOfWork uow = new UnitOfWork();
      uow.Update( "Wrong table name", changes );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.False( uowResult.Success );
        Assert.Null( uowResult.Results );
      },
      fault =>
      {
        Assert.True( false, "An error was expected, but it was not" );
      } ) );
    }
  }
}