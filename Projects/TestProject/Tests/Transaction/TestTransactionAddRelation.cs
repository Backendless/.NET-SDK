using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using BackendlessAPI.Transaction;
using System.Collections.Generic;

namespace TestProject.Tests.Transaction
{
  [Collection( "Tests" )]
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
    public void TestAddRelation_Class_Callback()
    {
      Person personObj = new Person();
      Order orderObj = new Order();
      personObj.age = 22;
      personObj.name = "Eva";
      orderObj.LastName = "Smith";
      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;
      orderObj.objectId = Backendless.Data.Of<Order>().Save( orderObj ).objectId;

      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.AddToRelation( personObj, relationColumn, new List<Order>() { orderObj } );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        DataQueryBuilder dqb = DataQueryBuilder.Create().SetRelationsDepth( 10 );
        dqb.SetRelationsPageSize( 10 );

        IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );
        Assert.True( listCheckPersonObj.Count == 1 );
        Assert.True( listCheckPersonObj[ 0 ].Surname != null );
      },
      fault =>
      {
        Assert.True( false, "An error went during the execution operation" );
      } ) );
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
      order[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( order )[ "objectId" ];
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
      Assert.True( listCheckPersonObj[ 0 ].Surname != null );
    }
    
    [Fact]
    public void TestAddRelation_Dictionary_Callback()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";

      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;
      Dictionary<String, Object> order = new Dictionary<String, Object>();
      order[ "LastName" ] = "Smith";
      order[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( order )[ "objectId" ];
      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.AddToRelation( personObj.GetType().Name, personObj.objectId, relationColumn, new List<Dictionary<String, Object>>() { order } );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        DataQueryBuilder dqb = DataQueryBuilder.Create();
        dqb.SetRelationsPageSize( 10 );
        dqb.SetRelationsDepth( 10 );
        IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

        Assert.True( listCheckPersonObj.Count == 1 );
        Assert.True( listCheckPersonObj[ 0 ].Surname != null );
      },
      fault =>
      {
        Assert.True( false, "An error went during the execution operation" );
      } ) );
    }

    [Fact]
    public void TestAddRelation_OpResult()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";
      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";
      childObjMap[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( childObjMap )[ "objectId" ];

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );
      UnitOfWork uow = new UnitOfWork();
      OpResult gifts = uow.Find( "Order", queryBuilder );
      String relationColumn = "Surname";
      uow.AddToRelation( personObj.GetType().Name, personObj.objectId, relationColumn, gifts );
      UnitOfWorkResult uowResult = uow.Execute();

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsPageSize( 10 );
      dqb.SetRelationsDepth( 10 );
      IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );
      Assert.True( listCheckPersonObj.Count == 1 );
      Assert.True( listCheckPersonObj[ 0 ].Surname != null );
    }

    [Fact]
    public void TestAddRelation_OpResult_Callback()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";
      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      Dictionary<String, Object> childObjMap = new Dictionary<String, Object>();
      childObjMap[ "LastName" ] = "Smith";
      childObjMap[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( childObjMap )[ "objectId" ];

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );
      UnitOfWork uow = new UnitOfWork();
      OpResult gifts = uow.Find( "Order", queryBuilder );
      String relationColumn = "Surname";
      uow.AddToRelation( personObj.GetType().Name, personObj.objectId, relationColumn, gifts );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        DataQueryBuilder dqb = DataQueryBuilder.Create();
        dqb.SetRelationsPageSize( 10 );
        dqb.SetRelationsDepth( 10 );
        IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );
        Assert.True( listCheckPersonObj.Count == 1 );
        Assert.True( listCheckPersonObj[ 0 ].Surname != null );
      },
      fault =>
      {
        Assert.True( false, "An error went during the execution operation" );
      } ) );
    }

    [Fact]
    public void TestAddRelation_WithId()
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
    public void TestAddRelation_WithId_Callback()
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
      uow.AddToRelation( personObj.GetType().Name, personObj.objectId, relationColumn, new String[] { (String) childObjMap[ "objectId" ] } );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.True( uowResult.Success );
        Assert.NotNull( uowResult.Results );

        DataQueryBuilder dqb = DataQueryBuilder.Create();
        dqb.SetRelationsPageSize( 10 );
        dqb.SetRelationsDepth( 10 );
        IList<Person> listCheckPersonObj = Backendless.Data.Of<Person>().Find( dqb );

        Assert.True( listCheckPersonObj.Count == 1 );
        Assert.True( listCheckPersonObj[ 0 ].Surname != null );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public void TestAddToRelation_CheckError()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";

      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      Order childObjMap = new Order();
      childObjMap.LastName = "Smith";
      childObjMap.objectId = Backendless.Data.Of<Order>().Save( childObjMap ).objectId;
      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.AddToRelation( "Wrong name", personObj.objectId, relationColumn, new String[] { childObjMap.objectId } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.False( uowResult.Success );
      Assert.Null( uowResult.Results );
    }

    [Fact]
    public void TestAddToRelation_CheckError_Callback()
    {
      Person personObj = new Person();
      personObj.age = 22;
      personObj.name = "Eva";

      personObj.objectId = Backendless.Data.Of<Person>().Save( personObj ).objectId;

      Order childObjMap = new Order();
      childObjMap.LastName = "Smith";
      childObjMap.objectId = Backendless.Data.Of<Order>().Save( childObjMap ).objectId;
      String relationColumn = "Surname";

      UnitOfWork uow = new UnitOfWork();
      uow.AddToRelation( "Wrong name", personObj.objectId, relationColumn, new String[] { childObjMap.objectId } );
      uow.Execute( new AsyncCallback<UnitOfWorkResult>(
      uowResult =>
      {
        Assert.False( uowResult.Success );
        Assert.Null( uowResult.Results );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }
  }
}