using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Test.PersistenceService.AsyncEntities;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.DeleteEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.AsyncTests
{
  [TestClass]
  public class DeleteObjectTest : TestsFrame
  {
    [TestMethod]
    public void TestDeleteObjectWithWrongId()
    {
      RunAndAwait( () =>
        {
          WPPersonAsync wpPerson = GetRandomWPPerson();
          Backendless.Persistence.Save( wpPerson,
                                        new ResponseCallback<WPPersonAsync>( this )
                                          {
                                            ResponseHandler = response =>
                                              {
                                                wpPerson.ObjectId = "foobar";
                                                Backendless.Persistence.Of<WPPersonAsync>()
                                                           .Remove( wpPerson,
                                                                    new AsyncCallback<long>(
                                                                      l => Assert.Fail( "Server didn't throw an exception" ),
                                                                      fault => CheckErrorCode( 1000, fault ) ) );
                                              }
                                          } );
        } );
    }

    [TestMethod]
    public void TestDeleteObject()
    {
      RunAndAwait( () =>
        {
          var entity = new BaseDeleteEntityAsync {Name = "bot_#delete", Age = 20};
          IDataStore<BaseDeleteEntityAsync> connection = Backendless.Persistence.Of<BaseDeleteEntityAsync>();

          connection.Save( entity,
                           new ResponseCallback<BaseDeleteEntityAsync>( this )
                             {
                               ResponseHandler =
                                 savedEntity =>
                                 connection.Remove( savedEntity,
                                                    new ResponseCallback<long>( this )
                                                      {
                                                        ResponseHandler = response =>
                                                          {
                                                            connection.FindById( savedEntity.ObjectId,
                                                                             new AsyncCallback<BaseDeleteEntityAsync>(
                                                                               @async =>
                                                                               Assert.Fail( "Server probably found a result" ),
                                                                               fault => testLatch.Signal() ) );
                                                          }
                                                      } )
                             } );
        } );
    }
  }
}