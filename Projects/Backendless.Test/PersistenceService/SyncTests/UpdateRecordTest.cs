using System;
using BackendlessAPI.Test.PersistenceService.Entities.UpdateEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.SyncTests
{
  [TestClass]
  public class UpdateRecordTest : TestsFrame
  {
    [TestMethod]
    public void TestBasicUpdate()
    {
      BaseUpdateEntity baseUpdateEntity = new BaseUpdateEntity();
      baseUpdateEntity.Name = "foobar";
      baseUpdateEntity.Age = 20;

      BaseUpdateEntity savedEntity = Backendless.Persistence.Save( baseUpdateEntity );
      savedEntity.Name = "foobar1";
      savedEntity.Age = 21;

      Backendless.Persistence.Save( savedEntity );

      BaseUpdateEntity foundEntity = Backendless.Persistence.Of<BaseUpdateEntity>().Find()[0];

      Assert.AreEqual( savedEntity, foundEntity, "Server didn't update an entity" );
      Assert.IsNotNull( foundEntity.Updated, "Server didn't set an updated field value" );
    }
  }
}