using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Property;
using BackendlessAPI.Test.PersistenceService.AsyncEntities;
using BackendlessAPI.Test.PersistenceService.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.AsyncTests
{
  [TestClass]
  public class RetrievePropertiesTest : TestsFrame
  {
    [TestMethod]
    public void TestRetrieveEntityProperties()
    {
      RunAndAwait( () =>
        {
          WPPersonAsync wpPerson = GetRandomWPPerson();
          Backendless.Persistence.Save( wpPerson,
                                        new ResponseCallback<WPPersonAsync>( this )
                                          {
                                            ResponseHandler =
                                              response =>
                                              Backendless.Persistence.Describe( typeof( WPPersonAsync ).Name,
                                                                                new ResponseCallback<List<ObjectProperty>>
                                                                                  ( this )
                                                                                  {
                                                                                    ResponseHandler = properties =>
                                                                                      {
                                                                                        Assert.IsNotNull( properties,
                                                                                                          "Server returned null" );
                                                                                        Assert.AreEqual(
                                                                                          properties.Count, 5,
                                                                                          "Server returned unexpected amount of properties" );

                                                                                        foreach(
                                                                                          ObjectProperty property in
                                                                                            properties )
                                                                                        {
                                                                                          if( property.Name.Equals( "Age" ) )
                                                                                          {
                                                                                            Assert.AreEqual(
                                                                                              DateTypeEnum.INT,
                                                                                              property.Type,
                                                                                              "Property was of unexpected type" );
                                                                                            Assert.IsFalse(
                                                                                              property.IsRequired,
                                                                                              "Property had a wrong required value" );
                                                                                          }
                                                                                          else if(
                                                                                            property.Name.Equals( "Name" ) )
                                                                                          {
                                                                                            Assert.AreEqual(
                                                                                              DateTypeEnum.STRING,
                                                                                              property.Type,
                                                                                              "Property was of unexpected type" );
                                                                                            Assert.IsFalse(
                                                                                              property.IsRequired,
                                                                                              "Property had a wrong required value" );
                                                                                          }
                                                                                          else if(
                                                                                            property.Name.Equals(
                                                                                              "created" ) )
                                                                                          {
                                                                                            Assert.AreEqual(
                                                                                              DateTypeEnum.DATETIME,
                                                                                              property.Type,
                                                                                              "Property was of unexpected type" );
                                                                                            Assert.IsFalse(
                                                                                              property.IsRequired,
                                                                                              "Property had a wrong required value" );
                                                                                          }
                                                                                          else if(
                                                                                            property.Name.Equals(
                                                                                              "objectId" ) )
                                                                                          {
                                                                                            Assert.AreEqual(
                                                                                              DateTypeEnum.STRING,
                                                                                              property.Type,
                                                                                              "Property was of unexpected type" );
                                                                                            Assert.IsFalse(
                                                                                              property.IsRequired,
                                                                                              "Property had a wrong required value" );
                                                                                          }
                                                                                          else if(
                                                                                            property.Name.Equals(
                                                                                              "updated" ) )
                                                                                          {
                                                                                            Assert.AreEqual(
                                                                                              DateTypeEnum.DATETIME,
                                                                                              property.Type,
                                                                                              "Property was of unexpected type" );
                                                                                            Assert.IsFalse(
                                                                                              property.IsRequired,
                                                                                              "Property had a wrong required value" );
                                                                                          }
                                                                                          else
                                                                                          {
                                                                                            Assert.Fail(
                                                                                              "Got unexpected property: " +
                                                                                              property.Name );
                                                                                          }
                                                                                        }

                                                                                        CountDown();
                                                                                      }
                                                                                  } )
                                          } );
        } );
    }

    [TestMethod]
    public void TestRetrievePropertiesForUnknownObject()
    {
      RunAndAwait(
        () =>
        Backendless.Persistence.Describe( this.GetType().Name,
                                          new AsyncCallback<List<ObjectProperty>>(
                                            response => Assert.Fail( "Server didn't throw an exception" ),
                                            fault => CheckErrorCode( 1009, fault ) ) ) );

    }
  }
}