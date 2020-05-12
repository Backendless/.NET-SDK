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
  }
}
