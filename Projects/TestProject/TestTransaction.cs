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
  public class TestTransaction
  {
    public class Person
    {
      private String name;
      private int age;
      private String objectId;

      public String GetObjectId()
      {
        return objectId;
      }

      public void SetObjectId( String objectId )
      {
        this.objectId = objectId;
      }

      public String GetName()
      {
        return name;
      }

      public void SetName( String name )
      {
        this.name = name;
      }

      public int GetAge()
      {
        return age;
      }

      public void SetAge( int age )
      {
        this.age = age;
      }
    }

    public class Order
    {
      private String objectId;
      private String name;

      public String GetObjectId()
      {
        return objectId;
      }

      public void SetObjectId( String objectId )
      {
        this.objectId = objectId;
      }

      public String GetLastName()
      {
        return name;
      }

      public void SetLastName( String name )
      {
        this.name = name;
      }
    }

    [TestMethod]
    public void TestCreateSingleObjectDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Dictionary<String, Object> order = new Dictionary<String, Object>();
      order[ "name" ] = "Joe";
      order[ "age" ] = 23;
      OpResult opResult = unitOfWork.Create( "Order", order );

      UnitOfWorkResult unitOfWorkRes = unitOfWork.Execute();
      Assert.IsTrue( true );
    }

    [TestMethod]
    public void TestDeleteSingleObjectDictionary()
    {
      String objId = "43346BF5-B982-6C16-FF16-40618391DA00";
      UnitOfWork unitOfWork = new UnitOfWork();
      Dictionary<String, Object> order = new Dictionary<String, Object>();

      order[ "objectId" ] = objId;

      unitOfWork.Delete( "Order", order );

      UnitOfWorkResult unitOfWorkRes = unitOfWork.Execute();
      Assert.IsTrue( true );
    }

    [TestMethod]
    public void TestFindSingleObjectDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      DataQueryBuilder dQB = DataQueryBuilder.Create();

      dQB.SetPageSize( 1 );

      OpResult findObjectResult = unitOfWork.Find( "Person", dQB );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestUpdateSingleObjectDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Dictionary<String, Object> res = new Dictionary<String, Object>();
      res[ "age" ] = 35;
      res["objectId"] = "43346BF5-B982-6C16-FF16-40618391DA00";
      unitOfWork.Update( "Order", res );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestCreateMultipleObjectsDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Dictionary<String, Object>> people = new List<Dictionary<String, Object>>();

      Dictionary<String, Object> person1 = new Dictionary<String, Object>();
      person1["name"] = "Mary";
      person1["age"]  = 32;

      Dictionary<String, Object> person2 = new Dictionary<String, Object>();
      person2["name"] = "Bob";
      person2["age"] = 22;

      people.Add( person1 );
      people.Add( person2 );

      unitOfWork.BulkCreate( "Person", people );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestUpdateMultipleObjectsDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<String> objectForChanges = new List<String>();

      objectForChanges.Add( "5596648F-1489-CAC5-FFAF-A1B037F7B500" );
      objectForChanges.Add( "D7FCBC10-F8A3-56A7-FF30-33FDCB404C00" );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "ChangedNEXT";

      unitOfWork.BulkUpdate( "Person", objectForChanges, changes );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteMultipleObjectsDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<String> objectsToDelete = new List<String>();

      objectsToDelete.Add( "5596648F-1489-CAC5-FFAF-A1B037F7B500" );
      objectsToDelete.Add( "D7FCBC10-F8A3-56A7-FF30-33FDCB404C00" );

      unitOfWork.BulkDelete( "Person", objectsToDelete.ToArray() );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestAddRelationDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { "ECC44968-2639-A89A-FF70-5007DBAE4100" };

      String parentObjectId = "41483F08-2C22-69A3-FFDD-208F3BA5E200";
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.AddToRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestSetRelationDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { "980D0AC3-C63F-4AA5-FFDA-B2B65ED8CD00" };

      String parentObjectId = "41483F08-2C22-69A3-FFDD-208F3BA5E200";
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.SetRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteRelationDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      String[] giftId = { "980D0AC3-C63F-4AA5-FFDA-B2B65ED8CD00" };

      String parentObjectId = "41483F08-2C22-69A3-FFDD-208F3BA5E200";
      String parentTableName = "Person";
      String relationColumnName = "Surname";

      unitOfWork.DeleteRelation( parentTableName, parentObjectId, relationColumnName, giftId );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestCreateSingleObjectClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Person person = new Person();

      person.SetName( "Joe" );
      person.SetAge( 30 );

      OpResult addPersonResult = unitOfWork.Create( person );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteSingleObjectClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Person person = new Person();
      person.SetObjectId( "C58E493B-828E-9519-FF74-90D48EA5D100" );

      unitOfWork.Delete( person );
      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestUpdateSingleObjectsCLass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Person person = new Person();
      person.SetObjectId( "12132893-2623-FB12-FFB9-31F02E593A00" );
      person.SetName( "Tommy" );

      unitOfWork.Update( person );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestCreateMultipleObjectsClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Person> people = new List<Person>();

      Person people1 = new Person();
      people1.SetAge( 11 );
      people1.SetName( "Eleven" );

      Person people2 = new Person();
      people2.SetName( "Twelve" );
      people2.SetAge( 12 );

      people.Add( people1 );
      people.Add( people2 );

      unitOfWork.BulkCreate( people );
      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteMultipleObjectsClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Person> peopleId = new List<Person>();

      Person person1 = new Person();
      person1.SetObjectId( "53D97BA8-DE4B-5736-FF73-F7F45B47F900" );

      Person person2 = new Person();
      person2.SetObjectId( "DB5997D7-1280-C4BB-FF83-8E1EE60A4300" );

      peopleId.Add( person1 );
      peopleId.Add( person2 );

      unitOfWork.BulkDelete( peopleId );

      unitOfWork.Execute();
    }
    
    [TestMethod]
    public void TestAddRelationClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Order> gifts = new List<Order>();

      Order order = new Order();
      order.SetObjectId( "980D0AC3-C63F-4AA5-FFDA-B2B65ED8CD00" );

      gifts.Add( order );

      Person person = new Person();
      person.SetObjectId( "41483F08-2C22-69A3-FFDD-208F3BA5E200" );

      String relationColumn = "Surname";
      unitOfWork.AddToRelation( person, relationColumn, gifts );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteRelationClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Order> gifts = new List<Order>();

      Order order = new Order();
      order.SetObjectId( "980D0AC3-C63F-4AA5-FFDA-B2B65ED8CD00" );

      gifts.Add( order );

      Person person = new Person();
      person.SetObjectId( "41483F08-2C22-69A3-FFDD-208F3BA5E200" );
      String relationColumn = "Surname";
      unitOfWork.DeleteRelation( person, relationColumn, gifts );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestSetRelationClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Order> gifts = new List<Order>();

      Order order = new Order();
      order.SetObjectId( "980D0AC3-C63F-4AA5-FFDA-B2B65ED8CD00" );

      gifts.Add( order );

      Person person = new Person();
      person.SetObjectId( "41483F08-2C22-69A3-FFDD-208F3BA5E200" );

      String relationColumn = "Surname";
      unitOfWork.SetRelation( person, relationColumn, gifts );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestUpdateSingleObjectOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder dQB = DataQueryBuilder.Create();

      dQB.SetWhereClause( "name='JOE'" );
      dQB.SetPageSize( 1 );

      OpResult findResult = unitOfWork.Find( "Person", dQB );

      OpResultValueReference objectRef = findResult.ResolveTo( 0 );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "Joe";

      unitOfWork.Update( objectRef, changes );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestUpdateMultipleObjectsOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder dataQueryBuilder = DataQueryBuilder.Create();

      dataQueryBuilder.SetPageSize( 100 );

      OpResult PersonsResult = unitOfWork.Find( "Person", dataQueryBuilder );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "JOEEE";

      unitOfWork.BulkUpdate( PersonsResult, changes );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteSingleObjectOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "name='JOEEE'" );

      OpResult opResult = unitOfWork.Find( "Person", queryBuilder );
      OpResultValueReference firstInvalid = opResult.ResolveTo( 0 );

      unitOfWork.Delete( firstInvalid );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteMultipleObjectsOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "name='JOEEE'" );

      OpResult invalidName = unitOfWork.Find( "Person", queryBuilder );

      unitOfWork.BulkDelete( invalidName );

      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestAddRelationOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Snow')" );

      OpResult gifts = unitOfWork.Find( "Order", queryBuilder );
      String personObjectId = "A303FAD8-7DB3-E8A8-FF31-A441D1DA6B00";
      String parentTable = "Person";
      String relationColumn = "Surname";

      unitOfWork.AddToRelation( parentTable, personObjectId, relationColumn, gifts );
      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestSetRelationOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );

      OpResult gifts = unitOfWork.Find( "Order", queryBuilder );
      String personObjectId = "B22BB493-DF4C-77BF-FF5D-96021D8DA200";
      String parentTable = "Person";
      String relationColumn = "Surname";

      unitOfWork.SetRelation( parentTable, personObjectId, relationColumn, gifts );
      unitOfWork.Execute();
    }

    [TestMethod]
    public void TestDeleteRelationOpResult()
    {
      UnitOfWork unitOfWork = new UnitOfWork();
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "LastName in ('Smith')" );

      OpResult gifts = unitOfWork.Find( "Order", queryBuilder );
      String personObjectId = "B22BB493-DF4C-77BF-FF5D-96021D8DA200";
      String parentTable = "Person";
      String relationColumn = "Surname";

      unitOfWork.DeleteRelation( parentTable, personObjectId, relationColumn, gifts );
      unitOfWork.Execute();
    }
  }
}
