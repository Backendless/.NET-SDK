using System;
using BackendlessAPI.Test.PersistenceService.AsyncEntities;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.PrimitiveEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.AsyncTests
{
  [TestClass]
  public class SaveNewObjectTest : TestsFrame
  {
    [TestMethod]
    public void TestSaveEntityAsyncToANewDataBase()
    {
      RunAndAwait( () =>
        {
          var uniqueWpPerson = new UniqueWPPersonAsync {Age = 16, Name = "John", Birthday = DateTime.Now};
          Backendless.Persistence.Save( uniqueWpPerson,
                                        new ResponseCallback<UniqueWPPersonAsync>( this )
                                          {
                                            ResponseHandler = savedWPPerson =>
                                              {
                                                Assert.IsNotNull( savedWPPerson, "Server returned a null result" );
                                                Assert.IsNotNull( savedWPPerson.Name,
                                                                  "Returned object doesn't have expected field" );
                                                Assert.IsNotNull( savedWPPerson.ObjectId,
                                                                  "Returned object doesn't have expected field id" );
                                                Assert.IsNotNull( savedWPPerson.Created,
                                                                  "Returned object doesn't have expected field created" );
                                                Assert.AreEqual( uniqueWpPerson.Name, savedWPPerson.Name,
                                                                 "Returned object has wrong field value" );
                                                Assert.AreEqual( uniqueWpPerson.Age, savedWPPerson.Age,
                                                                 "Returned object has wrong field value" );
                                                Assert.IsTrue(
                                                  (savedWPPerson.Birthday.Ticks - uniqueWpPerson.Birthday.Ticks) < 1000,
                                                  "Returned object has wrong field value" );
                                                CountDown();
                                              }
                                          } );
        } );
    }

    [TestMethod]
    public void TestSaveStringEntityAsync()
    {
      RunAndAwait( () =>
        {
          StringEntityAsync EntityAsync = new StringEntityAsync {StringField = "foobar"};
          Backendless.Persistence.Save( EntityAsync,
                                        new ResponseCallback<StringEntityAsync>( this )
                                          {
                                            ResponseHandler = savedEntityAsync =>
                                              {
                                                Assert.IsNotNull( savedEntityAsync, "Server returned a null result" );
                                                Assert.IsNotNull( savedEntityAsync.StringField,
                                                                  "Returned object doesn't have expected field" );
                                                Assert.IsNotNull( savedEntityAsync.ObjectId,
                                                                  "Returned object doesn't have expected field id" );
                                                Assert.IsNotNull( savedEntityAsync.Created,
                                                                  "Returned object doesn't have expected field created" );
                                                Assert.AreEqual( EntityAsync.StringField, savedEntityAsync.StringField,
                                                                 "Returned object has wrong field value" );
                                                CountDown();
                                              }
                                          } );
        } );
    }

    [TestMethod]
    public void TestSaveBooleanEntityAsync()
    {
      RunAndAwait( () =>
        {
          var EntityAsync = new BooleanEntityAsync {BooleanField = false};
          Backendless.Persistence.Save( EntityAsync,
                                        new ResponseCallback<BooleanEntityAsync>( this )
                                          {
                                            ResponseHandler = savedEntityAsync =>
                                              {
                                                Assert.IsNotNull( savedEntityAsync, "Server returned a null result" );
                                                Assert.IsNotNull( savedEntityAsync.ObjectId,
                                                                  "Returned object doesn't have expected field id" );
                                                Assert.IsNotNull( savedEntityAsync.Created,
                                                                  "Returned object doesn't have expected field created" );
                                                Assert.AreEqual( EntityAsync.BooleanField, savedEntityAsync.BooleanField,
                                                                 "Returned object has wrong field value" );
                                                CountDown();
                                              }
                                          } );
        } );
    }

    [TestMethod]
    public void TestSaveDateEntityAsync()
    {
      RunAndAwait( () =>
        {
          var EntityAsync = new DateEntityAsync {DateField = DateTime.Now};
          Backendless.Persistence.Save( EntityAsync,
                                        new ResponseCallback<DateEntityAsync>( this )
                                          {
                                            ResponseHandler = savedEntityAsync =>
                                              {
                                                Assert.IsNotNull( savedEntityAsync, "Server returned a null result" );
                                                Assert.IsNotNull( savedEntityAsync.DateField,
                                                                  "Returned object doesn't have expected field" );
                                                Assert.IsNotNull( savedEntityAsync.ObjectId,
                                                                  "Returned object doesn't have expected field id" );
                                                Assert.IsNotNull( savedEntityAsync.Created,
                                                                  "Returned object doesn't have expected field created" );
                                                Assert.IsTrue(
                                                  savedEntityAsync.DateField.Ticks - EntityAsync.DateField.Ticks < 1000,
                                                  "Returned object has wrong field value" );
                                                CountDown();
                                              }
                                          } );
        } );
    }

    [TestMethod]
    public void TestSaveIntEntityAsync()
    {
      RunAndAwait( () =>
        {
          var EntityAsync = new IntEntityAsync {IntField = 16};
          Backendless.Persistence.Save( EntityAsync,
                                        new ResponseCallback<IntEntityAsync>( this )
                                          {
                                            ResponseHandler = savedEntityAsync =>
                                              {
                                                Assert.IsNotNull( savedEntityAsync, "Server returned a null result" );
                                                Assert.IsNotNull( savedEntityAsync.ObjectId,
                                                                  "Returned object doesn't have expected field id" );
                                                Assert.IsNotNull( savedEntityAsync.Created,
                                                                  "Returned object doesn't have expected field created" );
                                                Assert.AreEqual( EntityAsync.IntField, savedEntityAsync.IntField,
                                                                 "Returned object has wrong field value" );
                                                CountDown();
                                              }
                                          } );
        } );
    }

    [TestMethod]
    public void TestSaveDoubleEntityAsync()
    {
      RunAndAwait( () =>
        {
          var EntityAsync = new DoubleEntityAsync {DoubleField = 16.1616d};
          Backendless.Persistence.Save( EntityAsync,
                                        new ResponseCallback<DoubleEntityAsync>( this )
                                          {
                                            ResponseHandler = savedEntityAsync =>
                                              {
                                                Assert.IsNotNull( savedEntityAsync, "Server returned a null result" );
                                                Assert.IsNotNull( savedEntityAsync.ObjectId,
                                                                  "Returned object doesn't have expected field id" );
                                                Assert.IsNotNull( savedEntityAsync.Created,
                                                                  "Returned object doesn't have expected field created" );
                                                Assert.AreEqual( EntityAsync.DoubleField, savedEntityAsync.DoubleField,
                                                                 0.000000d, "Returned object has wrong field value" );
                                                CountDown();
                                              }
                                          } );
        } );
    }
  }
}