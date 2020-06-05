using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;
using BackendlessAPI.Async;

namespace TestProject
{
  [TestClass]
  public class TestTransactionFindMethods
  {
    [TestMethod]
    public void TestFind_AllObjects()
    {
      List<Person> listPerson = new List<Person>();
      Person personCreated1 = new Person();
      personCreated1.age = 17;
      personCreated1.name = "Alexandra";

      Person personCreated2 = new Person();
      personCreated2.age = 24;
      personCreated2.name = "Joe";

      listPerson.Add( personCreated1 );
      listPerson.Add( personCreated2 );
      IList<String> objectIds = Backendless.Data.Of<Person>().Create( listPerson );

      UnitOfWork uow = new UnitOfWork();
      OpResult opResultCreateBulkPerson = uow.BulkCreate( listPerson );
      OpResult opResultFindPerson = uow.Find( "Person", DataQueryBuilder.Create() );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Dictionary<String, OperationResult> results = uowResult.Results;
      Assert.IsTrue( 2 == results.Count );

      Dictionary<Object, Object>[] resultFind = (Dictionary<Object, Object>[]) results[ opResultFindPerson.GetOpResultId() ].Result;
      Assert.IsTrue( 4 == resultFind.Length );

      Backendless.Data.Of( "Person" ).Remove( "age > '15'" );
    }

    [TestMethod]
    public void TestFind_CheckError()
    {
      UnitOfWork uow = new UnitOfWork();
      uow.Find( "Wrong table name", DataQueryBuilder.Create() );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsFalse( uowResult.Success );
      Assert.IsNull( uowResult.Results );
    }
  }
}
