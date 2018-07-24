using System;
using System.Collections.Generic;
using BackendlessAPI.Property;
using BackendlessAPI.Test.PersistenceService.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.SyncTests
{
  [TestClass]
  public class RetrievePropertiesTest : TestsFrame
  {
    [TestMethod]
    public void TestRetrieveEntityProperties()
    {
      WPPerson wpPerson = GetRandomWPPerson();
      Backendless.Persistence.Save( wpPerson );

      List<ObjectProperty> properties = Backendless.Persistence.Describe( typeof( WPPerson ).Name );

      Assert.IsNotNull( properties, "Server returned null" );
      Assert.AreEqual( properties.Count, 5, "Server returned unexpected amount of properties" );

      foreach( ObjectProperty property in properties )
      {
        if( property.Name.Equals( "Age" ) )
        {
          Assert.AreEqual( DateTypeEnum.INT, property.Type, "Property was of unexpected type" );
          Assert.IsFalse( property.IsRequired, "Property had a wrong required value" );
        }
        else if( property.Name.Equals( "Name" ) )
        {
          Assert.AreEqual( DateTypeEnum.STRING, property.Type, "Property was of unexpected type" );
          Assert.IsFalse( property.IsRequired, "Property had a wrong required value" );
        }
        else if( property.Name.Equals( "created" ) )
        {
          Assert.AreEqual( DateTypeEnum.DATETIME, property.Type, "Property was of unexpected type" );
          Assert.IsFalse( property.IsRequired, "Property had a wrong required value" );
        }
        else if( property.Name.Equals( "objectId" ) )
        {
          Assert.AreEqual( DateTypeEnum.STRING, property.Type, "Property was of unexpected type" );
          Assert.IsFalse( property.IsRequired, "Property had a wrong required value" );
        }
        else if( property.Name.Equals( "updated" ) )
        {
          Assert.AreEqual( DateTypeEnum.DATETIME, property.Type, "Property was of unexpected type" );
          Assert.IsFalse( property.IsRequired, "Property had a wrong required value" );
        }
        else
        {
          Assert.Fail( "Got unexpected property: " + property.Name );
        }
      }
    }

    [TestMethod]
    public void TestRetrievePropertiesForUnknownObject()
    {
      try
      {
        Backendless.Persistence.Describe( this.GetType().Name );
        Assert.Fail( "Server didn't throw an exception" );
      }
      catch( System.Exception e )
      {
        CheckErrorCode( 1009, e );
      }
    }
  }
}