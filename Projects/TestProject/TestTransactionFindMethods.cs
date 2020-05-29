using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;
using BackendlessAPI.Service;
using Weborb.Writer;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace TestProject
{
  [TestClass]
  public class TestTransactionFindMethods
  {
    [TestMethod]
    public void TestFindSingleObjectDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      DataQueryBuilder dQB = DataQueryBuilder.Create();

      dQB.SetPageSize( 1 );

      OpResult findObjectResult = unitOfWork.Find( "Person", dQB );

      OpResultValueReference gg = findObjectResult.ResolveTo( 0 );


      UnitOfWorkResult result = unitOfWork.Execute();

      Assert.IsTrue( ((Dictionary<Object, Object>) result.Results["findPerson1"]).Count == 2 );
    }
  }
}
