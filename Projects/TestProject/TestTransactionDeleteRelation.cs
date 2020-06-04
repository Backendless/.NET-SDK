using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransactionDeleteRelation
  {
    [TestCleanup]
    public void TearDown()
    {
      Backendless.Data.Of( "Person" ).Remove( "age = '22'" );
      Backendless.Data.Of( "Order" ).Remove( "LastName = 'Smith'" );
    }

    [TestMethod]
    public void TestDeleteRelation_Dictionary()
    {
      List<Dictionary<String, Object>> listObjMap = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> parentObj = new Dictionary<String, Object>();
      Dictionary<String, Object> childObj = new Dictionary<String, Object>();

      parentObj[ "name" ] = "Eva";
      parentObj[ "age" ] = 22;

      childObj[ "LastName" ] = "Smith";
      listObjMap.Add( parentObj );

      IList<String> parentId = Backendless.Data.Of( "Person" ).Create( listObjMap );

      parentObj[ "objectId" ] = parentId[ 0 ];
      listObjMap.Clear();
      listObjMap.Add( childObj );

      List<String> childId = (List<String>) Backendless.Data.Of( "Order" ).Create( listObjMap );
      listObjMap.Clear();
      childObj[ "objectId" ] = childId[ 0 ];
      listObjMap.Add( childObj );

      String relationColumnName = "Surname";
      Backendless.Data.Of( "Person" ).AddRelation( parentObj, relationColumnName, listObjMap.ToArray() );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      IList<Dictionary<String, Object>> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );

      Assert.IsTrue( objectBefore_DeleteRelation[ 0 ][ "Surname" ] != null );

      listObjMap.Clear();
      childObj[ "objectId" ] = childId[ 0 ];
      listObjMap.Add( childObj );

      UnitOfWork uow = new UnitOfWork();
      uow.DeleteRelation( "Person", parentId[ 0 ], relationColumnName, listObjMap );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Dictionary<String, Object>> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );
      Assert.IsNull( objectAfter_DeleteRelation[ 0 ][ "Surname" ] );
    }

    [TestMethod]
    public void TestDeleteRelation_Class()
    {
      List<Person> listObjects = new List<Person>();
      Person personObject = new Person();
      personObject.age = 22;
      personObject.name = "Jia";
      listObjects.Add( personObject );

      List<Order> childObject = new List<Order>();
      Order orderObject = new Order();
      orderObject.LastName = "Smith";

      personObject.objectId = Backendless.Data.Of<Person>().Create( listObjects )[ 0 ];

      childObject.Add( orderObject );
      orderObject.objectId = Backendless.Data.Of<Order>().Create( childObject )[ 0 ];

      childObject.Clear();
      childObject.Add( orderObject );

      String relationColumn = "Surname";

      Backendless.Data.Of<Person>().AddRelation( personObject, relationColumn, childObject.ToArray() );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      IList<Dictionary<String, Object>> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );

      Assert.IsTrue( objectBefore_DeleteRelation[ 0 ][ "Surname" ] != null );

      UnitOfWork uow = new UnitOfWork();
      uow.DeleteRelation( personObject, relationColumn, childObject );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Dictionary<String, Object>> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );
      Assert.IsNull( objectAfter_DeleteRelation[ 0 ][ "Surname" ] );
    }

    [TestMethod]
    public void TestDeleteRelation_OpResult()
    {
      List<Person> listObjects = new List<Person>();
      Person personObject = new Person();
      personObject.age = 22;
      personObject.name = "Jia";
      listObjects.Add( personObject );

      List<Order> childObject = new List<Order>();
      Order orderObject = new Order();
      orderObject.LastName = "Smith";

      personObject.objectId = Backendless.Data.Of<Person>().Create( listObjects )[ 0 ];

      childObject.Add( orderObject );
      orderObject.objectId = Backendless.Data.Of<Order>().Create( childObject )[ 0 ];

      childObject.Clear();
      childObject.Add( orderObject );

      String relationColumn = "Surname";

      Backendless.Data.Of<Person>().AddRelation( personObject, relationColumn, childObject.ToArray() );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      IList<Dictionary<String, Object>> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );

      Assert.IsTrue( objectBefore_DeleteRelation[ 0 ][ "Surname" ] != null );

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );

      UnitOfWork uow = new UnitOfWork();
      OpResult gifts = uow.Find( "Order", queryBuilder );
      uow.DeleteRelation( personObject.GetType().Name, personObject.objectId, relationColumn, gifts );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Dictionary<String, Object>> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );
      Assert.IsNull( objectAfter_DeleteRelation[ 0 ][ "Surname" ] );
    }

    [TestMethod]
    public void TestDeleteRelation_WithId()
    {
      List<Person> listObjects = new List<Person>();
      Person personObject = new Person();
      personObject.age = 22;
      personObject.name = "Jia";
      listObjects.Add( personObject );

      List<Order> childObject = new List<Order>();
      Order orderObject = new Order();
      orderObject.LastName = "Smith";

      personObject.objectId = Backendless.Data.Of<Person>().Create( listObjects )[ 0 ];

      childObject.Add( orderObject );
      orderObject.objectId = Backendless.Data.Of<Order>().Create( childObject )[ 0 ];

      childObject.Clear();
      childObject.Add( orderObject );

      String relationColumn = "Surname";

      Backendless.Data.Of<Person>().AddRelation( personObject, relationColumn, childObject.ToArray() );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      IList<Dictionary<String, Object>> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );

      Assert.IsTrue( objectBefore_DeleteRelation[ 0 ][ "Surname" ] != null );

      UnitOfWork unitOfWork = new UnitOfWork();
      unitOfWork.DeleteRelation( personObject.GetType().Name, personObject.objectId, relationColumn, new string[] { orderObject.objectId } );
      UnitOfWorkResult uowResult = unitOfWork.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Dictionary<String, Object>> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );
      Assert.IsNull( objectAfter_DeleteRelation[ 0 ][ "Surname" ] );
    }

    [TestMethod]
    public void TestDeleteRaltion_CheckError()
    {
      List<Person> listObjects = new List<Person>();
      Person personObject = new Person();
      personObject.age = 22;
      personObject.name = "Jia";
      listObjects.Add( personObject );

      List<Order> childObject = new List<Order>();
      Order orderObject = new Order();
      orderObject.LastName = "Smith";

      personObject.objectId = Backendless.Data.Of<Person>().Create( listObjects )[ 0 ];

      childObject.Add( orderObject );
      orderObject.objectId = Backendless.Data.Of<Order>().Create( childObject )[ 0 ];

      childObject.Clear();
      childObject.Add( orderObject );

      Backendless.Data.Of<Person>().AddRelation( personObject, "Surname", childObject.ToArray() );

      DataQueryBuilder dqb = DataQueryBuilder.Create();
      dqb.SetRelationsDepth( 10 );
      dqb.SetRelationsPageSize( 10 );

      IList<Dictionary<String, Object>> objectBefore_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );

      Assert.IsTrue( objectBefore_DeleteRelation[ 0 ][ "Surname" ] != null );

      UnitOfWork uow = new UnitOfWork();
      uow.DeleteRelation( personObject, "Wrong column name" , childObject );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsFalse( uowResult.Success );
      Assert.IsNull( uowResult.Results );

      IList<Dictionary<String, Object>> objectAfter_DeleteRelation = Backendless.Data.Of( "Person" ).Find( dqb );
      Assert.IsNotNull( objectAfter_DeleteRelation[ 0 ][ "Surname" ] );
    }
  }
}
