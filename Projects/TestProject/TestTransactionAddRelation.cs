using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;
using Weborb.Writer;
using System.Reflection.PortableExecutable;

namespace TestProject
{
  [TestClass]
  public class TestTransactionAddRelation
  {
    [TestMethod]
    public void TestAddRelationDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { (String) Backendless.Data.Of( "Order" ).FindFirst()[ "objectId" ] };

      String parentObjectId = (String) Backendless.Data.Of( "Person" ).FindById( "8B329DB4-46DB-03A1-FF1A-6828DF619400" )[ "objectId" ];
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.AddToRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "addToRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestAddRelationClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Order> gifts = new List<Order>();

      Order order = new Order();
      order.SetObjectId( (String) Backendless.Data.Of( "Order" ).FindFirst()[ "objectId" ] );

      gifts.Add( order );

      Person person = new Person();
      person.SetObjectId( (String) Backendless.Data.Of( "Person" ).FindById( "28237548-69DC-A0FD-FFBD-81D043E97D00" )[ "objectId" ] );

      String relationColumn = "Surname";
      unitOfWork.AddToRelation( person, relationColumn, gifts );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "addToRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestAddRelationOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Snow')" );

      OpResult gifts = unitOfWork.Find( "Order", queryBuilder );
      String personObjectId = (String) Backendless.Data.Of( "Person" ).FindById( "93957B42-9B1A-1CCE-FFAD-ADE44D459A00" )[ "objectId" ];
      String parentTable = "Person";
      String relationColumn = "Surname";

      unitOfWork.AddToRelation( parentTable, personObjectId, relationColumn, gifts );
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "addToRelationPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestAddRelationWithId()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] objToRel = { (String) Backendless.Data.Of( "Order" ).FindFirst()["objectId"] };
      String personObjId = (String) Backendless.Data.Of( "Person" ).FindById( "BE23F5DA-7431-F64B-FF8B-B6176463C400" )[ "objectId" ];
      String parentTable = "Person";
      String relationColumn = "Surname";

      unitOfWork.AddToRelation( parentTable, personObjId, relationColumn, objToRel );
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "addToRelationPerson1" ] ).Count == 2 );
    }
  }
}
