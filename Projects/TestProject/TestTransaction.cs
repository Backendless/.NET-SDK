using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransaction
  {
    [TestMethod]
    public void TestCreateSingleObject()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Dictionary<String, Object> order = new Dictionary<String, Object>();
      order[ "name" ] = "Joe";
      order[ "age" ] = 23;
      OpResult opResult = unitOfWork.Create( "Order", order );

      UnitOfWorkResult unitOfWorkRes = unitOfWork.Execute();
      Assert.IsTrue( true );
    }

    [TestMethod]
    public void TestDeleteSingleObject()
    {
      String objId = "43346BF5-B982-6C16-FF16-40618391DA00";
      UnitOfWork unitOfWork = new UnitOfWork();
      Dictionary<String, Object> order = new Dictionary<String, Object>();

      order[ "objectId" ] = objId;

      unitOfWork.Delete( "Order", order );

      UnitOfWorkResult unitOfWorkRes = unitOfWork.Execute();
      Assert.IsTrue( true );
    }

    [TestMethod]
    public void TestFindSingleObject()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      DataQueryBuilder dQB = DataQueryBuilder.Create();

      dQB.SetWhereClause( "name = 'Joe'" );
      dQB.SetPageSize( 1 );

      OpResult findObjectResult = unitOfWork.Find( "Order", dQB );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestUpdateSingleObject()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Dictionary<String, Object> res = new Dictionary<String, Object>();
      res[ "age" ] = 35;
      res["objectId"] = "43346BF5-B982-6C16-FF16-40618391DA00";
      unitOfWork.Update( "Order", res );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestCreateMultipleObjects()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Dictionary<String, Object>> people = new List<Dictionary<String, Object>>();

      Dictionary<String, Object> person1 = new Dictionary<String, Object>();
      person1["name"] = "Mary";
      person1["age"]  = 32;

      Dictionary<String, Object> person2 = new Dictionary<String, Object>();
      person2["name"] = "Bob";
      person2["age"] = 22;

      people.Add( person1 );
      people.Add( person2 );

      unitOfWork.BulkCreate( "Person", people );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestUpdateMultipleObjects()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<String> objectForChanges = new List<String>();

      objectForChanges.Add( "5596648F-1489-CAC5-FFAF-A1B037F7B500" );
      objectForChanges.Add( "D7FCBC10-F8A3-56A7-FF30-33FDCB404C00" );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "ChangedNEXT";

      unitOfWork.BulkUpdate( "Person", objectForChanges, changes );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteMultipleObjects()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<String> objectsToDelete = new List<String>();

      objectsToDelete.Add( "5596648F-1489-CAC5-FFAF-A1B037F7B500" );
      objectsToDelete.Add( "D7FCBC10-F8A3-56A7-FF30-33FDCB404C00" );

      unitOfWork.BulkDelete( "Person", objectsToDelete.ToArray() );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestAddRelation()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { "ECC44968-2639-A89A-FF70-5007DBAE4100" };

      String parentObjectId = "41483F08-2C22-69A3-FFDD-208F3BA5E200";
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.AddToRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestSetRelation()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { "980D0AC3-C63F-4AA5-FFDA-B2B65ED8CD00" };

      String parentObjectId = "41483F08-2C22-69A3-FFDD-208F3BA5E200";
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.SetRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteRelation()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { "980D0AC3-C63F-4AA5-FFDA-B2B65ED8CD00" };

      String parentObjectId = "41483F08-2C22-69A3-FFDD-208F3BA5E200";
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.DeleteRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      unitOfWork.Execute();
    }
  }
}
