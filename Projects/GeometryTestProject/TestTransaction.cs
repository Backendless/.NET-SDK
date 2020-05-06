using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace GeometryTestProject
{
  [TestClass]
  public class TestTransaction
  {
    [TestMethod]
    public void TestCreate()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Dictionary<String, Object> order = new Dictionary<String, Object>();
      order[ "name" ] = "Joe";
      order[ "age" ] = 23;
      OpResult opResult = unitOfWork.Create( "Order", order );

      UnitOfWorkResult unitOfWorkRes = unitOfWork.Execute();
      
    }
  }
}
