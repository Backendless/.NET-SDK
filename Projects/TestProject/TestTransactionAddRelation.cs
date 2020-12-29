using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransactionAddRelation
  {
    [TestCleanup]
    public void TearDown()
    {
      Backendless.Data.Of( "Person" ).Remove( "age = '22'" );
      Backendless.Data.Of( "Order" ).Remove( "LastName = 'Smith'" );
    }

    [ClassInitialize]
    public static void ClassInitialize( TestContext context )
    {
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      TestInitialization.CreateDefaultTable( "Person" );
      TestInitialization.CreateDefaultTable( "Order" );
      TestInitialization.CreateRelationColumnOneToMany( "Person", "Order", "Surname" );
    }

    [TestMethod]
    public void TestAddRelation_Class()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";

      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      Order orderObj = new Order();
      orderObj.LastName = "Smith";

      orderObj.objectId = Backendless.Data.Of<Order>().Save( orderObj ).objectId;

      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.AddToRelation( personObj, relationColumn, new List<Order>() { orderObj } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create().SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );
      Assert.IsTrue( listCheckPersonObj.Count == 1 );
      Assert.IsTrue( listCheckPersonObj[ 0 ].Surname != null );
    }

    [TestMethod]
    public void TestAddRelation_Dictionary()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";

      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      Dictionary<String, Object> order = new Dictionary<String, Object>();
      order[ "LastName" ] = "Smith";

      order["objectId"] = Backendless.Data.Of( "Order" ).Save( order )["objectId"];

      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.AddToRelation( personObj.GetType().Name, personObj.objectId, relationColumn, new List<Dictionary<String, Object>>() { order } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

      Assert.IsTrue( listCheckPersonObj.Count == 1 );
      Assert.IsTrue( listCheckPersonObj[0].Surname != null );
    }

    [TestMethod]
    public void TestAddRelation_OpResult()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";
      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      List<Order> listOrder = new List<Order>();
      Order orderObj = new Order();
      orderObj.LastName = "Smith";
      listOrder.Add( orderObj );
      List<Dictionary<String, Object>> listChildObjMap = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";
      listChildObjMap.Add( childObjMap );

      IList<String> childObjIds = Backendless.Data.Of( "Order" ).Create( listChildObjMap );
      listChildObjMap.Clear();
      childObjMap[ "objectId" ] = childObjIds[ 0 ];
      listChildObjMap.Add( childObjMap );

      UnitOfWork uow = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );

      OpResult gifts = uow.Find( "Order", queryBuilder );
      String relationColumn = "Surname";

      uow.AddToRelation( personObj.GetType().Name, personObj.objectId, relationColumn, gifts );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

      Assert.IsTrue( listCheckPersonObj.Count == 1 );
      Assert.IsTrue( listCheckPersonObj[ 0 ].Surname != null );
    }

    [TestMethod]
    public void TestAddRelation_WithId()
    {
      List<Person> listPerson = new List<Person>();
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";
      listPerson.Add( personObj );

      IList<String> parentObjIds = Backendless.Data.Of<Person>().Create( listPerson );
      listPerson.Clear();
      personObj.objectId = parentObjIds[ 0 ];

      List<Order> listOrder = new List<Order>();
      Order orderObj = new Order();
      orderObj.LastName = "Smith";
      listOrder.Add( orderObj );
      List<Dictionary<String, Object>> listChildObjMap = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";
      listChildObjMap.Add( childObjMap );

      IList<String> childObjIds = Backendless.Data.Of( "Order" ).Create( listChildObjMap );
      listChildObjMap.Clear();
      childObjMap[ "objectId" ] = childObjIds[ 0 ];
      listChildObjMap.Add( childObjMap );

      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.AddToRelation( personObj.GetType().Name, personObj.objectId, relationColumn, new String[] { (String) childObjMap[ "objectId" ] } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

      Assert.IsTrue( listCheckPersonObj.Count == 1 );
      Assert.IsTrue( listCheckPersonObj[ 0 ].Surname != null );
    }

    [TestMethod]
    public void TestAddToRelation_CheckError()
    {
      List<Person> listPerson = new List<Person>();
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";
      listPerson.Add( personObj );

      IList<String> parentObjIds = Backendless.Data.Of<Person>().Create( listPerson );
      listPerson.Clear();
      personObj.objectId = parentObjIds[ 0 ];

      List<Order> listOrder = new List<Order>();
      Order orderObj = new Order();
      orderObj.LastName = "Smith";
      listOrder.Add( orderObj );
      List<Dictionary<String, Object>> listChildObjMap = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";
      listChildObjMap.Add( childObjMap );

      IList<String> childObjIds = Backendless.Data.Of( "Order" ).Create( listChildObjMap );
      listChildObjMap.Clear();
      childObjMap[ "objectId" ] = childObjIds[ 0 ];
      listChildObjMap.Add( childObjMap );

      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.AddToRelation( "Wrong name", personObj.objectId, relationColumn, new String[] { (String) childObjMap[ "objectId" ] } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsFalse( uowResult.Success );
      Assert.IsNull( uowResult.Results );
    }
  }
}
