/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransactionDeleteMethods
  {
    [TestMethod]
    public void TestDeleteSingleObject_Class()
    {
      List<Person> listPerson = new List<Person>();
      Person personCreated = new Person();
      personCreated.age = 30;
      personCreated.name = "Alexandra";
      listPerson.Add( personCreated );
      IList<String> objectIds = Backendless.Data.Of<Person>().Create( listPerson );
      personCreated.objectId = objectIds[ 0 ];

      UnitOfWork uow = new UnitOfWork();
      uow.Delete( personCreated );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();

      Assert.IsTrue( listCheckPerson.Count == 0 );
    }

    [TestMethod]
    public void TestDeleteSingleObject_Dictionary()
    {
      IList<Dictionary<String, Object>> objectMap = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> defaultObject = new Dictionary<String, Object>();
      defaultObject[ "name" ] = "Joe";
      defaultObject[ "age" ] = 28;
      objectMap.Add( defaultObject );
      String objectId = Backendless.Data.Of( "Person" ).Create( objectMap )[ 0 ];
      
      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Person", objectId );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Dictionary<String, Object>> personMaps = Backendless.Data.Of( "Person" ).Find();

      Assert.IsTrue( personMaps.Count == 0 );
    }

    [TestMethod]
    public void TestDeleteSingleObject_OpResult()
    {
      List<Person> personList = new List<Person>();
      Person personObject = new Person();
      personObject.name = "Bob";
      personObject.age = 23;
      personList.Add( personObject );
      String objectId = Backendless.Data.Of<Person>().Create( personList )[ 0 ];

      UnitOfWork uow = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "age = '23'" );

      OpResult opResult = uow.Find( "Person", queryBuilder );
      OpResultValueReference firstInvalid = opResult.ResolveTo( 0 );

      uow.Delete( firstInvalid );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
      Assert.IsTrue( listCheckPerson.Count == 0 );
    }

    [TestMethod]
    public void TestDeleteSingleObject_WithId()
    {
      List<Person> personList = new List<Person>();
      Person defaultPersonObject = new Person();
      defaultPersonObject.age = 20;
      defaultPersonObject.name = "John";
      personList.Add( defaultPersonObject );
      Backendless.Data.Of<Person>().Create( personList );

      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Person", (String) Backendless.Data.Of( "Person" ).Find()[ 0 ][ "objectId" ] );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
      Assert.IsTrue( listCheckPerson.Count == 0 );
    }

    [TestMethod]
    public void TestDeleteSingleObject_CheckError()
    {
      UnitOfWork uow = new UnitOfWork();
      uow.Delete( "Wrong table name", "Empty objectId" );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsFalse( uowResult.Success );
      Assert.IsNull( uowResult.Results );
    }

    [TestMethod]
    public void TestDeleteBulkObjects_CheckError()
    {
      UnitOfWork uow = new UnitOfWork();
      List<String> emptyObjectId = new List<String>();
      emptyObjectId.Add( "empty objectId" );
      uow.BulkDelete( "Wrong table name", emptyObjectId.ToArray() );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsFalse( uowResult.Success );
      Assert.IsNull( uowResult.Results );
    }
  }
}
*/