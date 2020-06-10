using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction;

namespace TestProject
{
  [TestClass]
  public class TestTransactionCreateMethods
  {
    [TestMethod]
    public void TestCreateSingleObject_Dictionary()
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
      OperationResult operationResult = result[ createPersonResult.OpResultId ];

      Dictionary<Object, Object> transactionResult = (Dictionary<Object, Object>) operationResult.Result;

      Assert.IsTrue( "Joe" == (String) transactionResult[ "name" ] );
      Assert.IsTrue( 23 == (Int32) transactionResult[ "age" ] );

      Backendless.Data.Of( "Person" ).Remove( "name = 'Joe'" );
    }

    [TestMethod]
    public void TestCreateSingleObject_Class()
    {
      UnitOfWork unitOfWork = new UnitOfWork();

      Person person = new Person();

      person.name = "Joe";
      person.age = 30;

      OpResult addPersonResult = unitOfWork.Create( person );
      UnitOfWorkResult uowResult = unitOfWork.Execute();
      Assert.IsTrue( uowResult.Success );
      Assert.IsNotNull( uowResult.Results );

      Person personObject = Backendless.Data.Of<Person>().Find( DataQueryBuilder.Create() )[ 0 ];
      Assert.IsTrue( person.age == personObject.age );
      Assert.IsTrue( person.name == personObject.name );

      Backendless.Data.Of( "Person" ).Remove( "name = '" + personObject.name + "'" );
    }

    [TestMethod]
    public void TestCreateSingleObject_CheckError()
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
  }
}
