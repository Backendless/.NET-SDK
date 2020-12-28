using BackendlessAPI;
using BackendlessAPI.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
  [TestClass]
  public class TestFindFirstDictionary
  {
    [TestMethod]
    public void FFWithoutParametersDictionary()
    {
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of( "Person" ).Save( person );
      Dictionary<String, Object> receivedPerson = Backendless.Data.Of( "Person" ).FindFirst();

      if( !String.IsNullOrEmpty( receivedPerson[ "objectId" ].ToString() ) )
      {
        Assert.IsTrue( (Double) receivedPerson[ "age" ] == Convert.ToDouble(person[ "age" ]) );
        Assert.IsTrue( receivedPerson[ "name" ].ToString() == person[ "name" ].ToString() );
      }

      Backendless.Data.Of( "Person" ).Remove( "age='16'" );
    }

#if !(NET_35 || NET_40)
    [TestMethod]
    public void FFAsyncMethodDictionary()
    {
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of( "Person" ).Save( person );
      Task.Run( async () =>
       {
         Dictionary<String, Object> receivedPerson = await Backendless.Data.Of( "Person" ).FindFirstAsync();

         if( !String.IsNullOrEmpty( receivedPerson[ "objectId" ].ToString() ) )
         {
           Assert.IsTrue( receivedPerson[ "age" ] == person[ "age" ] );
           Assert.IsTrue( receivedPerson[ "name" ] == person[ "name" ] );
         }

         Backendless.Data.Of( "Person" ).Remove( "age='16'" );
       } );
    }
#endif

    [TestMethod]
    public void FFAsyncCallbackDictionary()
    {
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of( "Person" ).Save( person );
      Backendless.Data.Of( "Person" ).FindFirst( new AsyncCallback<Dictionary<String, Object>>(
      callback=>
      {
        if( !String.IsNullOrEmpty( callback[ "objectId" ].ToString() ) )
        {
          Assert.IsTrue( (Double) callback[ "age" ] == Convert.ToDouble( person[ "age" ] ) );
          Assert.IsTrue( callback[ "name" ].ToString() == person[ "name" ].ToString() );
        }
      },
      fault=>
      {
        Assert.IsTrue( false );
      } ));

      Backendless.Data.Of( "Person" ).Remove( "age='16'" );
    }
  }
}
