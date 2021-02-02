using Xunit;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using System.Collections.Generic;
using BackendlessAPI.Property;

namespace TestProject.Tests.Persistence
{
  [Collection("Tests")]
  public class TestDescribe
  {
    [Fact]
    public void TestDescribe_BlockCall()
    {
      var checker = Backendless.Data.Describe( "Person" );

      Assert.NotNull( checker );
      Assert.IsType<List<ObjectProperty>>( checker );
      Assert.True( checker.Count >= 8 );
    }

    [Fact]
    public void TestDescriveCallback()
    {
      Backendless.Data.Describe( "Person", new AsyncCallback<List<ObjectProperty>>(
      result =>
      {
        Assert.NotNull( result );
        Assert.True( result.Count >= 8 );
      },
      fault =>
      {
        Assert.True( false, "An error occurred while executing the method." );
      } ) );
    }

    [Fact]
    public void TestDescribeNonexistentTable_BlockCall()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Describe( "Non-existent-table" ) );
    }

    [Fact]
    public void TestDesribeNonexistentTable_Callback()
    {
      Backendless.Data.Describe( "Non-existent-table", new AsyncCallback<List<ObjectProperty>>(
      nullable =>
      {
        Assert.True( false, "The expected error didn't occur" );
      },
      fault =>
      {
        Assert.NotNull( fault );
        Assert.NotNull( fault.Message );
        Assert.NotEmpty( fault.Message );
      } ) );
    }
  }
}
