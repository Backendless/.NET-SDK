using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using System.Collections.Generic;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestBulkCreateClass : IDisposable
  {
    List<Person> people = new List<Person>();
    Person person_1 = new Person();
    Person person_2 = new Person();
    public TestBulkCreateClass()
    {
      people.Add( person_1 );
      people.Add( person_2 );
      person_1.age = 20;
      person_1.name = "Elizabeth";
      person_2.age = 18;
      person_2.name = "Alexandra";
    }

    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age>'0'" );
    }

    [Fact]
    public void BulkCreate_BlockCall()
    {
      Backendless.Data.Of<Person>().Create( people );

      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.NotNull( actual );
      Assert.NotEmpty( actual );
      Assert.True( person_1.age == actual[ 0 ].age || person_2.age == actual[ 0 ].age );
      Assert.True( person_1.name == actual[ 0 ].name || person_2.name == actual[ 0 ].name );
      Assert.True( person_1.age == actual[ 1 ].age || person_2.age == actual[ 1 ].age );
      Assert.True( person_1.name == actual[ 1 ].name || person_2.name == actual[ 1 ].name );
    }

    [Fact]
    public void BulkCreateCallback()
    {
      Backendless.Data.Of<Person>().Create( people, new AsyncCallback<IList<String>>(
      objectIds =>
      {
        IList<Person> actual = Backendless.Data.Of<Person>().Find();

        Assert.NotNull( actual );
        Assert.NotEmpty( actual );
        Assert.True( person_1.age == actual[ 0 ].age || person_2.age == actual[ 0 ].age );
        Assert.True( person_1.name == actual[ 0 ].name || person_2.name == actual[ 0 ].name );
        Assert.True( person_1.age == actual[ 1 ].age || person_2.age == actual[ 1 ].age );
        Assert.True( person_1.name == actual[ 1 ].name || person_2.name == actual[ 1 ].name );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the execution of the 'BulkCreate' operation" );
      } ) );
    }

    [Fact]
    public async void BulkCreateAsync()
    {
      await Backendless.Data.Of<Person>().CreateAsync( people );

      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.NotNull( actual );
      Assert.NotEmpty( actual );
      Assert.True( person_1.age == actual[ 0 ].age || person_2.age == actual[ 0 ].age );
      Assert.True( person_1.name == actual[ 0 ].name || person_2.name == actual[ 0 ].name );
      Assert.True( person_1.age == actual[ 1 ].age || person_2.age == actual[ 1 ].age );
      Assert.True( person_1.name == actual[ 1 ].name || person_2.name == actual[ 1 ].name );
    }

    [Fact]
    public void BulkCreateWrongTableName_BlockCall()
    {
      Area area = new Area();

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Area>().Create( new List<Area> { area } ) );
    }

    [Fact]
    public void BulkCreateWrongTableName_Callback()
    {
      Area area = new Area();

      Backendless.Data.Of<Area>().Create( new List<Area> { area }, new AsyncCallback<IList<String>>(
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
    public void BulkCreateWrongTableName_Async()
    {
      Area area = new Area();
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Area>().CreateAsync( new List<Area> { area } ) );
    }

    [Fact]
    public void BulkCreateEmptyObjects_BlockCall()
    {
      people.Clear();

      Backendless.Data.Of<Person>().Create( people );
      var actual = Backendless.Data.Of<Person>().Find();

      Assert.NotNull( actual );
      Assert.Empty( actual );
    }

    [Fact]
    public void BulkCreateEmptyObjects_Callback()
    {
      people.Clear();

      Backendless.Data.Of<Person>().Create( people, new AsyncCallback<IList<String>>(
      nullable =>
      {
        var actual = Backendless.Data.Of<Person>().Find();

        Assert.NotNull( actual );
        Assert.Empty( actual );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void BulkCreateEmptyObjects_Async()
    {
      people.Clear();

      await Backendless.Data.Of<Person>().CreateAsync( people );
      var actual = Backendless.Data.Of<Person>().Find();

      Assert.NotNull( actual );
      Assert.Empty( actual );
    }
  }
}
