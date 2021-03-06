﻿using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using System.Collections.Generic;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestSaveDictionary : IDisposable
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();

    public TestSaveDictionary()
    {
      person[ "name" ] = "Elizabeth";
      person[ "age" ] = 20;
    }

    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestSave_BlockCall()
    {
      Dictionary<String, Object> actual = Backendless.Data.Of( "Person" ).Save( person );


      Assert.NotNull( actual );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
    }

    [Fact]
    public void TestSave_Callback()
    {
      Backendless.Data.Of( "Person" ).Save( person, new AsyncCallback<Dictionary<String, Object>>(
        actual =>
        {
          Assert.NotNull( actual );
          Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
          Assert.Equal( person[ "name" ], actual[ "name" ] );
        },
        fault =>
        {
          Assert.True( false, "Something went wrong during the execution of the 'Save' method" );
        } ) );
    }

    [Fact]
    public async void TestSave_Async()
    {
      Dictionary<String, Object> actual = await Backendless.Data.Of( "Person" ).SaveAsync( person );

      Assert.NotNull( person );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
    }

    [Fact]
    public void TestUpdate_BlockCall()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "age" ] = 21;

      Dictionary<String, Object> actual = Backendless.Data.Of( "Person" ).Save( expected );
      Assert.NotNull( actual );
      Assert.Equal( expected[ "objectId" ], actual[ "objectId" ] );
      Assert.True( Comparer.IsEqual( expected[ "age" ], actual[ "age" ] ) );
    }

    [Fact]
    public void TestUpdate_Callback()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "age" ] = 21;

      Backendless.Data.Of( "Person" ).Save( expected, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( expected[ "objectId" ], actual[ "objectId" ] );
        Assert.True( Comparer.IsEqual( expected[ "age" ], actual[ "age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the execution operation" );
      } ) );
    }

    [Fact]
    public async void TestUpdate_Async()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "age" ] = 21;

      Dictionary<String, Object> actual = await Backendless.Data.Of( "Person" ).SaveAsync( expected );
      Assert.NotNull( actual );
      Assert.Equal( expected[ "objectId" ], actual[ "objectId" ] );
      Assert.True( Comparer.IsEqual( expected[ "age" ], actual[ "age" ] ) );
    }

    [Fact]
    public void TestUpdateAddField_BlockCall()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "New" ] = "Non-existent field";

      var actual = Backendless.Data.Of( "Person" ).Save( expected );

      Assert.NotNull( actual[ "New" ] );
      Assert.IsType<String>( actual[ "New" ] );
      Assert.Equal( expected[ "New" ], actual[ "New" ] );
    }

    [Fact]
    public void TestUpdateAddField_Callback()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "New" ] = "Non-existent field";

      Backendless.Data.Of( "Person" ).Save( expected, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual[ "New" ] );
        Assert.IsType<String>( actual[ "New" ] );
        Assert.Equal( expected[ "New" ], actual[ "New" ] );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the execution operation" );
      } ) );
    }

    [Fact]
    public async void TestUpdateAddField_Async()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected["New"] = "Non-existent field";

      var actual = await Backendless.Data.Of( "Person" ).SaveAsync( expected );

      Assert.NotNull( actual[ "New" ] );
      Assert.IsType<String>( actual[ "New" ] );
      Assert.Equal( expected[ "New" ], actual[ "New" ] );
    }

    [Fact]
    public void TestUpdateWithWrongObjectId_BlockCall()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "objectId" ] = "The-wrong-objectId";

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Person" ).Save( expected ) );
    }

    [Fact]
    public void TestUpdateWrongFieldName_BlockCall()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "New-_" ] = "Non-existent field";

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Person" ).Save( expected ) );
    }

    [Fact]
    public void TestUpdateWithWrongObjectId_Callback()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "objectId" ] = "The-wrong-objectId";

      Backendless.Data.Of( "Person" ).Save( expected, new AsyncCallback<Dictionary<String, Object>>(
      error =>
      {
        Assert.True( false, "An error was expected, but it was not" );
      },
      fault =>
      {
        Assert.True( true );
      } ) );
    }

    [Fact]
    public void TestUpdateWrongFieldName_Callback()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "New-_" ] = "Non-existent field";

      Backendless.Data.Of( "Person" ).Save( expected, new AsyncCallback<Dictionary<String, Object>>(
      error =>
      {
        Assert.True( false, "An error was expected, but is was not" );
      },
      fault =>
      {
        Assert.True( true );
      } ) );
    }

    [Fact]
    public void TestUpdateWithWrongObjectIdAsync()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "objectId" ] = "The-wrong-objectId";

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Person" ).SaveAsync( expected ) );
    }

    [Fact]
    public void TestUpdateWrongFieldNameAsync()
    {
      Dictionary<String, Object> expected = Backendless.Data.Of( "Person" ).Save( person );
      expected[ "New-_" ] = "Non-existent field";

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Person" ).SaveAsync( expected ) );
    }
  }
}
