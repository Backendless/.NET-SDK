using System;
using BackendlessAPI.Data;
using BackendlessAPI.Test.PersistenceService.Entities;
using BackendlessAPI.Test.PersistenceService.Entities.DeleteEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.SyncTests
{
  [TestClass]
  public class DeleteObjectTest : TestsFrame
  {
    [TestMethod]
    public void TestDeleteObjectWithWrongId()
    {
      WPPerson wpPerson = GetRandomWPPerson();
      Backendless.Persistence.Save( wpPerson );
      wpPerson.ObjectId = "foobar";

      try
      {
        Backendless.Persistence.Of<WPPerson>().Remove( wpPerson );
        Assert.Fail( "Server didn't throw an exception" );
      }
      catch( System.Exception e )
      {
        CheckErrorCode( 1000, e );
      }
    }

    [TestMethod]
    public void TestDeleteObject()
    {
      var entity = new BaseDeleteEntity {Name = "bot_#delete", Age = 20};
      IDataStore<BaseDeleteEntity> connection = Backendless.Persistence.Of<BaseDeleteEntity>();

      BaseDeleteEntity savedEntity = connection.Save( entity );
      connection.Remove( savedEntity );

      try
      {
        connection.FindById( savedEntity.ObjectId );
        Assert.Fail( "Server probably found a result" );
      }
      catch( System.Exception )
      {
      }
    }
  }
}