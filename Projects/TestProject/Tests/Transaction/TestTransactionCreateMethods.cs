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
  public class TestTransactionCreateMethods
  {
    [Fact]
    public void TestCreateSingleObject_Dictionary()
    {
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;

      UnitOfWork uow = new UnitOfWork();
      OpResult createPersonResult = uow.Create( "Person", pers );
      UnitOfWorkResult unitOfWorkRes = uow.Execute();

      Assert.True( unitOfWorkRes.Success );
      Assert.NotNull( unitOfWorkRes.Results );

      Dictionary<String, OperationResult> result = unitOfWorkRes.Results;
      OperationResult operationResult = result[ createPersonResult.OpResultId ];

      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.True( "Joe" == (String) transactionResult[ "name" ] );
      Assert.True( 23 == (Int32) transactionResult[ "age" ] );

      Backendless.Data.Of( "Person" ).Remove( "name = 'Joe'" );
    }

    [Fact]
    public void TestCreateSingleObject_Dictionary_Callback()
    {
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;

      UnitOfWork uow = new UnitOfWork();
      OpResult createPersonResult = uow.Create( "Person", pers );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      unitOfWorkRes=>
      {

        Assert.True( unitOfWorkRes.Success );
        Assert.NotNull( unitOfWorkRes.Results );

        Dictionary<String, OperationResult> result = unitOfWorkRes.Results;
        OperationResult operationResult = result[ createPersonResult.OpResultId ];

        Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

        Assert.True( "Joe" == (String) transactionResult[ "name" ] );
        Assert.True( 23 == (Int32) transactionResult[ "age" ] );

        Backendless.Data.Of( "Person" ).Remove( "name = 'Joe'" );
      },
      fault=>
      {
        Assert.True( false, "Something went wrong during the execution operation" );
      } ) );

    }

    [Fact]
    public void TestCreateSingleObject_Class()
    {
      Person person = new Person();
      person.name = "Joe";
      person.age = 30;

      UnitOfWork unitOfWork = new UnitOfWork();
      OpResult addPersonResult = unitOfWork.Create( person );
      UnitOfWorkResult uowResult = unitOfWork.Execute();
      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Person personObject = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() )[ 0 ];
      Assert.True( person.age == personObject.age );
      Assert.True( person.name == personObject.name );

      Backendless.Data.Of( "Person" ).Remove( "name = '" + personObject.name + "'" );
    }

    [Fact]
    public void TestCreateSingleObject_Class_Callback()
    {
      Person person = new Person();
      person.name = "Joe";
      person.age = 30;

      UnitOfWork unitOfWork = new UnitOfWork();
      OpResult addPersonResult = unitOfWork.Create( person );
      unitOfWork.Execute(new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );
        Person personObject = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() )[ 0 ];
        Assert.True( person.age == personObject.age );
        Assert.True( person.name == personObject.name );

        Backendless.Data.Of( "Person" ).Remove( "name = '" + personObject.name + "'" );
      },
      fault=>
      {
        Assert.True( false, "Something went wrong during the execution operation" );
      } ) );
    }

    [Fact]
    public void TestCreateSingleObject_CheckError()
    {
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;

      UnitOfWork uow = new UnitOfWork();
      uow.Create( "NonexistentTable", pers );
      UnitOfWorkResult unitOfWorkRes = uow.Execute();

      Assert.False( unitOfWorkRes.Success );
      Assert.Null( unitOfWorkRes.Results );
    }

    [Fact]
    public void TestCreateSingleObject_CheckError_Callback()
    {
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;

      UnitOfWork uow = new UnitOfWork();
      uow.Create( "NonexistentTable", pers );
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