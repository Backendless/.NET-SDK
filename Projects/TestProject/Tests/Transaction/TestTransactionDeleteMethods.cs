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
  public class TestTransactionDeleteMethods
  {
    [Fact]
    public void TestDeleteSingleObject_Class()
    {
      Person personObj = new Person();
      personObj.age = 30;
      personObj.name = "Alexandra";
      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      UnitOfWork uow = new UnitOfWork();
      uow.Delete( personObj );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
      Assert.True( listCheckPerson.Count == 0 );
    }

    [Fact]
    public void TestDeleteSingleObject_ClassCallback()
    {
      Person personObj = new Person();
      personObj.age = 30;
      personObj.name = "Alexandra";
      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      UnitOfWork uow = new UnitOfWork();
      uow.Delete( personObj );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
        Assert.True( listCheckPerson.Count == 0 );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public void TestDeleteSingleObject_Dictionary()
    {
      Dictionary<String, Object> defaultObject = new Dictionary<String, Object>();
      defaultObject[ "name" ] = "Joe";
      defaultObject[ "age" ] = 28;
      defaultObject[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( defaultObject )[ "objectId" ];

      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Person", (String) defaultObject[ "objectId" ] );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      IList<Dictionary<String, Object>> personMaps = Backendless.Data.Of( "Person" ).Find();
      Assert.True( personMaps.Count == 0 );
    }

    [Fact]
    public void TestDeleteSingleObject_DictionaryCallback()
    {
      Dictionary<String, Object> defaultObject = new Dictionary<String, Object>();
      defaultObject[ "name" ] = "Joe";
      defaultObject[ "age" ] = 28;
      defaultObject[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( defaultObject )[ "objectId" ];

      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Person", (String) defaultObject[ "objectId" ] );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        IList<Dictionary<String, Object>> personMaps = Backendless.Data.Of( "Person" ).Find();
        Assert.True( personMaps.Count == 0 );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public void TestDeleteSingleObject_OpResult()
    {
      Person personObject = new Person();
      personObject.name = "Bob";
      personObject.age = 23;
      personObject.objectId = Backendless.Data.Of<Person>().Save( personObject ).objectId;

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "age = '23'" );

      UnitOfWork uow = new UnitOfWork();
      OpResult opResult = uow.Find( "Person", queryBuilder );
      OpResultValueReference firstInvalid = opResult.ResolveTo( 0 );
      uow.Delete( firstInvalid );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
      Assert.True( listCheckPerson.Count == 0 );
    }

    [Fact]
    public void TestDeleteSingleObject_OpResultCallback()
    {
      Person personObject = new Person();
      personObject.name = "Bob";
      personObject.age = 23;
      personObject.objectId = Backendless.Data.Of<Person>().Save( personObject ).objectId;

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "age = '23'" );

      UnitOfWork uow = new UnitOfWork();
      OpResult opResult = uow.Find( "Person", queryBuilder );
      OpResultValueReference firstInvalid = opResult.ResolveTo( 0 );
      uow.Delete( firstInvalid );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
        Assert.True( listCheckPerson.Count == 0 );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
        
    }

    [Fact]
    public void TestDeleteSingleObject_WithId()
    {
      Person defaultPersonObject = new Person();
      defaultPersonObject.age = 20;
      defaultPersonObject.name = "John";
      defaultPersonObject.objectId = Backendless.Data.Of<Person>().Save( defaultPersonObject ).objectId;

      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Person", defaultPersonObject.objectId );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
      Assert.True( listCheckPerson.Count == 0 );
    }

    [Fact]
    public void TestDeleteSingleObject_WithIdCallback()
    {
      Person defaultPersonObject = new Person();
      defaultPersonObject.age = 20;
      defaultPersonObject.name = "John";
      defaultPersonObject.objectId = Backendless.Data.Of<Person>().Save( defaultPersonObject ).objectId;

      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Person", defaultPersonObject.objectId );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
        Assert.True( listCheckPerson.Count == 0 );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public void TestDeleteSingleObject_CheckError()
    {
      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Wrong table name", "Empty objectId" );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.False( uowResult.Success );
      Assert.Null( uowResult.Results );
    }

    [Fact]
    public void TestDeleteSingleObject_CheckErrorCallback()
    {
      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Wrong table name", "Empty objectId" );
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