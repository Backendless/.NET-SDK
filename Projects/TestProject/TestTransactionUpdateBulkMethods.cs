/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransactionUpdateBulkMethods
  {
    [TestMethod]
    public void TestUpdateBulkObjects_CheckError()
    {
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "Joe";

      UnitOfWork uow = new UnitOfWork();
      uow.BulkUpdate( "Wrong table name", "name != 'Joe'", changes );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsFalse( uowResult.Success );
      Assert.IsNull( uowResult.Results );
    }

    [TestMethod]
    public void TestUpdateBulkObjects_Query()
    {
      List<Dictionary<String, Object>> objectsForCreate = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> firstObject = new Dictionary<String, Object>();
      Dictionary<String, Object> secondObject = new Dictionary<String, Object>();

      firstObject[ "age" ] = 13;
      secondObject[ "age" ] = 12;
      objectsForCreate.Add( firstObject );
      objectsForCreate.Add( secondObject );
      Backendless.Data.Of( "Person" ).Create( objectsForCreate );

      UnitOfWork uow = new UnitOfWork();
      String whereClause = "age > '11'";
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "age" ] = 111;
      OpResult updatePerson = uow.BulkUpdate( "Person", whereClause, changes );

      UnitOfWorkResult uowResult = uow.Execute();
      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Double transactionResult = (Double) operationResult.Result;

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

      Assert.IsTrue( transactionResult == (Double) objectsForCreate.Count );
      Assert.IsNull( personList[ 0 ].name );
      Assert.IsNull( personList[ 1 ].name );
      Assert.IsTrue( (Int32?) personList[ 0 ].age == 111 );
      Assert.IsTrue( (Int32?) personList[ 1 ].age == 111 );

      Backendless.Data.Of( "Person" ).Remove( "age = '111'" );
    }
    [TestMethod]
    public void TestUpdateBulkObjects_OpResult()
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
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Double transactionResult = (Double) operationResult.Result;

      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

      Assert.IsTrue( transactionResult == (Double) objectsForCreate.Count );
      Assert.IsNull( personList[ 0 ].age );
      Assert.IsNull( personList[ 1 ].age );
      Assert.IsTrue( (String) personList[ 0 ].name == "JOEEE" );
      Assert.IsTrue( (String) personList[ 1 ].name == "JOEEE" );

      Backendless.Data.Of( "Person" ).Remove( "name = 'JOEEE'" );
    }

    [TestMethod]
    public void TestUpdateBulkObjects_Dictionary()
    {
      Dictionary<String, Object> objData_First = new Dictionary<String, Object>();
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
      OperationResult operationResult = result[ updatePersonsObj.OpResultId ];
      Double transactionResult = (Double) operationResult.Result;
      IList<Person> personList = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() );

      Assert.IsTrue( transactionResult == (Double) personList.Count );
      Assert.IsNull( personList[ 0 ].name );
      Assert.IsNull( personList[ 1 ].name );
      Assert.IsTrue( personList[ 0 ].age == 100 );
      Assert.IsTrue( personList[ 1 ].age == 100 );

      Backendless.Data.Of( "Person" ).Remove( "age = '100'" );
    }
  }
}
*/