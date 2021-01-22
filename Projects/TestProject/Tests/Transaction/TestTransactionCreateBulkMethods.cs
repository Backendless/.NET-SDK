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
  public class TestTransactionCreateBulkMethods
  {
    [Fact]
    public void TestCreateBulkObjects_Dictionary()
    {
      List<Dictionary<String, Object>> people = new List<Dictionary<String, Object>>();

      Dictionary<String, Object> person1 = new Dictionary<String, Object>();
      person1[ "name" ] = "Mary";
      person1[ "age" ] = 32;

      Dictionary<String, Object> person2 = new Dictionary<String, Object>();
      person2[ "name" ] = "Bob";
      person2[ "age" ] = 22;

      people.Add( person1 );
      people.Add( person2 );

      UnitOfWork uow = new UnitOfWork();
      OpResult createPersonsObj = uow.BulkCreate( "Person", people );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );
      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ createPersonsObj.OpResultId ];
      String[] transactionsObjID = (String[]) operationResult.Result;

      int iteratorI = 0;
      int iteratorJ = 1;
      if( transactionsObjID[ 0 ] != personList[ 1 ].objectId )
      {
        iteratorI++;
        iteratorJ--;
      }
      Assert.True( transactionsObjID[ iteratorI ] == personList[ 1 ].objectId );
      Assert.True( transactionsObjID[ iteratorJ ] == personList[ 0 ].objectId );
      Assert.True( personList[ iteratorJ ].age == (Int32) person1[ "age" ] );
      Assert.True( personList[ iteratorI ].age == (Int32) person2[ "age" ] );
      Assert.True( personList[ iteratorJ ].name == (String) person1[ "name" ] );
      Assert.True( personList[ iteratorI ].name == (String) person2[ "name" ] );

      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestCreateBulkObjects_Dictionary_Callback()
    {
      List<Dictionary<String, Object>> people = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> person1 = new Dictionary<String, Object>();
      Dictionary<String, Object> person2 = new Dictionary<String, Object>();

      person1[ "name" ] = "Mary";
      person1[ "age" ] = 32;
      person2[ "name" ] = "Bob";
      person2[ "age" ] = 22;
      people.Add( person1 );
      people.Add( person2 );

      UnitOfWork uow = new UnitOfWork();
      OpResult createPersonsObj = uow.BulkCreate( "Person", people );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );
        Dictionary<String, OperationResult> result = uowResult.Results;
        OperationResult operationResult = result[ createPersonsObj.OpResultId ];
        String[] transactionsObjID = (String[]) operationResult.Result;

        int iteratorI = 0;
        int iteratorJ = 1;
        if( transactionsObjID[ 0 ] != personList[ 1 ].objectId )
        {
          iteratorI++;
          iteratorJ--;
        }
        Assert.True( transactionsObjID[ iteratorI ] == personList[ 1 ].objectId );
        Assert.True( transactionsObjID[ iteratorJ ] == personList[ 0 ].objectId );
        Assert.True( personList[ iteratorJ ].age == (Int32) person1[ "age" ] );
        Assert.True( personList[ iteratorI ].age == (Int32) person2[ "age" ] );
        Assert.True( personList[ iteratorJ ].name == (String) person1[ "name" ] );
        Assert.True( personList[ iteratorI ].name == (String) person2[ "name" ] );

        Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [ Fact]
    public void TestCreateBulkObjects_Class()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Person> people = new List<Person>();

      Person people1 = new Person();
      people1.age = 11;
      people1.name = "Eleven";

      Person people2 = new Person();
      people2.name = "Twelve";
      people2.age = 12;

      people.Add( people1 );
      people.Add( people2 );

      OpResult createPersonObjects = unitOfWork.BulkCreate( people );
      UnitOfWorkResult uowResult = unitOfWork.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );
      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ createPersonObjects.OpResultId ];
      String[] transactionsObjID = (String[]) operationResult.Result;

      Assert.True( transactionsObjID[ 0 ] == personList[ 0 ].objectId || transactionsObjID[ 0 ] == personList[ 1 ].objectId );
      Assert.True( transactionsObjID[ 1 ] == personList[ 0 ].objectId || transactionsObjID[ 1 ] == personList[ 1 ].objectId );
      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestCreateBulkObjects_Class_Callback()
    {
      List<Person> people = new List<Person>();
      Person people1 = new Person();
      Person people2 = new Person();

      people1.age = 11;
      people1.name = "Eleven";
      people2.name = "Twelve";
      people2.age = 12;
      people.Add( people1 );
      people.Add( people2 );

      UnitOfWork unitOfWork = new UnitOfWork();
      OpResult createPersonObjects = unitOfWork.BulkCreate( people );
      unitOfWork.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );
        Dictionary<String, OperationResult> result = uowResult.Results;
        OperationResult operationResult = result[ createPersonObjects.OpResultId ];
        String[] transactionsObjID = (String[]) operationResult.Result;

        Assert.True( transactionsObjID[ 0 ] == personList[ 0 ].objectId || transactionsObjID[ 0 ] == personList[ 1 ].objectId );
        Assert.True( transactionsObjID[ 1 ] == personList[ 0 ].objectId || transactionsObjID[ 1 ] == personList[ 1 ].objectId );
        Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
      },
      fault=>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public void TestCreateBulkObjects_CheckError()
    {
      List<Dictionary<String, Object>> persMap = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;
      persMap.Add( pers );

      UnitOfWork uow = new UnitOfWork();
      uow.BulkCreate( "Wrong table name", persMap );
      UnitOfWorkResult unitOfWorkRes = uow.Execute();

      Assert.False( unitOfWorkRes.Success );
      Assert.Null( unitOfWorkRes.Results );
    }

    [Fact]
    public void TestCreateBulkdObjects_CheckError_Callback()
    {
      List<Dictionary<String, Object>> persMap = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;
      persMap.Add( pers );

      UnitOfWork uow = new UnitOfWork();
      uow.BulkCreate( "Wrong table name", persMap );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      unitOfWorkRes =>
      {
        Assert.False( unitOfWorkRes.Success );
        Assert.Null( unitOfWorkRes.Results );
      },
      fault =>
      {
        Assert.True( false, "An error was expected, but is was not" );
      } ) );
    }
  }
}