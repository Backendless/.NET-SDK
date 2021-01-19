using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;

namespace TestProject
{
  [Collection("Tests")]
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
    public void TestBulkCreateBlockCall()
    {
      Backendless.Data.Of<Person>().Create( people );

      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.NotNull( actual );
      Assert.NotEmpty( actual );  
      Assert.True( person_1.age == actual[ 0 ].age || person_2.age == actual[0].age );
      Assert.True( person_1.name == actual[ 0 ].name || person_2.name == actual[ 0 ].name );
      Assert.True( person_1.age == actual[ 1 ].age || person_2.age == actual[ 1 ].age );
      Assert.True( person_1.name == actual[ 1 ].name || person_2.name == actual[ 1 ].name );
    }

    [Fact]
    public void TestBulkCreateCallback()
    {
      Backendless.Data.Of<Person>().Create( people, new AsyncCallback<IList<String>>(
      objectIds=>
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
    public async void TestBulkCreateAsync()
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
  }
}
