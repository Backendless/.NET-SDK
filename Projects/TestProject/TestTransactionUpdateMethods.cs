using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;
using Weborb.Writer;

namespace TestProject
{
  [TestClass]
  public class TestTransactionUpdateMethods
  {
    [TestMethod]
    public void TestUpdateSingleObjectDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Dictionary<String, Object> res = new Dictionary<String, Object>();
      res[ "age" ] = 35;
      res[ "objectId" ] = Backendless.Data.Of("Person").FindFirst()["objectId"];
      unitOfWork.Update( "Person", res );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "updatePerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestUpdateMultipleObjectsDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<String> objectForChanges = new List<String>();

      objectForChanges.Add( (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ] );
      objectForChanges.Add( (String) Backendless.Data.Of( "Person" ).FindLast()[ "objectId" ] );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "ChangedNEXT";

      unitOfWork.BulkUpdate( "Person", objectForChanges, changes );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "updateBulkPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestUpdateSingleObjectsCLass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Person person = new Person();
      person.SetObjectId( (String) Backendless.Data.Of("Person").FindFirst()["objectId"] );
      person.SetName( "Tommy" );

      unitOfWork.Update( person );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "updatePerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestUpdateSingleObjectOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder dQB = DataQueryBuilder.Create();

      dQB.SetWhereClause( "age > 12" );
      dQB.SetPageSize( 1 );

      OpResult findResult = unitOfWork.Find( "Person", dQB );

      OpResultValueReference objectRef = findResult.ResolveTo( 0 );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 100500;

      unitOfWork.Update( objectRef, changes );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "updatePerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestUpdateMultipleObjectsOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder dataQueryBuilder = DataQueryBuilder.Create();

      dataQueryBuilder.SetWhereClause( "age > 15" );
      dataQueryBuilder.SetPageSize( 100 );

      OpResult PersonsResult = unitOfWork.Find( "Person", dataQueryBuilder );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "JOEEE";

      unitOfWork.BulkUpdate( PersonsResult, changes );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "updateBulkPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestUpdateMultipleObjectsQuery()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      String whereClause = "age = '11'";
      Dictionary<String, Object> changes = new Dictionary<String, Object>();

      changes[ "age" ] = 111;
      unitOfWork.BulkUpdate( "Person", whereClause, changes );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "updateBulkPerson1" ] ).Count == 2 );
    }
  }
}
