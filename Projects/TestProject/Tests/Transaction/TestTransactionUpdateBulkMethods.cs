using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject.Tests.Transaction
{
  [Collection( "Tests" )]
  public class TestTransactionUpdateBulkMethods : IDisposable
  {
    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestUpdateBulkObjects_CheckError()
    {
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "Joe";

      UnitOfWork uow = new UnitOfWork();
      uow.BulkUpdate( "Wrong table name", "name != 'Joe'", changes );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.False( uowResult.Success );
      Assert.Null( uowResult.Results );
    }

    [Fact]
    public void TestUpdateBulkObjects_CheckError_Callback()
    {
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "Joe";

      UnitOfWork uow = new UnitOfWork();
      uow.BulkUpdate( "Wrong table name", "name != 'Joe'", changes );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.False( uowResult.Success );
        Assert.Null( uowResult.Results );
      },
      fault =>
      {
        Assert.True( false, "An error occured during the operation" );
      } ) );
    }

    [Fact]
    public void TestUpdateBulkObjects_Query()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> firstObject = new Dictionary<String, Object>();
      Dictionary<String, Object> secondObject = new Dictionary<String, Object>();

      firstObject[ "age" ] = 13;
      secondObject[ "age" ] = 12;
      objectsForCreate.Add( firstObject );
      objectsForCreate.Add( secondObject );
      Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      UnitOfWork uow = new UnitOfWork();
      String whereClause = "age > '11'";
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 111;
      OpResult updatePerson = uow.BulkUpdate( "Person", whereClause, changes );

      UnitOfWorkResult uowResult = uow.Execute();
      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Double transactionResult = (Double) operationResult.Result;

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

      Assert.True( transactionResult == (Double) objectsForCreate.Count );
      Assert.Null( personList[ 0 ].name );
      Assert.Null( personList[ 1 ].name );
      Assert.True( (Int32?) personList[ 0 ].age == 111 );
      Assert.True( (Int32?) personList[ 1 ].age == 111 );
    }

    [Fact]
    public void TestUpdateBulkObjects_Query_Callback()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> firstObject = new Dictionary<String, Object>();
      Dictionary<String, Object> secondObject = new Dictionary<String, Object>();

      firstObject[ "age" ] = 13;
      secondObject[ "age" ] = 12;
      objectsForCreate.Add( firstObject );
      objectsForCreate.Add( secondObject );
      Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      UnitOfWork uow = new UnitOfWork();
      String whereClause = "age > '11'";
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 111;
      OpResult updatePerson = uow.BulkUpdate( "Person", whereClause, changes );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        Dictionary<String, OperationResult> result = uowResult.Results;
        OperationResult operationResult = result[ updatePerson.OpResultId ];
        Double transactionResult = (Double) operationResult.Result;

        IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

        Assert.True( transactionResult == (Double) objectsForCreate.Count );
        Assert.Null( personList[ 0 ].name );
        Assert.Null( personList[ 1 ].name );
        Assert.True( (Int32?) personList[ 0 ].age == 111 );
        Assert.True( (Int32?) personList[ 1 ].age == 111 );
      },
      fault =>
      {
        Assert.True( false, "An error occured during the operation" );
      } ) );
    }

    [Fact]
    public void TestUpdateBulkObjects_OpResult()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> objectFirst = new Dictionary<String, Object>();
      Dictionary<String, Object> objectSecond = new Dictionary<String, Object>();
      objectFirst[ "name" ] = "Joe";
      objectSecond[ "name" ] = "Joe";
      objectsForCreate.Add( objectFirst );
      objectsForCreate.Add( objectSecond );
      Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      DataQueryBuilder dataQueryBuilder = DataQueryBuilder.Create();
      dataQueryBuilder.SetWhereClause( "name = 'Joe'" );

      UnitOfWork uow = new UnitOfWork();
      OpResult personsResult = uow.Find( "Person", dataQueryBuilder );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "JOEEE";

      OpResult updatePerson = uow.BulkUpdate( personsResult, changes );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Double transactionResult = (Double) operationResult.Result;

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

      Assert.True( transactionResult == (Double) objectsForCreate.Count );
      Assert.Null( personList[ 0 ].age );
      Assert.Null( personList[ 1 ].age );
      Assert.True( (String) personList[ 0 ].name == "JOEEE" );
      Assert.True( (String) personList[ 1 ].name == "JOEEE" );

      Backendless.Data.Of( "Person" ).Remove( "name = 'JOEEE'" );
    }

    [Fact]
    public void TestUpdateBulkObjects_OpResult_Callback()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> objectFirst = new Dictionary<String, Object>();
      Dictionary<String, Object> objectSecond = new Dictionary<String, Object>();
      objectFirst[ "name" ] = "Joe";
      objectSecond[ "name" ] = "Joe";
      objectsForCreate.Add( objectFirst );
      objectsForCreate.Add( objectSecond );
      Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      DataQueryBuilder dataQueryBuilder = DataQueryBuilder.Create();
      dataQueryBuilder.SetWhereClause( "name = 'Joe'" );

      UnitOfWork uow = new UnitOfWork();
      OpResult personsResult = uow.Find( "Person", dataQueryBuilder );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "JOEEE";

      OpResult updatePerson = uow.BulkUpdate( personsResult, changes );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        Dictionary<String, OperationResult> result = uowResult.Results;
        OperationResult operationResult = result[ updatePerson.OpResultId ];
        Double transactionResult = (Double) operationResult.Result;

        IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

        Assert.True( transactionResult == (Double) objectsForCreate.Count );
        Assert.Null( personList[ 0 ].age );
        Assert.Null( personList[ 1 ].age );
        Assert.True( (String) personList[ 0 ].name == "JOEEE" );
        Assert.True( (String) personList[ 1 ].name == "JOEEE" );

        Backendless.Data.Of( "Person" ).Remove( "name = 'JOEEE'" );
      },
      fault =>
      {
        Assert.True( false, "An error occured during the operation" );
      } ) );
    }

    [Fact]
    public void TestUpdateBulkObjects_Dictionary()
    {
      Dictionary<String, Object> objData_First = new Dictionary<String, Object>();
      Dictionary<String, Object> objData_Second = new Dictionary<String, Object>();
      List<Dictionary<String, Object>> listPerson = new List<Dictionary<String, Object>>();

      objData_First[ "age" ] = 19;
      objData_Second[ "age" ] = 25;
      listPerson.Add( objData_First );
      listPerson.Add( objData_Second );
      List<String> objectForChanges = (List<String>) Backendless.Data.Of( "Person" ).Create( listPerson );

      UnitOfWork uow = new UnitOfWork();
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 100;

      OpResult updatePersonsObj = uow.BulkUpdate( "Person", objectForChanges, changes );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePersonsObj.OpResultId ];
      Double transactionResult = (Double) operationResult.Result;
      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

      Assert.True( transactionResult == (Double) personList.Count );
      Assert.Null( personList[ 0 ].name );
      Assert.Null( personList[ 1 ].name );
      Assert.True( personList[ 0 ].age == 100 );
      Assert.True( personList[ 1 ].age == 100 );
    }

    [Fact]
    public void TestUpdateBulkObjects_Dictionary_Callback()
    {
      Dictionary<String, Object> objData_First = new Dictionary<String, Object>();
      Dictionary<String, Object> objData_Second = new Dictionary<String, Object>();
      List<Dictionary<String, Object>> listPerson = new List<Dictionary<String, Object>>();

      objData_First[ "age" ] = 19;
      objData_Second[ "age" ] = 25;
      listPerson.Add( objData_First );
      listPerson.Add( objData_Second );
      List<String> objectForChanges = (List<String>) Backendless.Data.Of( "Person" ).Create( listPerson );

      UnitOfWork uow = new UnitOfWork();
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 100;

      OpResult updatePersonsObj = uow.BulkUpdate( "Person", objectForChanges, changes );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        Dictionary<String, OperationResult> result = uowResult.Results;
        OperationResult operationResult = result[ updatePersonsObj.OpResultId ];
        Double transactionResult = (Double) operationResult.Result;
        IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

        Assert.True( transactionResult == (Double) personList.Count );
        Assert.Null( personList[ 0 ].name );
        Assert.Null( personList[ 1 ].name );
        Assert.True( personList[ 0 ].age == 100 );
        Assert.True( personList[ 1 ].age == 100 );
      },
      fault =>
      {
        Assert.True( false, "An error occured, during the operation" );
      } ) );
    }
  }
}