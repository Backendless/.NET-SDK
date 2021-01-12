using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System.Collections.Generic;

namespace TestProject
{
  [Collection("Tests")]
  public class ExcludePropertiesTests : IDisposable
  {
    Dictionary<String, Object> data = new Dictionary<String, Object>();
    Dictionary<String, Object> dataIdParent_1;
    Dictionary<String, Object> dataIdParent_2;
    public ExcludePropertiesTests()
    {
      data.Add( "name", "Joe" );
      data.Add( "age", 23 );
      dataIdParent_1 = Backendless.Data.Of( "Person" ).Save( data );

      data.Clear();
      data.Add( "name", "Tom" );
      data.Add( "age", 20 );
      dataIdParent_2 = Backendless.Data.Of( "Person" ).Save( data );
    }

    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestExcludeTwoFields()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.ExcludeProperties( "name", "age" );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Person" ).Find( queryBuilder );

      Assert.False( res[0].ContainsKey( "name" ), "First object is contains key 'name'" );
      Assert.False( res[0].ContainsKey( "age" ), "First object is contains key 'age'" );
                    
      Assert.False( res[1].ContainsKey( "name" ), "First object is contains key 'name'" );
      Assert.False( res[1].ContainsKey( "age" ), "First object is contains key 'age'" );
    }

    [Fact]
    public void TestCreateFieldTIME()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "TIME(created) as myTime" );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Person" ).Find( queryBuilder );

      Assert.True( res[ 0 ].ContainsKey( "myTime" ), "First object does not contain 'myTime' key" );
      Assert.True( res[ 1 ].ContainsKey( "myTime" ), "Second object does not contain 'myTime' key" );
    }

    [Fact]
    public void TestRelatedField()
    {
      data.Clear();
      data.Add( "adress", "Curse Street" );
      Dictionary<String, Object> dataIdChildren_1 = Backendless.Data.Of( "Location" ).Save( data );//First object in the "Location" table

      data.Clear();
      data.Add( "adress", "Tom Street" );
      Dictionary<String, Object> dataIdChildren_2 = Backendless.Data.Of( "Location" ).Save( data );//Second object int the "Location" table

      Object[] children = new Object[] { dataIdChildren_1 };

      Backendless.Data.Of( "Person" ).SetRelation( dataIdParent_1, "Location:Location:n", children );//First relation

      children = new Object[] { dataIdChildren_2 };

      Backendless.Data.Of( "Person" ).SetRelation( dataIdParent_2, "Location:Location:n", children );//Second relation
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "Location.adress" );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Person" ).Find( queryBuilder );

      Assert.True( res[ 0 ].ContainsKey( "adress" ), "First object does not contain 'adress' field" );
      Assert.True( res[ 1 ].ContainsKey( "adress" ), "Second object does not contain 'adress' field" );
    }
  }
}