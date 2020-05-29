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
  public class TestTransactionSetRelation
  {
    [TestMethod]
    public void TestSetRelationDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { (String) Backendless.Data.Of( "Order" ).FindFirst()[ "objectId" ] };

      String parentObjectId = (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ];
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.SetRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "setRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestSetRelationClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Order> gifts = new List<Order>();

      Order order = new Order();
      order.SetObjectId( (String) Backendless.Data.Of( "Order" ).FindFirst()[ "objectId" ] );

      gifts.Add( order );

      Person person = new Person();
      person.SetObjectId( (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ] );

      String relationColumn = "Surname";
      unitOfWork.SetRelation( person, relationColumn, gifts );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "setRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestSetRelationOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );

      OpResult gifts = unitOfWork.Find( "Order", queryBuilder );
      String personObjectId = (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ];
      String parentTable = "Person";
      String relationColumn = "Surname";

      unitOfWork.SetRelation( parentTable, personObjectId, relationColumn, gifts );
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "setRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestSetRelationWithId()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      String[] objIds = { (String) Backendless.Data.Of( "Order" ).FindFirst()[ "objectId" ] };

      String personObjectId = (String) Backendless.Data.Of( "Person" ).FindLast()[ "objectId" ];
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.SetRelation( parentTableName, personObjectId, relationColumnName, objIds );
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "setRelationPerson1" ] ).Count == 2 );
    }
  }
}
