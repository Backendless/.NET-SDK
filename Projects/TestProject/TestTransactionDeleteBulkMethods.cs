using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransactionDeleteBulkMethods
  {
    [TestMethod]
    public void TestDeleteBulkObjects_Dictionary()
    {
      List<Dictionary<String, Object>> objectsMaps = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "name" ] = "Joe";
      secondMap[ "name" ] = "Tommy";
      objectsMaps.Add( firstMap );
      objectsMaps.Add( secondMap );

      List<String> objectIds = (List<String>) Backendless.Data.Of( "Person" ).Create( objectsMaps );

      UnitOfWork uow = new UnitOfWork();
      uow.BulkDelete( "Person", objectIds.ToArray() );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Dictionary<String, Object>> personMaps = Backendless.Data.Of( "Person" ).Find();

      Assert.IsTrue( personMaps.Count == 0 );
    }

    [TestMethod]
    public void TestDeleteBulkObjects_Class()
    {
      List<Person> personList = new List<Person>();
      Person firstPerson = new Person();
      Person secondPerson = new Person();

      firstPerson.age = 17;
      firstPerson.name = "Tom";
      secondPerson.age = 27;
      secondPerson.name = "Mary";
      personList.Add( firstPerson );
      personList.Add( secondPerson );

      IList<String> objectIds = Backendless.Data.Of<Person>().Create( personList );
      firstPerson.objectId = objectIds[ 0 ];
      secondPerson.objectId = objectIds[ 1 ];

      UnitOfWork uow = new UnitOfWork();
      uow.BulkDelete( personList );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();

      Assert.IsTrue( listCheckPerson.Count == 0 );
    }

    [TestMethod]
    public void TestDeleteBulkObjects_OpResult()
    {
      List<Person> personList = new List<Person>();
      Person firstPerson = new Person();
      Person secondPerson = new Person();
      firstPerson.age = 22;
      firstPerson.name = "John";
      secondPerson.age = 12;
      secondPerson.name = "Ivie";
      personList.Add( firstPerson );
      personList.Add( secondPerson );

      UnitOfWork uow = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "age > '10'" );

      OpResult invalidName = uow.Find( "Person", queryBuilder );

      uow.BulkDelete( invalidName );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
      Assert.IsTrue( listCheckPerson.Count == 0 );
    }

    [TestMethod]
    public void TestDeteleBulkObjects_WithId()
    {
      List<Person> personList = new List<Person>();
      Person firstPersonObj = new Person();
      Person secondPersonObj = new Person();
      firstPersonObj.age = 11;
      firstPersonObj.name = "Tom";
      secondPersonObj.age = 13;
      secondPersonObj.name = "Bob";
      personList.Add( firstPersonObj );
      personList.Add( secondPersonObj );

      List<String> objIdToDel = (List<String>) Backendless.Data.Of<Person>().Create( personList );
      UnitOfWork uow = new UnitOfWork();

      uow.BulkDelete( "Person", objIdToDel.ToArray() );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
      Assert.IsTrue( listCheckPerson.Count == 0 );
    }

    [TestMethod]
    public void TestDeleteBulkObjects_WhereClause()
    {
      List<Dictionary<String, Object>> personList = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> firstPerson = new Dictionary<String, Object>();
      Dictionary<String, Object> secondPerson = new Dictionary<String, Object>();
      Dictionary<String, Object> undeletableObject = new Dictionary<String, Object>();

      firstPerson[ "age" ] = 80;
      firstPerson[ "name" ] = "Bob";
      secondPerson[ "age" ] = 50;
      secondPerson[ "name" ] = "Tom";
      undeletableObject[ "age" ] = 20;
      undeletableObject[ "name" ] = "Alexandra";

      personList.Add( firstPerson );
      personList.Add( secondPerson );
      personList.Add( undeletableObject );
      Backendless.Data.Of( "Person" ).Create( personList );

      UnitOfWork uow = new UnitOfWork();

      uow.BulkDelete( "Person", "age > '45'" );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> listCheckPerson = Backendless.Data.Of<Person>().Find();
      Assert.IsTrue( listCheckPerson.Count == 1 );

      Backendless.Data.Of( "Person" ).Remove( "age = '20'" );
    }
  }
}
