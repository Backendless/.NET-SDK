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
  public class TestTransactionUpdateMethods
  {
    [TestMethod]
    public void TestUpdateSingleObjectDictionary()
    { 
      Dictionary<String, Object> objData = new Dictionary<String, Object>();
      IList<Dictionary<String, Object>> creator = new List<Dictionary<String, Object>>(); 
      objData[ "age" ] = 17;
      creator.Add( objData );
      Backendless.Data.Of( "Person" ).Create( creator );

      objData.Clear();
      UnitOfWork uow = new UnitOfWork();

      objData[ "age" ] = 35;
      objData[ "objectId" ] = Backendless.Data.Of( "Person" ).FindFirst()[ "objectId" ];
      OpResult updatePerson = uow.Update( "Person", objData );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.GetOpResultId() ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.IsNull( transactionResult[ "name" ] );
      Assert.IsTrue( 35 == (Int32) transactionResult[ "age" ] );

      Backendless.Data.Of( "Person" ).Remove( "age = '35'" );
    }

    [TestMethod]
    public void TestUpdateMultipleObjectsDictionary()
    {
      Dictionary<string, Object> objData_First = new Dictionary<String, Object>();
      Dictionary<String, Object> objData_Second = new Dictionary<String, Object>();
      List<Dictionary<String, Object>> listPerson = new List<Dictionary<String, Object>>();

      objData_First[ "age" ] = 19;
      objData_Second[ "age" ] = 25;
      listPerson.Add( objData_First );
      listPerson.Add( objData_Second );
      List<String> objectForChanges = (List<String>) Backendless.Data.Of( "Person" ).Create( listPerson );

      UnitOfWork uow = new UnitOfWork();
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 100;

      OpResult updatePersonsObj = uow.BulkUpdate( "Person", objectForChanges, changes );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePersonsObj.GetOpResultId() ];
      Double transactionResult = (Double) operationResult.Result;
      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

      Assert.IsTrue( transactionResult == (Double) personList.Count );
      Assert.IsNull( personList[ 0 ].GetName() );
      Assert.IsNull( personList[ 1 ].GetName() );
      Assert.IsTrue( personList[ 0 ].GetAge() == 100 );
      Assert.IsTrue( personList[ 1 ].GetAge() == 100 );

      Backendless.Data.Of( "Person" ).Remove( "age = '100'" );
    }

    [TestMethod]
    public void TestUpdateSingleObjectsCLass()
    {
      Person person = new Person();
      person.SetName( "Joe" );

      IList<Person> personList = new List<Person>();
      personList.Add( person );
      person.SetObjectId( Backendless.Data.Of<Person>().Create( personList )[ 0 ] );

      UnitOfWork unitOfWork = new UnitOfWork();
      person.SetName( "Tommy" );

      OpResult updatePerson = unitOfWork.Update( person );
      UnitOfWorkResult uowResult = unitOfWork.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.GetOpResultId() ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.IsTrue( (String) transactionResult[ "name" ] == "Tommy" );
      Assert.IsTrue( (String) transactionResult[ "objectId" ] == person.GetObjectId() );

      Backendless.Data.Of( "Person" ).Remove( "name = 'Tommy'" );
    }

    [TestMethod]
    public void TestUpdateSingleObjectWhereClause()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> objectData = new Dictionary<String, Object>();
      Dictionary<String, Object> wrongObject = new Dictionary<String, Object>();

      wrongObject[ "age" ] = 1212;
      objectData[ "age" ] = 12;
      objectsForCreate.Add( objectData );
      objectsForCreate.Add( wrongObject );
      IList<String> createResult = Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      UnitOfWork uow = new UnitOfWork();

      DataQueryBuilder dQB = DataQueryBuilder.Create();

      dQB.SetWhereClause( "age = 12" );
      dQB.SetPageSize( 1 );

      OpResult findResult = uow.Find( "Person", dQB );

      OpResultValueReference objectRef = findResult.ResolveTo( 0 );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 100500;

      OpResult updatePerson = uow.Update( objectRef, changes );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.GetOpResultId() ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.IsTrue( (Int32) transactionResult[ "age" ] == 100500 );
      Assert.IsTrue( (String) transactionResult[ "objectId" ] == createResult[0] );
      Backendless.Data.Of( "Person" ).Remove( "age = '100500'" );
      Backendless.Data.Of( "Person" ).Remove( "age = '1212'" );
    }

    [TestMethod]
    public void TestUpdateMultipleObjectsWhereClause()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> objectFirst = new Dictionary<String, Object>();
      Dictionary<String, Object> objectSecond = new Dictionary<String, Object>();
      objectFirst[ "name" ] = "Joe";
      objectSecond[ "name" ] = "Joe";
      objectsForCreate.Add( objectFirst );
      objectsForCreate.Add( objectSecond );
      Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      UnitOfWork uow = new UnitOfWork();

      DataQueryBuilder dataQueryBuilder = DataQueryBuilder.Create();

      dataQueryBuilder.SetWhereClause( "name = 'Joe'" );

      OpResult personsResult = uow.Find( "Person", dataQueryBuilder );

      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "JOEEE";

      OpResult updatePerson = uow.BulkUpdate( personsResult, changes );
      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.GetOpResultId() ];
      Double transactionResult = (Double) operationResult.Result;

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

      Assert.IsTrue( transactionResult == (Double) objectsForCreate.Count );
      Assert.IsNull( personList[ 0 ].GetAge() );
      Assert.IsNull( personList[ 1 ].GetAge() );
      Assert.IsTrue( (String) personList[ 0 ].GetName() == "JOEEE" );
      Assert.IsTrue( (String) personList[ 1 ].GetName() == "JOEEE" );

      Backendless.Data.Of( "Person" ).Remove( "name = 'JOEEE'" );
    }           

    //[TestMethod]
    //public void TestUpdateMultipleObjectsQuery()
    //{
    //  UnitOfWork unitOfWork = new UnitOfWork();
    //  String whereClause = "age = '11'";
    //  Dictionary<String, Object> changes = new Dictionary<String, Object>();

    //  changes[ "age" ] = 111;
    //  unitOfWork.BulkUpdate( "Person", whereClause, changes );

    //  UnitOfWorkResult result = unitOfWork.Execute();
    //  Assert.IsTrue( ( (Dictionary<Object, Object>) result.Results[ "updateBulkPerson1" ] ).Count == 2 );
    //}
  }
}
