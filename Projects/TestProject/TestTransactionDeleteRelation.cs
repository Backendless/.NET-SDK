using Xunit;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [Collection( "Tests" )]
  public class TestTransactionDeleteRelation : IDisposable
  {
    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age = '22'" );
      Backendless.Data.Of( "Order" ).Remove( "LastName = 'Smith'" );
    }

    [Fact]
    public void TestDeleteRelation_Dictionary()
    {
      Dictionary<String, Object> parentObj = new Dictionary<String, Object>();
      Dictionary<String, Object> childObj = new Dictionary<String, Object>();

      parentObj[ "name" ] = "Eva";
      parentObj[ "age" ] = 22;
      parentObj[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( parentObj )[ "objectId" ];

      childObj[ "LastName" ] = "Smith";
      childObj[ "objectId" ] = Backendless.Data.Of( "Order" ).Save( childObj )[ "objectId" ];

      String relationColumnName = "Surname";
      Backendless.Data.Of( "Person" ).AddRelation( parentObj, relationColumnName, new Object[] { childObj } );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      Dictionary<String, Object> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( (String) parentObj[ "objectId" ], dqb );
      Assert.NotNull( objectBefore_DeleteRelation[ "Surname" ] );

      UnitOfWork uow = new UnitOfWork();
      uow.DeleteRelation( "Person", (String) parentObj[ "objectId" ], relationColumnName, new List<Dictionary<String, Object>> { childObj } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, Object> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( (String) parentObj[ "objectId" ], dqb );
      Assert.True( ( (Object[]) objectAfter_DeleteRelation[ "Surname" ] ).Length == 0 );
    }

    [Fact]
    public void TestDeleteRelation_Class()
    {
      Person personObject = new Person();
      personObject.age = 22;
      personObject.name = "Jia";

      Order orderObject = new Order();
      orderObject.LastName = "Smith";

      personObject.objectId = Backendless.Data.Of<Person>().Save( personObject ).objectId;
      orderObject.objectId = Backendless.Data.Of<Order>().Save( orderObject ).objectId;

      String relationColumn = "Surname";

      Backendless.Data.Of<Person>().AddRelation( personObject, relationColumn, new Object[] { orderObject } );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      Dictionary<String, Object> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( personObject.objectId, dqb );

      Assert.NotNull( objectBefore_DeleteRelation[ "Surname" ] );

      UnitOfWork uow = new UnitOfWork();
      uow.DeleteRelation( personObject, relationColumn, new List<Order> { orderObject } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, Object> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( personObject.objectId, dqb );
      Assert.True( ( (Object[]) objectAfter_DeleteRelation[ "Surname" ] ).Length == 0 );
    }

    [Fact]
    public void TestDeleteRelation_OpResult()
    {
      Person personObject = new Person();
      personObject.age = 22;
      personObject.name = "Jia";

      Order orderObject = new Order();
      orderObject.LastName = "Smith";

      personObject.objectId = Backendless.Data.Of<Person>().Save( personObject ).objectId;
      orderObject.objectId = Backendless.Data.Of<Order>().Save( orderObject ).objectId;

      String relationColumn = "Surname";
      Backendless.Data.Of<Person>().AddRelation( personObject, relationColumn, new Object[] { orderObject } );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      Dictionary<String, Object> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( personObject.objectId, dqb );

      Assert.NotNull( objectBefore_DeleteRelation[ "Surname" ] );

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );

      UnitOfWork uow = new UnitOfWork();
      OpResult gifts = uow.Find( "Order", queryBuilder );
      uow.DeleteRelation( personObject.GetType().Name, personObject.objectId, relationColumn, gifts );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, Object> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( personObject.objectId, dqb );
      Assert.True( ( (Object[]) objectAfter_DeleteRelation[ "Surname" ] ).Length == 0 );
    }

    [Fact]
    public void TestDeleteRelation_WithId()
    {
      Person personObject = new Person();
      personObject.age = 22;
      personObject.name = "Jia";

      Order orderObject = new Order();
      orderObject.LastName = "Smith";

      personObject.objectId = Backendless.Data.Of<Person>().Save( personObject ).objectId;
      orderObject.objectId = Backendless.Data.Of<Order>().Save( orderObject ).objectId;

      String relationColumn = "Surname";
      Backendless.Data.Of<Person>().AddRelation( personObject, relationColumn, new Object[] { orderObject } );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      Dictionary<String, Object> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( personObject.objectId, dqb );
      Assert.NotNull( objectBefore_DeleteRelation[ "Surname" ] );

      UnitOfWork unitOfWork = new UnitOfWork();
      unitOfWork.DeleteRelation( personObject.GetType().Name, personObject.objectId, relationColumn, new String[] { orderObject.objectId } );
      UnitOfWorkResult uowResult = unitOfWork.Execute();

      Assert.True( uowResult.Success );
      Assert.NotNull( uowResult.Results );

      Dictionary<String, Object> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( personObject.objectId, dqb );
      Assert.True( ( (Object[]) objectAfter_DeleteRelation[ "Surname" ] ).Length == 0 );
    }

    [Fact]
    public void TestDeleteRaltion_CheckError()
    {
      Person personObject = new Person();
      personObject.age = 22;
      personObject.name = "Jia";

      Order orderObject = new Order();
      orderObject.LastName = "Smith";

      personObject.objectId = Backendless.Data.Of<Person>().Save( personObject ).objectId;
      orderObject.objectId = Backendless.Data.Of<Order>().Save( orderObject ).objectId;

      Backendless.Data.Of<Person>().AddRelation( personObject, "Surname", new Object[] { orderObject } );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      Dictionary<String, Object> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( personObject.objectId, dqb );
      Assert.NotNull( objectBefore_DeleteRelation[ "Surname" ] );

      UnitOfWork uow = new UnitOfWork();
      uow.DeleteRelation( personObject, "Wrong column name", new List<Order> { orderObject } );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.False( uowResult.Success );
      Assert.Null( uowResult.Results );

      Dictionary<String, Object> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).FindById( personObject.objectId, dqb );
      Assert.NotNull( objectAfter_DeleteRelation[ "Surname" ] );
    }
  }
}