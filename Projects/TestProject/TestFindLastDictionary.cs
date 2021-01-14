using Xunit;
using System;
using System.Collections.Generic;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;

namespace TestProject
{
  [Collection( "Tests" )]
  public class TestFindLastDictionary : IDisposable
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();
    public TestFindLastDictionary()
    {
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";
    }

    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age='16'" );
    }

    [Fact]
    public void FLWithoutParametersDictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );
      Dictionary<String, Object> receivedPerson = Backendless.Data.Of( "Person" ).FindLast();

      if( !String.IsNullOrEmpty( receivedPerson[ "objectId" ].ToString() ) )
      {
        Assert.True( (Double) receivedPerson[ "age" ] == Convert.ToDouble( person[ "age" ] ), "Actual field 'age' is not equal expected" );
        Assert.True( receivedPerson[ "name" ].ToString() == person[ "name" ].ToString(), "Actual field 'name' is not equal expected" );
      }
      else
        Assert.True( false, "Person's objectId is null" );
    }

    [Fact]
    public void FLAsyncMethodDictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );
      Task.Run( async () =>
      {
        Dictionary<String, Object> receivedPerson = await Backendless.Data.Of( "Person" ).FindLastAsync();

        if( !String.IsNullOrEmpty( receivedPerson[ "objectId" ].ToString() ) )
        {
          Assert.True( (Double) receivedPerson[ "age" ] == Convert.ToDouble( person[ "age" ] ), "Actual field 'age' is not equal expected" );
          Assert.True( receivedPerson[ "name" ].ToString() == person[ "name" ].ToString(), "Actual field 'name' is not equal expected" );
        }
        else
          Assert.True( false, "Person's objectId is null" );
      } );
    }

    [Fact]
    public void FLAsyncCallbackDictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );
      Backendless.Data.Of( "Person" ).FindLast( new AsyncCallback<Dictionary<String, Object>>(
      callback =>
      {
        if( !String.IsNullOrEmpty( callback[ "objectId" ].ToString() ) )
        {
          Assert.True( (Double) callback[ "age" ] == Convert.ToDouble( person[ "age" ] ), "Actual field 'age' is not equal expected" );
          Assert.True( callback[ "name" ].ToString() == person[ "name" ].ToString(), "Actual field 'name' is not equal expected" );
        }
        else
          Assert.True( false, "Person's objectId is null" );
      },
      fault =>
      {
        Assert.True( false, "Person is null" );
      } ) );
    }
  }
}