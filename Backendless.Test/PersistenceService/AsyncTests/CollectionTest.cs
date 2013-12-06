using System.Collections.Generic;
using System.Threading;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Persistence;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.CollectionEntities;
using BackendlessAPI.Test.PersistenceService.Entities.CollectionEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.AsyncTests
{
  [TestClass]
  public class CollectionTest : TestsFrame
  {
    [TestMethod]
    public void TestCollectionNextPage()
    {
      RunAndAwait( () =>
        {
          var nextPageEntities = new List<NextPageEntityAsync>();
          var latch = new CountdownEvent( 20 );
          for( int i = 10; i < 30; i++ )
          {
            var entity = new NextPageEntityAsync {Name = "name#" + i, Age = 20 + i};
            Backendless.Persistence.Save( entity,
                                          new AsyncCallback<NextPageEntityAsync>( response => latch.Signal(), fault =>
                                            {
                                              for( int j = 0; j < latch.CurrentCount; j++ )
                                                latch.Signal();
                                            } ) );
            if( i >= 20 )
              nextPageEntities.Add( entity );
          }
          latch.Wait();

          var dataQuery = new BackendlessDataQuery( new QueryOptions( 10, 0, "Age" ) );
          Backendless.Persistence.Of<NextPageEntityAsync>()
                     .Find( dataQuery,
                            new ResponseCallback<BackendlessCollection<NextPageEntityAsync>>( this )
                              {
                                ResponseHandler =
                                  response =>
                                  response.NextPage(
                                    new ResponseCallback<BackendlessCollection<NextPageEntityAsync>>( this )
                                      {
                                        ResponseHandler = collection =>
                                          {
                                            Assert.IsNotNull( collection, "Next page returned a null object" );
                                            Assert.IsNotNull( collection.GetCurrentPage(),
                                                              "Next page contained a wrong data size" );
                                            Assert.AreEqual( nextPageEntities.Count, collection.GetCurrentPage().Count,
                                                             "Next page returned a wrong size" );

                                            foreach( NextPageEntityAsync entity in nextPageEntities )
                                              Assert.IsTrue( collection.GetCurrentPage().Contains( entity ),
                                                             "Server result didn't contain expected entity" );

                                            CountDown();
                                          }
                                      } )
                              } );
        } );
    }

    [TestMethod]
    public void TestCollectionGetPage()
    {
      RunAndAwait( () =>
        {
          var getPageEntities = new List<GetPageEntityAsync>();
          var latch = new CountdownEvent( 20 );
          for( int i = 10; i < 30; i++ )
          {
            var entity = new GetPageEntityAsync {Name = "name#" + i, Age = 20 + i};
            Backendless.Persistence.Save( entity,
                                          new AsyncCallback<GetPageEntityAsync>( response => latch.Signal(), fault =>
                                            {
                                              for( int j = 0; j < latch.CurrentCount; j++ )
                                                latch.Signal();
                                            } ) );

            if( i > 19 && i < 30 )
              getPageEntities.Add( entity );
          }
          latch.Wait();

          var dataQuery = new BackendlessDataQuery( new QueryOptions( 10, 0, "Age" ) );
          Backendless.Persistence.Of<GetPageEntityAsync>()
                     .Find( dataQuery,
                            new ResponseCallback<BackendlessCollection<GetPageEntityAsync>>( this )
                              {
                                ResponseHandler =
                                  response =>
                                  response.GetPage( 10, 10,
                                                    new ResponseCallback<BackendlessCollection<GetPageEntityAsync>>( this )
                                                      {
                                                        ResponseHandler = collection =>
                                                          {
                                                            Assert.IsNotNull( collection, "Next page returned a null object" );
                                                            Assert.IsNotNull( collection.GetCurrentPage(),
                                                                              "Next page contained a wrong data size" );
                                                            Assert.AreEqual( getPageEntities.Count,
                                                                             collection.GetCurrentPage().Count,
                                                                             "Next page returned a wrong size" );

                                                            foreach( GetPageEntityAsync entity in getPageEntities )
                                                              Assert.IsTrue(
                                                                collection.GetCurrentPage().Contains( entity ),
                                                                "Server result didn't contain expected entity" );

                                                            CountDown();
                                                          }
                                                      } )
                              } );
        } );
    }
  }
}