using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using BackendlessAPI.Exception;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestRemoveClass
  {
    Person person = new Person();

    [Fact]
    public void TestRemove_BlockCall_Class()
    {
      person.name = "Alexandra";
      person.age = 18;
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;

      Backendless.Data.Of<Person>().Remove( person );

      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.Empty( actual );
    }

    [Fact]
    public void TestRemove_Callback_Class()
    {
      person.name = "Alexandra";
      person.age = 18;
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;

      Backendless.Data.Of<Person>().Remove( person, new AsyncCallback<Int64>(
      count =>
      {
        IList<Person> actual = Backendless.Data.Of<Person>().Find();

        Assert.Empty( actual );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the 'Remove' operation" );
      } ) );
    }

    [Fact]
    public void TestRemove_Blockcall_Clause()
    {
      person.name = "Alexandra";
      person.age = 18;
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;

      Backendless.Data.Of<Person>().Remove( "age = '18'" );
      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.Empty( actual );
    }

    [Fact]
    public void TestRemove_Callback_Clause()
    {
      person.name = "Alexandra";
      person.age = 18;
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;

      Backendless.Data.Of<Person>().Remove( "age = '18'", new AsyncCallback<Int32>(
      count =>
      {
        IList<Person> actual = Backendless.Data.Of<Person>().Find();

        Assert.Empty( actual );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the 'Remove' operation" );
      } ) );
    }

    [Fact]
    public async void TestRemove_Async_Class()
    {
      person.name = "Alexandra";
      person.age = 18;
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;

      await Backendless.Data.Of<Person>().RemoveAsync( person );
      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.Empty( actual );
    }

    [Fact]
    public async void TestRemove_Async_Clause()
    {
      person.name = "Alexandra";
      person.age = 18;
      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;

      await Backendless.Data.Of<Person>().RemoveAsync( "age = '18'" );
      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.Empty( actual );
    }

    [Fact]
    public void TestRemoveWrongTableName_BlockCall_Clause()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Area>().Remove( "age>'0'" ) );
    }

    [Fact]
    public void TestRemoveWrongTableName_Callback_Clause()
    {
      Backendless.Data.Of<Area>().Remove( "age>'0'", new AsyncCallback<Int32>(
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
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Area>().RemoveAsync( "age>'0'" ) );
    }

    [Fact]
    public void TestRemoveWrongTableName_BlockCall_Class()
    {
      Area area = new Area();
      area.UserId = 32;

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Area>().Remove( area ) );
    }

    [Fact]
    public void TestRemoveWrongTableName_Callback_Class()
    {
      Area area = new Area();
      area.UserId = 32;

      Backendless.Data.Of<Area>().Remove( area, new AsyncCallback<Int64>(
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
    public void TestRemoveWrongTableName_Async_Class()
    {
      Area area = new Area();
      area.UserId = 32;

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Area>().RemoveAsync( area ) );
    }

    [Fact]
    public void TestRemoveWrongClause_BlockCall_Clause()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Person>().Remove( "_+%@$" ) );
    }

    [Fact]
    public void TestRemoveWrongClause_Callback_Clause()
    {
      Backendless.Data.Of<Person>().Remove( "_+%@$", new AsyncCallback<Int32>(
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
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Person>().RemoveAsync( "_+%@$" ) );
    }

    [Fact]
    public void TestRemoveWrongObjectId_BlockCall_Class()
    {
      person.objectId = "Wrong-object-id";

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Person>().Remove( person ) );
    }

    [Fact]
    public void TestRemoveWrongObjectId_Callback_Class()
    {
      person.objectId = "Wrong-object-id";

      Backendless.Data.Of<Person>().Remove( person, new AsyncCallback<Int64>(
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
    public void TestRemoveWrongObjectId_Async_Class()
    {
      person.objectId = "Wrong-object-id";

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Person>().RemoveAsync( person ) );
    }
  }
}