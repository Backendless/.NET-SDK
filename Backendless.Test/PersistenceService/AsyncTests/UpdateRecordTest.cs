using BackendlessAPI.Data;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.UpdateEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BackendlessAPI.Test.PersistenceService.AsyncTests
{
  [TestClass]
  public class UpdateRecordTest : TestsFrame
  {
    [TestMethod]
    public void TestBasicUpdate()
    {
      RunAndAwait( () =>
        {
          BaseUpdateEntityAsync baseUpdateEntity = new BaseUpdateEntityAsync();
          baseUpdateEntity.Name = "foobar";
          baseUpdateEntity.Age = 20;

          Backendless.Persistence.Save( baseUpdateEntity,
                                        new ResponseCallback<BaseUpdateEntityAsync>( this )
                                          {
                                            ResponseHandler = savedEntity =>
                                              {
                                                savedEntity.Name = "foobar1";
                                                savedEntity.Age = 21;

                                                Backendless.Persistence.Save( savedEntity,
                                                                              new ResponseCallback<BaseUpdateEntityAsync>(
                                                                                this )
                                                                                {
                                                                                  ResponseHandler = response =>
                                                                                    {
                                                                                      Backendless.Persistence
                                                                                                 .Of<BaseUpdateEntityAsync>()
                                                                                                 .Find(
                                                                                                   new ResponseCallback
                                                                                                     <
                                                                                                     IList
                                                                                                     <BaseUpdateEntityAsync>
                                                                                                     >( this )
                                                                                                     {
                                                                                                       ResponseHandler =
                                                                                                         collection =>
                                                                                                           {
                                                                                                             BaseUpdateEntityAsync
                                                                                                               foundEntity =
                                                                                                                 collection[0];
                                                                                                             Assert.AreEqual
                                                                                                               ( savedEntity,
                                                                                                                 foundEntity,
                                                                                                                 "Server didn't update an entity" );
                                                                                                             Assert
                                                                                                               .IsNotNull(
                                                                                                                 foundEntity
                                                                                                                   .Updated,
                                                                                                                 "Server didn't set an updated field value" );

                                                                                                             CountDown();
                                                                                                           }
                                                                                                     } );
                                                                                    }
                                                                                } );
                                              }
                                          } );
        } );
    }
  }
}