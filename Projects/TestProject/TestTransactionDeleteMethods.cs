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
  public class TestTransactionDeleteMethods
  {
    [TestMethod]
    public void TestDeleteSingleObjectDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      Dictionary<String, Object> pers = new Dictionary<String, Object>();

      pers[ "objectId" ] = (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ];

      unitOfWork.Delete( "Person", pers );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deletePerson1" ] ).Count == 2 );
    }


    [TestMethod]
    public void TestDeleteMultipleObjectsDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<String> objectsToDelete = new List<String>();

      objectsToDelete.Add( (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ] );
      objectsToDelete.Add( (String) Backendless.Data.Of( "Person" ).FindLast()[ "objectId" ] );

      unitOfWork.BulkDelete( "Person", objectsToDelete.ToArray() );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteBulkPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteSingleObjectClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Person person = new Person();
      person.SetObjectId( (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ] );

      unitOfWork.Delete( person );
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deletePerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteMultipleObjectsClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Person> peopleId = new List<Person>();

      Person person1 = new Person();
      person1.SetObjectId( (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ] );

      Person person2 = new Person();
      person2.SetObjectId( (String) Backendless.Data.Of( "Person" ).FindLast()[ "objectId" ] );

      peopleId.Add( person1 );
      peopleId.Add( person2 );

      unitOfWork.BulkDelete( peopleId );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteBulkPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteSingleObjectOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "age = '23'" );

      OpResult opResult = unitOfWork.Find( "Person", queryBuilder );
      OpResultValueReference firstInvalid = opResult.ResolveTo( 0 );

      unitOfWork.Delete( firstInvalid );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deletePerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteMultipleObjectsOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "age = '12'" );

      OpResult invalidName = unitOfWork.Find( "Person", queryBuilder );

      unitOfWork.BulkDelete( invalidName );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteBulkPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteSingleObjectWithId()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      unitOfWork.Delete( "Person", (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ] );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deletePerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeteleMultipleObjectsWithId()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      List<String> objIdToDel = new List<String>();
      objIdToDel.Add( (String) Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ] );
      objIdToDel.Add( (String) Backendless.Data.Of( "Person" ).FindLast()[ "objectId" ] );

      unitOfWork.BulkDelete( "Person", objIdToDel.ToArray() );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteBulkPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestDeleteMultipleObjectsQuery()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      unitOfWork.BulkDelete( "Person", "age = '100500'" );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "deleteBulkPerson1" ] ).Count == 2 );
    }
  }
}
