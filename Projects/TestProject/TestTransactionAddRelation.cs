using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using BackendlessAPI.Transaction;
using System.Collections.Generic;

namespace TestProject
{
  [Collection("Tests")]
  public class TestTransactionAddRelation : IDisposable
  {
    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age = '22'" );
      Backendless.Data.Of( "Order" ).Remove( "LastName = 'Smith'" );
    }

    [Fact]
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

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create().SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );
      Assert.True( listCheckPersonObj.Count == 1 );
      Assert.True( listCheckPersonObj[ 0 ].Surname != null );
    }

    [Fact]
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

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

      Assert.True( listCheckPersonObj.Count == 1 );
      Assert.True( listCheckPersonObj[0].Surname != null );
    }

    [Fact]
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

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

      Assert.True( listCheckPersonObj.Count == 1 );
      Assert.True( listCheckPersonObj[ 0 ].Surname != null );
    }

    [Fact]
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

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

      Assert.True( listCheckPersonObj.Count == 1 );
      Assert.True( listCheckPersonObj[ 0 ].Surname != null );
    }

    [Fact]
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

      Assert.False( uowResult.Success );
      Assert.Null( uowResult.Results );
    }
  }
}