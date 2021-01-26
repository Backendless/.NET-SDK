using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using System.Collections.Generic;

namespace TestProject.Tests.Persistence
{
  [Collection("Tests")]
  public class TestRemoveDictionary
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();
    public TestRemoveDictionary()
    {
    }

    [Fact]
    public void TestRemoveEntity_BlockCall()
    {
      person[ "name" ] = "Alexandra";
      person[ "age" ] = 18;
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      Backendless.Data.Of("Person").Remove( person );
      IList<Dictionary<String, Object>> actual = Backendless.Data.Of("Person").Find();

      Assert.Empty( actual );
    }

    [Fact]
    public void TestRemoveEntity_Callback()
    {
      person[ "name" ] = "Alexandra";
      person[ "age" ] = 18;
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      Backendless.Data.Of("Person").Remove( person, new AsyncCallback<Int64>(
      count =>
      {
        IList<Dictionary<String, Object>> actual = Backendless.Data.Of("Person").Find();

        Assert.Empty( actual );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the 'Remove' operation" );
      } ) );
    }

    [Fact]
    public void TestRemoveClause_Blockcall()
    {
      person[ "name" ] = "Alexandra";
      person[ "age" ] = 18;
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      Backendless.Data.Of("Person").Remove( "age = '18'" );
      IList<Dictionary<String, Object>> actual = Backendless.Data.Of("Person").Find();

      Assert.Empty( actual );
    }

    [Fact]
    public void TestRemoveClause_Callback()
    {
      person[ "name" ] = "Alexandra";
      person[ "age" ] = 18;
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      Backendless.Data.Of("Person").Remove( "age = '18'", new AsyncCallback<Int32>(
      count =>
      {
        IList<Dictionary<String, Object>> actual = Backendless.Data.Of("Person").Find();

        Assert.Empty( actual );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the 'Remove' operation" );
      } ) );
    }

    [Fact]
    public async void TestRemoveEntity_Async()
    {
      person[ "name" ] = "Alexandra";
      person[ "age" ] = 18;
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      await Backendless.Data.Of("Person").RemoveAsync( person );
      IList<Dictionary<String, Object>> actual = Backendless.Data.Of("Person").Find();

      Assert.Empty( actual );
    }

    [Fact]
    public async void TestRemoveClause_Async()
    {
      person[ "name" ] = "Alexandra";
      person[ "age" ] = 18;
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      await Backendless.Data.Of("Person").RemoveAsync( "age = '18'" );
      IList<Dictionary<String, Object>> actual = Backendless.Data.Of("Person").Find();

      Assert.Empty( actual );
    }

    [Fact]
    public void TestRemoveWrongTableName_BlockCall_Clause()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "WrongTableName" ).Remove( "age>'0'" ) );
    }

    [Fact]
    public void TestRemoveWrongTableName_Callback_Clause()
    {
      Backendless.Data.Of( "WrongTableName" ).Remove( "age>'0'", new AsyncCallback<Int32>(
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

    [Fact]
    public void TestRemoveWrongTableName_Async_Clause()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "WrongTableName" ).RemoveAsync( "age>'0'" ) );
    }

    [Fact]
    public void TestRemoveWrongTableName_BlockCall_Dictionary()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "WrongTableName" ).Remove( person ) );
    }

    [Fact]
    public void TestRemoveWrongTableName_Callback_Dictionary()
    {
      Backendless.Data.Of( "WrongTableName" ).Remove( person, new AsyncCallback<Int64>(
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

    [Fact]
    public void TestRemoveWrongTableName_Async_Dictionary()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "WrongTableName" ).RemoveAsync( person ) );
    }

    [Fact]
    public void TestRemoveWrongClause_BlockCall_Clause()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Person" ).Remove( "_+%@$" ) );
    }

    [Fact]
    public void TestRemoveWrongClause_Callback_Clause()
    {
      Backendless.Data.Of( "Person" ).Remove( "_+%@$", new AsyncCallback<Int32>(
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

    [Fact]
    public void TestRemoveWrongClause_Async_Clause()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Person" ).RemoveAsync( "_+%@$" ) );
    }

    [Fact]
    public void TestRemoveNullObjectId_BlockCall_Dictionary()
    {
      person.Remove( "objectId" );

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Person" ).Remove( person ) );
    }

    [Fact]
    public void TestRemoveNullObjectId_Callback_Dictionary()
    {
      person.Remove( "objectId" );

      Backendless.Data.Of( "Person" ).Remove( person, new AsyncCallback<Int64>(
      nullable =>
      {
        Assert.True( false, "The expected erro didn't occur" );
      },
      fault =>
      {
        Assert.NotNull( fault );
        Assert.NotNull( fault.Message );
        Assert.NotEmpty( fault.Message );
      } ) );
    }

    [Fact]
    public void TestRemoveNullObjectId_Async_Dictionary()
    {
      person.Remove( "objectId" );

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Person" ).RemoveAsync( person ) );
    }

    [Fact]
    public void TestRemoveWrongObjectId_BlockCall_Dictionary()
    {
      person[ "objectId" ] = "Wrong-object-Id";

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Person" ).Remove( person ) );
    }

    [Fact]
    public void TestRemoveWrongObjectId_Callback_Dictionary()
    {
      person[ "objectId" ] = "Wrong-object-Id";

      Backendless.Data.Of( "Person" ).Remove( person, new AsyncCallback<Int64>(
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

    [Fact]
    public void TestRemoveWrongObjectId_Async_Dictionary()
    {
      person[ "objectId" ] = "Wrong-object-Id";

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Person" ).RemoveAsync( person ) );
    }
  }
}
