using Xunit;
using System;
using System.Linq;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using BackendlessAPI.Transaction;
using System.Collections.Generic;

namespace TestProject
{
  [Collection("Tests")]
  public class TestTransactionSetRelation : IDisposable
  {
    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age = '22'" );
      Backendless.Data.Of( "Order" ).Remove( "LastName = 'Smith'" );
    }

    [Fact]
    public void TestSetRelation_Dictionary()
    {
      Dictionary<String, Object> objectMap = new Dictionary<String, Object>();
      objectMap["age"] = 22;
      objectMap["name"] = "Eva";

      objectMap[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( objectMap )[ "objectId" ];

      Order orderObj = new Order();
      orderObj.LastName = "Smith";
      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";

      childObjMap[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( childObjMap )[ "objectId" ];
      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.SetRelation( "Person", objectMap, relationColumn, new String[] { (String) childObjMap[ "objectId" ] } );
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
    public void TestSetRelation_Class()
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
      uow.SetRelation( personObj, relationColumn, new List<Order> { orderObj } );
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
    public void TestSetRelation_OpResult()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";
      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      Order orderObj = new Order();
      orderObj.LastName = "Smith";

      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";
      childObjMap[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( childObjMap );

      String relationColumn = "Surname";
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );

      UnitOfWork uow = new UnitOfWork();
      OpResult gifts = uow.Find( "Order", queryBuilder );
      uow.SetRelation( personObj.GetType().Name, personObj.objectId, relationColumn, gifts );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      Person checkPersonObj = Backendless.Data.Of<Person>().Find( dqb )[ 0 ];

      Assert.True( checkPersonObj.Surname != null );
    }

    [Fact]
    public void TestSetRelation_WithId()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";

      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";
      childObjMap[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( childObjMap )[ "objectId" ];

      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.SetRelation( personObj.GetType().Name, personObj.objectId, relationColumn, new String[] { (String) childObjMap[ "objectId" ] } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      Person checkPersonObj = Backendless.Data.Of<Person>().Find( dqb )[ 0 ];

      Assert.True( checkPersonObj.Surname != null );
    }

    [Fact]
    public void TestSetRelation_CheckError()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";

      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;
      
      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";

      childObjMap[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( childObjMap )[ "objectId" ];
      
      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.SetRelation( "Wrong name", personObj.objectId, relationColumn, new String[] { (String) childObjMap[ "objectId" ] } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.False( uowResult.Success );
      Assert.Null( uowResult.Results );
    }
  }
}