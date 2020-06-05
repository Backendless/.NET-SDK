using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransactionCreateBulkMethods
  {
    [TestMethod]
    public void TestCreateBulkObjects_Dictionary()
    {
      List<Dictionary<String, Object>> people = new List<Dictionary<String, Object>>();

      Dictionary<String, Object> person1 = new Dictionary<String, Object>();
      person1[ "name" ] = "Mary";
      person1[ "age" ] = 32;

      Dictionary<String, Object> person2 = new Dictionary<String, Object>();
      person2[ "name" ] = "Bob";
      person2[ "age" ] = 22;

      people.Add( person1 );
      people.Add( person2 );

      UnitOfWork uow = new UnitOfWork();
      OpResult createPersonsObj = uow.BulkCreate( "Person", people );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );
      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ createPersonsObj.GetOpResultId() ];
      String[] transactionsObjID = (String[]) operationResult.Result;

      int iteratorI = 0;
      int iteratorJ = 1;
      if( transactionsObjID[ 0 ] != personList[ 1 ].objectId )
      {
        iteratorI++;
        iteratorJ--;
      }
      Assert.IsTrue( transactionsObjID[ iteratorI ] == personList[ 1 ].objectId );
      Assert.IsTrue( transactionsObjID[ iteratorJ ] == personList[ 0 ].objectId );
      Assert.IsTrue( personList[ iteratorJ ].age == (Int32) person1[ "age" ] );
      Assert.IsTrue( personList[ iteratorI ].age == (Int32) person2[ "age" ] );
      Assert.IsTrue( personList[ iteratorJ ].name == (String) person1[ "name" ] );
      Assert.IsTrue( personList[ iteratorI ].name == (String) person2[ "name" ] );

      Backendless.Data.Of( "Person" ).Remove( "name = '" + personList[ 0 ].name + "'" );
      Backendless.Data.Of( "Person" ).Remove( "name = '" + personList[ 1 ].name + "'" );
    }

    [TestMethod]
    public void TestCreateBulkObjects_Class()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      List<Person> people = new List<Person>();

      Person people1 = new Person();
      people1.age = 11;
      people1.name = "Eleven";

      Person people2 = new Person();
      people2.name = "Twelve";
      people2.age = 12;

      people.Add( people1 );
      people.Add( people2 );

      OpResult createPersonObjects = unitOfWork.BulkCreate( people );
      UnitOfWorkResult uowResult = unitOfWork.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );
      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ createPersonObjects.GetOpResultId() ];
      String[] transactionsObjID = (String[]) operationResult.Result;

      Assert.IsTrue( transactionsObjID[ 0 ] == personList[ 0 ].objectId || transactionsObjID[ 0 ] == personList[ 1 ].objectId );
      Assert.IsTrue( transactionsObjID[ 1 ] == personList[ 0 ].objectId || transactionsObjID[ 1 ] == personList[ 1 ].objectId );
      Backendless.Data.Of( "Person" ).Remove( "name = '" + personList[ 0 ].name + "'" );
      Backendless.Data.Of( "Person" ).Remove( "name = '" + personList[ 1 ].name + "'" );
    }

    [TestMethod]
    public void TestCreateBulkObjects_CheckError()
    {
      UnitOfWork uow = new UnitOfWork();

      List<Dictionary<String, Object>> persMap = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;

      persMap.Add( pers );

      uow.BulkCreate( "Wrong table name", persMap );

      UnitOfWorkResult unitOfWorkRes = uow.Execute();

      Assert.IsFalse( unitOfWorkRes.Success );
      Assert.IsNull( unitOfWorkRes.Results );
    }
  }
}
