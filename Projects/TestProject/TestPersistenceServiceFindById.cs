using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Async;

namespace TestProject
{
  [TestClass]
  public class TestPersistenceServiceFindById
  {
    [ClassInitialize]
    public static void ClassInitialize( TestContext  context )
    {
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
      TestInitialization.DeleteTable( "Person" );
    }

    [TestCleanup]
    public void TestCleanup()
    {
      Backendless.Data.Of<Person>().Remove( "age='15'" );
    }

    [TestMethod]
    public void TestPersistenceServiceFindById_StringId()
    {
      List<Dictionary<String, Object>> listPerson = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 15;
      person[ "name" ] = "Alexandra";

      List<String> listId = new List<String>();
      listPerson.Add( person );
      listId = (List<String>) Backendless.Data.Of( "Person" ).Create( listPerson );

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );
      Dictionary<String, Object> result = Backendless.Data.Of( "Person" ).FindById( listId[ 0 ], queryBuilder );

      Assert.IsNotNull( result );
      Assert.IsFalse( result.ContainsKey( "name" ) );
      Assert.IsTrue( result.ContainsKey( "age" ) );
    }

    [TestMethod]
    public void TestPersistenceServiceFindById_StringId_Async()
    {
      List<Dictionary<String, Object>> listPerson = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 15;
      person[ "name" ] = "Alexandra";

      List<String> listId = new List<String>();
      listPerson.Add( person );
      listId = (List<String>) Backendless.Data.Of( "Person" ).Create( listPerson );

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );
      Backendless.Data.Of( "Person" ).FindById( listId[ 0 ], queryBuilder, new AsyncCallback<Dictionary<String, Object>>(
      callback =>
      {
        Assert.IsNotNull( callback );
        Assert.IsFalse( callback.ContainsKey( "name" ) );
        Assert.IsTrue( callback.ContainsKey( "age" ) );
      },
      fault =>
      {
        throw new ArgumentException( "Error" );
      } ) );
    }

    [TestMethod]
    public void TestPersistenceServiceFindById_Dictionary()
    {
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 15;
      person[ "name" ] = "Alexandra";

      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );
      Dictionary<String, Object> result = Backendless.Data.Of( "Person" ).FindById( person, queryBuilder );

      Assert.IsNotNull( result );
      Assert.IsFalse( result.ContainsKey( "name" ) );
      Assert.IsTrue( result.ContainsKey( "age" ) );
    }

    [TestMethod]
    public void TestPersistenceServiceFindById_Dictionary_Async()
    {
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 15;
      person[ "name" ] = "Alexandra";

      person["objectId"] = Backendless.Data.Of( "Person" ).Save( person )["objectId"];

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );
      Backendless.Data.Of( "Person" ).FindById( person, queryBuilder, new AsyncCallback<Dictionary<string, object>>(
      callback =>
      {
        Assert.IsNotNull( callback );
        Assert.IsFalse( callback.ContainsKey( "name" ) );
        Assert.IsTrue( callback.ContainsKey( "age" ) );
      },
      fault =>
      {
        throw new ArgumentException( "Error" );
      } ) );
    }
  }
}
