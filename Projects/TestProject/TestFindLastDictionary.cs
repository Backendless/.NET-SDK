using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;

namespace TestProject
{
  [TestClass]
  public class TestFindLastDictionary
  {
    [TestMethod]
    public void FLWithoutParametersDictionary()
    {
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of( "Person" ).Save( person );
      Dictionary<String, Object> receivedPerson = Backendless.Data.Of( "Person" ).FindLast();

      if( !String.IsNullOrEmpty( receivedPerson[ "objectId" ].ToString() ) )
      {
        Assert.IsTrue( (Double) receivedPerson[ "age" ] == Convert.ToDouble( person[ "age" ] ) );
        Assert.IsTrue( receivedPerson[ "name" ].ToString() == person[ "name" ].ToString() );
      }

      Backendless.Data.Of( "Person" ).Remove( "age='16'" );
    }

    [TestMethod]
    public void FLAsyncMethodDictionary()
    {
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of( "Person" ).Save( person );
      Task.Run( async () =>
      {
        Dictionary<String, Object> receivedPerson = await Backendless.Data.Of( "Person" ).FindLastAsync();

        if( !String.IsNullOrEmpty( receivedPerson[ "objectId" ].ToString() ) )
        {
          Assert.IsTrue( receivedPerson[ "age" ] == person[ "age" ] );
          Assert.IsTrue( receivedPerson[ "name" ] == person[ "name" ] );
        }

        Backendless.Data.Of( "Person" ).Remove( "age='16'" );
      } );
    }

    [TestMethod]
    public void FLAsyncCallbackDictionary()
    {
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of( "Person" ).Save( person );
      Backendless.Data.Of( "Person" ).FindLast( new AsyncCallback<Dictionary<String, Object>>(
      callback =>
      {
        if( !String.IsNullOrEmpty( callback[ "objectId" ].ToString() ) )
        {
          Assert.IsTrue( callback[ "age" ] == person[ "age" ] );
          Assert.IsTrue( callback[ "name" ] == person[ "name" ] );
        }
      },
      fault =>
      {
        Assert.IsTrue( false );
      } ) );

      Backendless.Data.Of( "Person" ).Remove( "age='16'" );
    }
  }
}
