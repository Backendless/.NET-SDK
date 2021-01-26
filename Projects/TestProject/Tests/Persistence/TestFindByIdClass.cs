using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using BackendlessAPI.Persistence;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestFindByIdClass : IDisposable
  {
    Person person = new Person();

    public TestFindByIdClass()
    {
      person.age = 18;
      person.name = "Joe";
    }

    public void Dispose()
    {
      Backendless.Data.Of("Person").Remove( "age>'0'" );
    }

    [Fact]
    public void TestFindById_BlockCall_Class()
    {
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;

      var actual = Backendless.Data.Of<Person>().FindById( person );

      Assert.NotNull( actual );
      Assert.IsType<Person>( actual );
      Assert.Equal( person.name, actual.name );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
    }

    [Fact]
    public void TestFindById_Callback_Class()
    {
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;
      Backendless.Data.Of<Person>().FindById( person, new AsyncCallback<Person>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.IsType<Person>( actual );
        Assert.Equal( person.name, actual.name );
        Assert.True( Comparer.IsEqual( person.age, actual.age ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void TestFindById_Async_Class()
    {
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;

      var actual = await Backendless.Data.Of<Person>().FindByIdAsync( person );

      Assert.NotNull( actual );
      Assert.IsType<Person>( actual );
      Assert.Equal( person.name, actual.name );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
    }

    [Fact]
    public void TestFindById_BlockCall_StringId()
    {
      String id = Backendless.Data.Of<Person>().Save( person ).objectId;

      var actual = Backendless.Data.Of<Person>().FindById( id );

      Assert.NotNull( actual );
      Assert.IsType<Person>( actual );
      Assert.Equal( person.name, actual.name );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
    }

    [Fact]
    public void TestFindById_Callback_StringId()
    {
      String id = Backendless.Data.Of<Person>().Save( person ).objectId;

      Backendless.Data.Of<Person>().FindById( id, new AsyncCallback<Person>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.IsType<Person>( actual );
        Assert.Equal( person.name, actual.name );
        Assert.True( Comparer.IsEqual( person.age, actual.age ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void TestFindById_Async_StringId()
    {
      String id = Backendless.Data.Of<Person>().Save( person ).objectId;

      var actual = await Backendless.Data.Of<Person>().FindByIdAsync( id );

      Assert.NotNull( actual );
      Assert.IsType<Person>( actual );
      Assert.Equal( person.name, actual.name );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
    }

    [Fact]
    public void TestFindById_BlockCall_Class_DQB()
    {
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      var actual = Backendless.Data.Of<Person>().FindById( person, queryBuilder );

      Assert.NotNull( actual );
      Assert.IsType<Person>( actual );
      Assert.Null( actual.name );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
    }

    [Fact]
    public void TestFindById_Callback_Class_DQB()
    {
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      Backendless.Data.Of<Person>().FindById( person, queryBuilder, new AsyncCallback<Person>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.IsType<Person>( actual );
        Assert.Null( actual.name );
        Assert.True( Comparer.IsEqual( person.age, actual.age ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void TestFindById_Async_Class_DQB()
    {
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      var actual = await Backendless.Data.Of<Person>().FindByIdAsync( person, queryBuilder );

      Assert.NotNull( actual );
      Assert.IsType<Person>( actual );
      Assert.Null( actual.name );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
    }

    [Fact]
    public void TestFindById_BlockCall_StringId_DQB()
    {
      String id = Backendless.Data.Of<Person>().Save( person ).objectId;
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      var actual = Backendless.Data.Of<Person>().FindById( id, queryBuilder );

      Assert.NotNull( actual );
      Assert.IsType<Person>( actual );
      Assert.Null( actual.name );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
    }

    [Fact]
    public void TestFindById_Callback_StringId_DQB()
    {
      String id = Backendless.Data.Of<Person>().Save( person ).objectId;
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      Backendless.Data.Of<Person>().FindById( id, queryBuilder, new AsyncCallback<Person>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.IsType<Person>( actual );
        Assert.Null( actual.name );
        Assert.True( Comparer.IsEqual( person.age, actual.age ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void TestFindById_Async_StringId_DQB()
    {
      String id = Backendless.Data.Of<Person>().Save( person ).objectId;
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      var actual = await Backendless.Data.Of<Person>().FindByIdAsync( id, queryBuilder );

      Assert.NotNull( actual );
      Assert.IsType<Person>( actual );
      Assert.Null( actual.name );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
    }
  }
}
