using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject.Tests.Transaction
{
  [Collection( "Tests" )]
  public class TestTransactionFindMethods
  {
    [Fact]
    public void TestFind_AllObjects()
    {
      Person personCreated1 = new Person();
      personCreated1.age = 17;
      personCreated1.name = "Alexandra";

      Person personCreated2 = new Person();
      personCreated2.age = 24;
      personCreated2.name = "Joe";

      Backendless.Data.Of<Person>().Save( personCreated1 );
      Backendless.Data.Of<Person>().Save( personCreated2 );

      UnitOfWork uow = new UnitOfWork();
      OpResult opResultFindPerson = uow.Find( "Person", DataQueryBuilder.Create() );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      var results = (Dictionary<Object, Object>[]) uowResult.Results[ opResultFindPerson.OpResultId ].Result;
      Assert.True( results.Length == 2 );
      Assert.True( ( (Dictionary<Object, Object>) results[ 0 ] ).Count == 7 );

      Backendless.Data.Of( "Person" ).Remove( "age > '15'" );
    }

    [Fact]
    public void TestFind_CheckError()
    {
      UnitOfWork uow = new UnitOfWork();

      uow.Find( "Wrong table name", DataQueryBuilder.Create() );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.False( uowResult.Success );
      Assert.Null( uowResult.Results );
    }
  }
}