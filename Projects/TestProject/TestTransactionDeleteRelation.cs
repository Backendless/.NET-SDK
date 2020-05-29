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
  public class TestTransactionDeleteRelation
  {
    [TestMethod]
    public void TestDeleteRelationDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { "5E42E354-418F-60D1-FF5F-94998EC5D500" };

      String parentObjectId = "8B329DB4-46DB-03A1-FF1A-6828DF619400";
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.DeleteRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteRelationClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Order> gifts = new List<Order>();

      Order order = new Order();
      order.SetObjectId( "D4C84730-5082-6CE1-FF10-D56534CECE00" );

      gifts.Add( order );

      Person person = new Person();
      person.SetObjectId( "28237548-69DC-A0FD-FFBD-81D043E97D00" );
      String relationColumn = "Surname";
      unitOfWork.DeleteRelation( person, relationColumn, gifts );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteRelationOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );

      OpResult gifts = unitOfWork.Find( "Order", queryBuilder );
      String personObjectId = "93957B42-9B1A-1CCE-FFAD-ADE44D459A00";
      String parentTable = "Person";
      String relationColumn = "Surname";

      unitOfWork.DeleteRelation( parentTable, personObjectId, relationColumn, gifts );
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteRelationWithId()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      String[] objToDelRel = { "5E42E354-418F-60D1-FF5F-94998EC5D500" };

      String personObjId = "BE23F5DA-7431-F64B-FF8B-B6176463C400";
      String parentTable = "Person";
      String relationColumn = "Surname";

      unitOfWork.DeleteRelation( parentTable, personObjId, relationColumn, objToDelRel );
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteRelationPerson1" ] ).Count == 2 );
    }
  }
}
