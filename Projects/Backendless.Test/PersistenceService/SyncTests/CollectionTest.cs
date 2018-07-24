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
      Backendless.Persistence.Of<Person>().LoadRelations( foundPerson.ObjectId, LoadRelationsQueryBuilder<Person>.Create().SetRelationName( "Address" ) );
      var foundPerson1 = Backendless.Persistence.Of<Person>().FindById(person.ObjectId, new List<string>(){"Address"});
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

      var dataQueryBuilder = DataQueryBuilder.Create().AddProperty( "Age" ).SetPageSize( 10 ).SetOffset( 0 );

      var collection = Backendless.Persistence.Of<NextPageEntity>().Find( dataQueryBuilder.PrepareNextPage() );

      Assert.IsNotNull( collection, "Next page returned a null object" );
      Assert.AreEqual( nextPageEntities.Count, collection.Count, "Next page returned a wrong size" );

      foreach( NextPageEntity entity in nextPageEntities )
        Assert.IsTrue( collection.Contains( entity ), "Server result didn't contain expected entity" );
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

      var dataQueryBuilder = DataQueryBuilder.Create().AddProperty( "Age" ).SetPageSize( 10 );
      var collection = Backendless.Persistence.Of<GetPageEntity>().Find( dataQueryBuilder.PrepareNextPage() );

      Assert.IsNotNull( collection, "Next page returned a null object" );
      Assert.AreEqual( getPageEntities.Count, collection.Count, "Next page returned a wrong size" );

      foreach( GetPageEntity entity in getPageEntities )
        Assert.IsTrue( collection.Contains( entity ), "Server result didn't contain expected entity" );
    }
  }
}