using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;
using Weborb.Writer;
using System.Linq;

namespace TestProject
{
  [TestClass]
  public class TestTransactionCreateMethods
  {
    [TestMethod]
    public void TestCreateSingleObjectDictionary()
    {
      UnitOfWork uow = new UnitOfWork();

      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;
      OpResult createPersonResult = uow.Create( "Person", pers );

      UnitOfWorkResult unitOfWorkRes = uow.Execute();

      Assert.IsTrue( unitOfWorkRes.Success );
      Assert.IsNotNull( unitOfWorkRes.Results );
      
      Dictionary<String, OperationResult> result = unitOfWorkRes.Results;
      OperationResult operationResult = result[ createPersonResult.GetOpResultId() ];

      Dictionary<Object, Object> transactionResult = (Dictionary<Object,Object>) operationResult.Result;

      Assert.IsTrue( "Joe" == (String) transactionResult[ "name" ] );
      Assert.IsTrue( 23 == (Int32) transactionResult[ "age" ] );

      Backendless.Data.Of( "Person" ).Remove( "name = 'Joe'" );
    }

    [TestMethod]
    public void TestCreateMultipleObjectsDictionary()
    {
      UnitOfWork uow = new UnitOfWork();

      List<Dictionary<String, Object>> people = new List<Dictionary<String, Object>>();

      Dictionary<String, Object> person1 = new Dictionary<String, Object>();
      person1[ "name" ] = "Mary";
      person1[ "age" ] = 32;

      Dictionary<String, Object> person2 = new Dictionary<String, Object>();
      person2[ "name" ] = "Bob";
      person2[ "age" ] = 22;

      people.Add( person1 );
      people.Add( person2 );

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
      if( transactionsObjID[ 0 ] != personList[ 1 ].GetObjectId() )
      {
        iteratorI++;
        iteratorJ--;
      }
      Assert.IsTrue( transactionsObjID[ iteratorI ] == personList[ 1 ].GetObjectId() );
      Assert.IsTrue( transactionsObjID[ iteratorJ ] == personList[ 0 ].GetObjectId() );
      Assert.IsTrue( personList[ iteratorJ ].GetAge() == (Int32) person1[ "age" ] );
      Assert.IsTrue( personList[ iteratorI ].GetAge() == (Int32) person2[ "age" ] );
      Assert.IsTrue( personList[ iteratorJ ].GetName() ==(String) person1[ "name" ] );
      Assert.IsTrue( personList[ iteratorI ].GetName() == (String) person2[ "name" ] );

      Backendless.Data.Of( "Person" ).Remove( "name = '" + personList[ 0 ].GetName() + "'" );
      Backendless.Data.Of( "Person" ).Remove( "name = '" + personList[ 1 ].GetName() + "'" );
    }

    [TestMethod]
    public void TestCreateSingleObjectClass()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Person person = new Person();

      person.SetName( "Joe" );
      person.SetAge( 30 );

      OpResult addPersonResult = unitOfWork.Create( person );
      UnitOfWorkResult uowResult = unitOfWork.Execute();
      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Person personObject = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() )[0];
      Assert.IsTrue( person.GetAge() == personObject.GetAge() );
      Assert.IsTrue( person.GetName() == personObject.GetName() );

      Backendless.Data.Of( "Person" ).Remove( "name = '"+personObject.GetName() + "'" );
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

      OpResult createPersonObjects = unitOfWork.BulkCreate( people );
      UnitOfWorkResult uowResult = unitOfWork.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );
      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ createPersonObjects.GetOpResultId() ];
      String[] transactionsObjID = (String[]) operationResult.Result;

      Assert.IsTrue( transactionsObjID[ 0 ] == personList[ 0 ].GetObjectId() || transactionsObjID[ 0 ] == personList[ 1 ].GetObjectId() );
      Assert.IsTrue( transactionsObjID[ 1 ] == personList[ 0 ].GetObjectId() || transactionsObjID[ 1 ] == personList[ 1 ].GetObjectId() );
      Backendless.Data.Of( "Person" ).Remove( "name = '" + personList[ 0 ].GetName() + "'" );
      Backendless.Data.Of( "Person" ).Remove( "name = '" + personList[ 1 ].GetName() + "'" );
    }

    [TestMethod]
    public void TestCreateCheckError()
    {
      UnitOfWork uow = new UnitOfWork();

      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      pers[ "name" ] = "Joe";
      pers[ "age" ] = 23;
      uow.Create( "NonexistentTable", pers );

      UnitOfWorkResult unitOfWorkRes = uow.Execute();

      Assert.IsFalse( unitOfWorkRes.Success );
      Assert.IsNull( unitOfWorkRes.Results );
    }

    [TestMethod]
    public void TestCreateBulkCheckError()
    {
      UnitOfWork uow = new UnitOfWork();
      List<Dictionary<String, Object>> personList = new List<Dictionary<String, Object>>();

      Dictionary<String, Object> person_One = new Dictionary<String, Object>();
      person_One[ "name" ] = "Joe";
      person_One[ "age" ] = 27;

      Dictionary<String, Object> person_Two = new Dictionary<String, Object>();
      person_Two[ "name" ] = "Bob";
      person_Two[ "age" ] = 30;

      personList.Add( person_One );
      personList.Add( person_Two );

      uow.BulkCreate( "NonexistentTable", personList );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsFalse( uowResult.Success );
      Assert.IsNull( uowResult.Results );
    }
  }
}
