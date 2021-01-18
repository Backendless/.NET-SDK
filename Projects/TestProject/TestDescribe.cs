using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using BackendlessAPI.Property;

namespace TestProject
{
  [Collection("Tests")]
  public class TestDescribe
  {
    [Fact]
    public void TestDescribeBlockCall()
    {
      var checker = Backendless.Data.Describe( "Person" );

      Assert.IsType<List<ObjectProperty>>( checker );
      Assert.True( checker.Count.Equals( 7 ) );
    }

    [Fact]
    public void TestDescriveCallback()
    {
      Backendless.Data.Describe( "Person", new AsyncCallback<List<ObjectProperty>>(
      result =>
      {
        Assert.True( result.Count.Equals( 7 ) );
      },
      fault =>
      {
        Assert.True( false, "An error occurred while executing the method." );
      } ) );
    }
  }
}
