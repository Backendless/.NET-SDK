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
  public class TestTransactionCreateMethods
  {
    [TestMethod]
    public void TestCreateSingleObjectDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;
      OpResult opResult = unitOfWork.Create( "Person", pers );

      UnitOfWorkResult unitOfWorkRes = unitOfWork.Execute();

      Assert.IsTrue( ((Dictionary<Object, Object>) unitOfWorkRes.Results[ "createPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestCreateMultipleObjectsDictionary()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Dictionary<String, Object>> people = new List<Dictionary<String, Object>>();

      Dictionary<String, Object> person1 = new Dictionary<String, Object>();
      person1[ "name" ] = "Mary";
      person1[ "age" ] = 32;

      Dictionary<String, Object> person2 = new Dictionary<String, Object>();
      person2[ "name" ] = "Bob";
      person2[ "age" ] = 22;

      people.Add( person1 );
      people.Add( person2 );

      unitOfWork.BulkCreate( "Person", people );

      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "createBulkPerson1" ] ).Count == 2 );
    }

    [TestMethod]
    public void TestCreateSingleObjectClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Person person = new Person();

      person.SetName( "Joe" );
      person.SetAge( 30 );

      OpResult addPersonResult = unitOfWork.Create( person );
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "createPerson1" ] ).Count == 2 );
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
      UnitOfWorkResult result = unitOfWork.Execute();
      Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "createBulkPerson1" ] ).Count == 2 );
    }
  }
}
