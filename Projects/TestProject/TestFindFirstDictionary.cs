using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestProject
{
  [Collection( "Tests" )]
  public class TestFindFirstDictionary : IDisposable
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();

    public TestFindFirstDictionary()
    {
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";
    }

    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age='16'" );
    }

    [Fact]
    public void FFWithoutParametersDictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );
      Dictionary<String, Object> receivedPerson = Backendless.Data.Of( "Person" ).FindFirst();

      if( !String.IsNullOrEmpty( receivedPerson[ "objectId" ].ToString() ) )
      {
        Assert.True( (Double) receivedPerson[ "age" ] == Convert.ToDouble( person[ "age" ] ) );
        Assert.True( receivedPerson[ "name" ].ToString() == person[ "name" ].ToString() );
      }
      else
        Assert.True( false );
    }

    [Fact]
    public void FFAsyncMethodDictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );
      Task.Run( async () =>
      {
        Dictionary<String, Object> receivedPerson = await Backendless.Data.Of( "Person" ).FindFirstAsync();

        if( !String.IsNullOrEmpty( receivedPerson[ "objectId" ].ToString() ) )
        {
          Assert.True( receivedPerson[ "age" ] == person[ "age" ] );
          Assert.True( receivedPerson[ "name" ] == person[ "name" ] );
        }
        else
          Assert.True( false );
      } );
    }

    [Fact]
    public void FFAsyncCallbackDictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );
      Backendless.Data.Of( "Person" ).FindFirst( new AsyncCallback<Dictionary<String, Object>>(
      callback =>
      {
        if( !String.IsNullOrEmpty( callback[ "objectId" ].ToString() ) )
        {
          Assert.True( (Double) callback[ "age" ] == Convert.ToDouble( person[ "age" ] ) );
          Assert.True( callback[ "name" ].ToString() == person[ "name" ].ToString() );
        }
        else
          Assert.True( false );
      },
      fault =>
      {
        Assert.True( false );
      } ) );
    }
  }
}