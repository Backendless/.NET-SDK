/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransactionUpdateMethods
  {
    [TestMethod]
    public void TestUpdateSingleObject_Dictionary()
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
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.IsNull( transactionResult[ "name" ] );
      Assert.IsTrue( 35 == (Int32) transactionResult[ "age" ] );

      Backendless.Data.Of( "Person" ).Remove( "age = '35'" );
    }
    
    [TestMethod]
    public void TestUpdateSingleObject_CLass()
    {
      Person person = new Person();
      person.name = "Joe";

      IList<Person> personList = new List<Person>();
      personList.Add( person );
      person.objectId = Backendless.Data.Of<Person>().Create( personList )[ 0 ];

      UnitOfWork unitOfWork = new UnitOfWork();
      person.name = "Tommy";

      OpResult updatePerson = unitOfWork.Update( person );
      UnitOfWorkResult uowResult = unitOfWork.Execute();

      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Dictionary<String, OperationResult> result = uowResult.Results;
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.IsTrue( (String) transactionResult[ "name" ] == "Tommy" );
      Assert.IsTrue( (String) transactionResult[ "objectId" ] == person.objectId );

      Backendless.Data.Of( "Person" ).Remove( "name = 'Tommy'" );
    }

    [TestMethod]
    public void TestUpdateSingleObject_OpResult()
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
      OperationResult operationResult = result[ updatePerson.OpResultId ];
      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.IsTrue( (Int32) transactionResult[ "age" ] == 100500 );
      Assert.IsTrue( (String) transactionResult[ "objectId" ] == createResult[ 0 ] );
      Backendless.Data.Of( "Person" ).Remove( "age = '100500'" );
      Backendless.Data.Of( "Person" ).Remove( "age = '1212'" );
    }

    [TestMethod]
    public void TestUpdateSingleObject_CheckError()
    {
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ "name" ] = "Joee";
      changes[ "age" ] = 18;

      UnitOfWork uow = new UnitOfWork();

      uow.Update( "Wrong table name", changes );

      UnitOfWorkResult uowResult = uow.Execute();

      Assert.IsFalse( uowResult.Success );
      Assert.IsNull( uowResult.Results );
    }
  }
}
*/