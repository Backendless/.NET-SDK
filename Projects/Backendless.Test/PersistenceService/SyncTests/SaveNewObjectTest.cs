using System;
using System.Globalization;
using BackendlessAPI.Exception;
using BackendlessAPI.Test.PersistenceService.Entities;
using BackendlessAPI.Test.PersistenceService.Entities.PrimitiveEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.SyncTests
{
  [TestClass]
  public class SaveNewObjectTest : TestsFrame
  {
    [TestMethod]
    public void TestSaveEntityToANewDataBase()
    {
      var uniqueWpPerson = new UniqueWPPerson {Age = 16, Name = "John", Birthday = DateTime.Now};
      UniqueWPPerson savedWPPerson = Backendless.Persistence.Save( uniqueWpPerson );
      Assert.IsNotNull( savedWPPerson, "Server returned a null result" );
      Assert.IsNotNull( savedWPPerson.Name, "Returned object doesn't have expected field" );
      Assert.IsNotNull( savedWPPerson.ObjectId, "Returned object doesn't have expected field id" );
      Assert.IsNotNull( savedWPPerson.Created, "Returned object doesn't have expected field created" );
      Assert.AreEqual( uniqueWpPerson.Name, savedWPPerson.Name, "Returned object has wrong field value" );
      Assert.AreEqual( uniqueWpPerson.Age, savedWPPerson.Age, "Returned object has wrong field value" );
      Assert.IsTrue( (savedWPPerson.Birthday.Ticks - uniqueWpPerson.Birthday.Ticks) < 1000,
                     "Returned object has wrong field value" );
    }

    [TestMethod]
    public void TestSaveStringEntity()
    {
      StringEntity entity = new StringEntity {StringField = "foobar"};
      StringEntity savedEntity = Backendless.Persistence.Save( entity );
      Assert.IsNotNull( savedEntity, "Server returned a null result" );
      Assert.IsNotNull( savedEntity.StringField, "Returned object doesn't have expected field" );
      Assert.IsNotNull( savedEntity.ObjectId, "Returned object doesn't have expected field id" );
      Assert.IsNotNull( savedEntity.Created, "Returned object doesn't have expected field created" );
      Assert.AreEqual( entity.StringField, savedEntity.StringField, "Returned object has wrong field value" );
    }

    [TestMethod]
    public void TestSaveBooleanEntity()
    {
      var entity = new BooleanEntity {BooleanField = false};
      BooleanEntity savedEntity = Backendless.Persistence.Save( entity );
      Assert.IsNotNull( savedEntity, "Server returned a null result" );
      Assert.IsNotNull( savedEntity.ObjectId, "Returned object doesn't have expected field id" );
      Assert.IsNotNull( savedEntity.Created, "Returned object doesn't have expected field created" );
      Assert.AreEqual( entity.BooleanField, savedEntity.BooleanField, "Returned object has wrong field value" );
    }

    [TestMethod]
    public void TestSaveDateEntity()
    {
      var entity = new DateEntity {DateField = DateTime.Now};
      DateEntity savedEntity = Backendless.Persistence.Save( entity );
      Assert.IsNotNull( savedEntity, "Server returned a null result" );
      Assert.IsNotNull( savedEntity.DateField, "Returned object doesn't have expected field" );
      Assert.IsNotNull( savedEntity.ObjectId, "Returned object doesn't have expected field id" );
      Assert.IsNotNull( savedEntity.Created, "Returned object doesn't have expected field created" );
      Assert.IsTrue( savedEntity.DateField.Ticks - entity.DateField.Ticks < 1000,
                     "Returned object has wrong field value" );
    }

    [TestMethod]
    public void TestSaveIntEntity()
    {
      var entity = new IntEntity {IntField = 16};
      IntEntity savedEntity = Backendless.Persistence.Save( entity );
      Assert.IsNotNull( savedEntity, "Server returned a null result" );
      Assert.IsNotNull( savedEntity.ObjectId, "Returned object doesn't have expected field id" );
      Assert.IsNotNull( savedEntity.Created, "Returned object doesn't have expected field created" );
      Assert.AreEqual( entity.IntField, savedEntity.IntField, "Returned object has wrong field value" );
    }

    [TestMethod]
    public void TestSaveDoubleEntity()
    {
      var entity = new DoubleEntity {DoubleField = 16.1616d};
      DoubleEntity savedEntity = Backendless.Persistence.Save( entity );
      Assert.IsNotNull( savedEntity, "Server returned a null result" );
      Assert.IsNotNull( savedEntity.ObjectId, "Returned object doesn't have expected field id" );
      Assert.IsNotNull( savedEntity.Created, "Returned object doesn't have expected field created" );
      Assert.AreEqual( entity.DoubleField, savedEntity.DoubleField, 0.000000d, "Returned object has wrong field value" );
    }
  }
}