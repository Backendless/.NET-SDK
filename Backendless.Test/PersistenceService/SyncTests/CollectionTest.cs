using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BackendlessAPI.Data;
using BackendlessAPI.Persistence;
using BackendlessAPI.Test.PersistenceService.Entities.CollectionEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.SyncTests
{
  public class Person: BackendlessEntity
  {
    public Address Address { get; set; }
    public string Name { get; set; }
  }

  public class Address: BackendlessEntity
  {
    public string Street { get; set; }
  }

  [TestClass]
  public class CollectionTest: TestsFrame
  {
    [TestMethod]
    public void FooTest()
    {
      var address = new Address(){Street = "myStreet"};
      var person = new Person() {Address = address, Name = "Bob"};

      var savedPerson = Backendless.Persistence.Save( person );
      var foundPerson = Backendless.Persistence.Of<Person>().FindById( person.ObjectId );
      Backendless.Persistence.Of<Person>().LoadRelations( foundPerson, new List<string>(){"Address"} );
      var foundPerson1 = Backendless.Persistence.Of<Person>().FindById(person.ObjectId, new List<string>(){"Address"});
      var f = "sdsdsd";
    }

    [TestMethod]
    public void TestCollectionNextPage()
    {
      var nextPageEntities = new List<NextPageEntity>();

      for( int i = 10; i < 30; i++ )
      {
        var entity = new NextPageEntity {Name = "name#" + i, Age = 20 + i};
        Backendless.Persistence.Save( entity );

        if( i >= 20 )
          nextPageEntities.Add( entity );

        Thread.Sleep( 1000 );
      }

      var dataQuery = new BackendlessDataQuery( new QueryOptions( 10, 0, "Age" ) );
      var collection = Backendless.Persistence.Of<NextPageEntity>().Find( dataQuery ).NextPage();

      Assert.IsNotNull( collection, "Next page returned a null object" );
      Assert.IsNotNull( collection.GetCurrentPage(), "Next page contained a wrong data size" );
      Assert.AreEqual( nextPageEntities.Count, collection.GetCurrentPage().Count, "Next page returned a wrong size" );

      foreach( NextPageEntity entity in nextPageEntities )
        Assert.IsTrue( collection.GetCurrentPage().Contains( entity ), "Server result didn't contain expected entity" );
    }

    [TestMethod]
    public void TestCollectionGetPage()
    {
      var getPageEntities = new List<GetPageEntity>();

      for( int i = 10; i < 30; i++ )
      {
        var entity = new GetPageEntity {Name = "name#" + i, Age = 20 + i};
        Backendless.Persistence.Save( entity );

        if( i > 19 && i < 30 )
          getPageEntities.Add( entity );

        Thread.Sleep( 1000 );
      }

      var dataQuery = new BackendlessDataQuery( new QueryOptions( 10, 0, "Age" ) );
      var collection = Backendless.Persistence.Of<GetPageEntity>().Find( dataQuery ).GetPage( 10, 10 );

      Assert.IsNotNull( collection, "Next page returned a null object" );
      Assert.IsNotNull( collection.GetCurrentPage(), "Next page contained a wrong data size" );
      Assert.AreEqual( getPageEntities.Count, collection.GetCurrentPage().Count, "Next page returned a wrong size" );

      foreach( GetPageEntity entity in getPageEntities )
        Assert.IsTrue( collection.GetCurrentPage().Contains( entity ), "Server result didn't contain expected entity" );
    }
  }
}