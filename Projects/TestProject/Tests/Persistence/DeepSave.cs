using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using TestProject.Tests.Utils;
using System.Collections.Generic;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class DeepSave : IDisposable, IClassFixture<DeepSaveInitializator>
  {
    People people = new People();
    public DeepSave()
    {
      people.Name = "Bob";
      people.Age = 20;
      people.ObjectId = "1";
      Backendless.Data.Of<People>().Create( new List<People> { people } );
    }

    public void Dispose()
    {
      Backendless.Data.Of( "Identity" ).Remove( "age>'0'" );
      Backendless.Data.Of( "People" ).Remove( "age>'0'" );
    }

    [Fact]
    public void DeepSaveCreateObject_BlockCall_DS1()
    {
      Identity identity = new Identity();
      identity.Name = "Joe";
      identity.Age = 30;
      identity.Friend[ "objectId" ] = "1";

      var actual = Backendless.Data.Of<Identity>().DeepSave( identity );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual.Name );
      Assert.True( Comparer.IsEqual( identity.Age, actual.Age ) );
      Assert.Equal( people.Name, actual.Friend[ "Name" ] );
      Assert.True( Comparer.IsEqual( people.Age, actual.Friend[ "Age" ] ) );
    }

    [Fact]
    public void DeepSaveCreateObject_Callback_DS1()
    {
      Identity identity = new Identity();
      identity.Name = "Joe";
      identity.Age = 30;
      identity.Friend[ "objectId" ] = "1";

      Backendless.Data.Of<Identity>().DeepSave( identity, new AsyncCallback<Identity>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( identity.Name, actual.Name );
        Assert.True( Comparer.IsEqual( identity.Age, actual.Age ) );
        Assert.Equal( people.Name, actual.Friend[ "Name" ] );
        Assert.True( Comparer.IsEqual( people.Age, actual.Friend[ "Age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void DeepSaveCreateObject_Async_DS1()
    {
      Identity identity = new Identity();
      identity.Name = "Joe";
      identity.Age = 30;
      identity.Friend[ "objectId" ] = "1";

      var actual = await Backendless.Data.Of<Identity>().DeepSaveAsync( identity );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual.Name );
      Assert.True( Comparer.IsEqual( identity.Age, actual.Age ) );
      Assert.Equal( people.Name, actual.Friend[ "Name" ] );
      Assert.True( Comparer.IsEqual( people.Age, actual.Friend[ "Age" ] ) );
    }

    [Fact]
    public void DeepSaveUpdateObject_BlockCall_DS2()
    {
      Identity initializeData = new Identity();
      Identity identityExp = new Identity();
      initializeData.Name = "Joe";
      initializeData.Age = 30;
      initializeData.Friend[ "objectId" ] = "1";
      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( initializeData ).ObjectId;
      identityExp.Age = 25;
      identityExp.Friend[ "Name" ] = "Suzi";
      identityExp.Friend[ "Age" ] = 20;

      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      var actual = Backendless.Data.Of( "Identity" ).DeepSave( map );

      Assert.NotNull( actual );
      Assert.Equal( initializeData.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
      Assert.Equal( identityExp.ObjectId, actual[ "objectId" ] );
      Assert.Equal( identityExp.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
      Assert.True( Comparer.IsEqual( identityExp.Friend[ "Age" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] ) );
    }

    [Fact]
    public void DeepSaveUpdateObject_Callback_DS2()
    {
      Identity initializeData = new Identity();
      Identity identityExp = new Identity();
      initializeData.Name = "Joe";
      initializeData.Age = 30;
      initializeData.Friend[ "objectId" ] = "1";
      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( initializeData ).ObjectId;
      identityExp.Age = 25;
      identityExp.Friend[ "Name" ] = "Suzi";
      identityExp.Friend[ "Age" ] = 20;

      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      Backendless.Data.Of( "Identity" ).DeepSave( map, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( initializeData.Name, actual[ "Name" ] );
        Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
        Assert.Equal( identityExp.ObjectId, actual[ "objectId" ] );
        Assert.Equal( identityExp.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
        Assert.True( Comparer.IsEqual( identityExp.Friend[ "Age" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "The expected error didn't occur" );
      } ) );
    }

    [Fact]
    public async void DeepSaveUpdateObject_Async_DS2()
    {
      Identity initializeData = new Identity();
      Identity identityExp = new Identity();
      initializeData.Name = "Joe";
      initializeData.Age = 30;
      initializeData.Friend[ "objectId" ] = "1";
      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( initializeData ).ObjectId;
      identityExp.Age = 25;
      identityExp.Friend[ "Name" ] = "Suzi";
      identityExp.Friend[ "Age" ] = 20;

      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      var actual = await Backendless.Data.Of( "Identity" ).DeepSaveAsync( map );

      Assert.NotNull( actual );
      Assert.Equal( initializeData.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
      Assert.Equal( identityExp.ObjectId, actual[ "objectId" ] );
      Assert.Equal( identityExp.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
      Assert.True( Comparer.IsEqual( identityExp.Friend[ "Age" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] ) );
    }

    [Fact]
    public void DeepSaveDeleteRelation_BlockCall_DS3()
    {
      Identity initializeData = new Identity();
      Identity identityExp = new Identity();
      initializeData.Name = "Joe";
      initializeData.Age = 30;
      initializeData.Friend[ "objectId" ] = "1";
      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( initializeData ).ObjectId;

      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Friend" ] = null;
      var actual = Backendless.Data.Of( "Identity" ).DeepSave( map );

      Assert.NotNull( actual );
      Assert.Equal( initializeData.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( initializeData.Age, actual[ "Age" ] ) );
      Assert.False( actual.ContainsKey( "Friend" ) );
    }

    [Fact]
    public void DeepSaveDeleteRelation_Callback_DS3()
    {
      Identity initializeData = new Identity();
      Identity identityExp = new Identity();
      initializeData.Name = "Joe";
      initializeData.Age = 30;
      initializeData.Friend[ "objectId" ] = "1";
      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( initializeData ).ObjectId;

      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Friend" ] = null;
      Backendless.Data.Of( "Identity" ).DeepSave( map, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( initializeData.Name, actual[ "Name" ] );
        Assert.True( Comparer.IsEqual( initializeData.Age, actual[ "Age" ] ) );
        Assert.False( actual.ContainsKey( "Friend" ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void DeepSaveDeleteRelation_Async_DS3()
    {
      Identity initializeData = new Identity();
      Identity identityExp = new Identity();
      initializeData.Name = "Joe";
      initializeData.Age = 30;
      initializeData.Friend[ "objectId" ] = "1";
      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( initializeData ).ObjectId;

      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Friend" ] = null;
      var actual = await Backendless.Data.Of( "Identity" ).DeepSaveAsync( map );

      Assert.NotNull( actual );
      Assert.Equal( initializeData.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( initializeData.Age, actual[ "Age" ] ) );
      Assert.False( actual.ContainsKey( "Friend" ) );
    }

    [Fact]
    public void DeepSaveCreateNewObject_BlockCall_DS4()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );

      var actual = Backendless.Data.Of<Identity>().DeepSave( identity );

      Assert.NotNull( actual );
      Assert.Null( actual.Age );
      Assert.Equal( identity.Name, actual.Name );
      Assert.True( Comparer.IsEqual( identity.Family[ 0 ][ "Age" ], actual.Family[ 1 ][ "Age" ] ) );
      Assert.Equal( identity.Family[ 0 ][ "objectId" ], actual.Family[ 1 ][ "objectId" ] );
      Assert.Equal( identity.Family[ 1 ][ "Name" ], actual.Family[ 0 ][ "Name" ] );
    }

    [Fact]
    public void DeepSaveCreateNewObject_Callback_DS4()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );

      Backendless.Data.Of<Identity>().DeepSave( identity, new AsyncCallback<Identity>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Null( actual.Age );
        Assert.Equal( identity.Name, actual.Name );
        Assert.True( Comparer.IsEqual( identity.Family[ 0 ][ "Age" ], actual.Family[ 1 ][ "Age" ] ) );
        Assert.Equal( identity.Family[ 0 ][ "objectId" ], actual.Family[ 1 ][ "objectId" ] );
        Assert.Equal( identity.Family[ 1 ][ "Name" ], actual.Family[ 0 ][ "Name" ] );
      },
      fautl =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void DeepSaveCreateNewObject_Async_DS4()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );

      var actual = await Backendless.Data.Of<Identity>().DeepSaveAsync( identity );

      Assert.NotNull( actual );
      Assert.Null( actual.Age );
      Assert.Equal( identity.Name, actual.Name );
      Assert.True( Comparer.IsEqual( identity.Family[ 0 ][ "Age" ], actual.Family[ 1 ][ "Age" ] ) );
      Assert.Equal( identity.Family[ 0 ][ "objectId" ], actual.Family[ 1 ][ "objectId" ] );
      Assert.Equal( identity.Family[ 1 ][ "Name" ], actual.Family[ 0 ][ "Name" ] );
    }

    [Fact]
    public void DeepSaveUpdateRelation_BlockCall_DS5()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );
      Identity identityExp = new Identity();

      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( identity ).ObjectId;
      identityExp.Age = 50;
      identityExp.Family.Add( new Dictionary<String, Object> { { "objectId", "1" } } );
      var actual = Backendless.Data.Of( "Identity" ).DeepSave( Test_sHelper.ConvertInstanceToMap( identityExp ) );

      Assert.NotNull( actual );
      Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.Equal( firstMap[ "objectId" ], ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "objectId" ] );
      Assert.Equal( identity.Name, ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Name" ] );
      Assert.True( Comparer.IsEqual( firstMap[ "Age" ], ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Age" ] ) );
    }

    [Fact]
    public void DeepSaveUpdateRelation_Callback_DS5()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );
      Identity identityExp = new Identity();

      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( identity ).ObjectId;
      identityExp.Age = 50;
      identityExp.Family.Add( new Dictionary<String, Object> { { "objectId", "1" } } );
      Backendless.Data.Of( "Identity" ).DeepSave( Test_sHelper.ConvertInstanceToMap( identityExp ), new AsyncCallback<Dictionary<string, object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
        Assert.Equal( identity.Name, actual[ "Name" ] );
        Assert.Equal( firstMap[ "objectId" ], ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "objectId" ] );
        Assert.Equal( identity.Name, ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Name" ] );
        Assert.True( Comparer.IsEqual( firstMap[ "Age" ], ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the exectution of the operation" );
      } ) );
    }

    [Fact]
    public async void DeepSaveUpdateRelation_Async_DS5()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );
      Identity identityExp = new Identity();

      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( identity ).ObjectId;
      identityExp.Age = 50;
      identityExp.Family.Add( new Dictionary<String, Object> { { "objectId", "1" } } );
      var actual = await Backendless.Data.Of( "Identity" ).DeepSaveAsync( Test_sHelper.ConvertInstanceToMap( identityExp ) );

      Assert.NotNull( actual );
      Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.Equal( firstMap[ "objectId" ], ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "objectId" ] );
      Assert.Equal( identity.Name, ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Name" ] );
      Assert.True( Comparer.IsEqual( firstMap[ "Age" ], ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Age" ] ) );
    }

    [Fact]
    public void DeepSaveUpdateObjectDeleteRelation_BlockCall_DS6()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );
      Identity identityExp = new Identity();

      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( identity ).ObjectId;
      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Age" ] = null;

      var actual = Backendless.Data.Of( "Identity" ).DeepSave( map );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.False( actual.ContainsKey( "Family" ) );
      Assert.Null( actual[ "Age" ] );
    }

    [Fact]
    public void DeepSaveUpdateObjectDeleteRelation_Callback_DS6()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );
      Identity identityExp = new Identity();

      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( identity ).ObjectId;
      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Age" ] = null;

      Backendless.Data.Of( "Identity" ).DeepSave( map, new AsyncCallback<Dictionary<string, object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( identity.Name, actual[ "Name" ] );
        Assert.False( actual.ContainsKey( "Family" ) );
        Assert.Null( actual[ "Age" ] );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void DeepSaveUpdateObjectDeleteRelation_Async_DS6()
    {
      Identity identity = new Identity();
      identity.Name = "Bob";
      identity.Age = null;
      Dictionary<String, Object> firstMap = new Dictionary<String, Object>();
      Dictionary<String, Object> secondMap = new Dictionary<String, Object>();
      firstMap[ "Age" ] = 15;
      firstMap[ "objectId" ] = "1";
      secondMap[ "Name" ] = "Jack";
      secondMap[ "Age" ] = 20;
      identity.Family.Add( firstMap );
      identity.Family.Add( secondMap );
      Identity identityExp = new Identity();

      identityExp.ObjectId = Backendless.Data.Of<Identity>().DeepSave( identity ).ObjectId;
      var map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Age" ] = null;

      var actual = await Backendless.Data.Of( "Identity" ).DeepSaveAsync( map );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.False( actual.ContainsKey( "Family" ) );
      Assert.Null( actual[ "Age" ] );
    }

    [Fact]
    public void DeepSaveCreateObject_BlockCall_DS7()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      var actual = Backendless.Data.Of( "Identity" ).DeepSave( map );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( identity.Age, actual[ "Age" ] ) );
      Assert.True( actual.ContainsKey( "Friend" ) );
      Assert.True( actual.ContainsKey( "Family" ) );
      Assert.Equal( identity.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
      Assert.True( Comparer.IsEqual( identity.Friend[ "Age" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] ) );
      Assert.True( ( (Dictionary<Object, Object>[]) actual[ "Family" ] ).Length == 2 );
    }

    [Fact]
    public void DeepSaveCreateObject_Callback_DS7()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      Backendless.Data.Of( "Identity" ).DeepSave( map, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( identity.Name, actual[ "Name" ] );
        Assert.True( Comparer.IsEqual( identity.Age, actual[ "Age" ] ) );
        Assert.True( actual.ContainsKey( "Friend" ) );
        Assert.True( actual.ContainsKey( "Family" ) );
        Assert.Equal( identity.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
        Assert.True( Comparer.IsEqual( identity.Friend[ "Age" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] ) );
        Assert.True( ( (Dictionary<Object, Object>[]) actual[ "Family" ] ).Length == 2 );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void DeepSaveCreateObject_Async_DS7()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      var actual = await Backendless.Data.Of( "Identity" ).DeepSaveAsync( map );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( identity.Age, actual[ "Age" ] ) );
      Assert.True( actual.ContainsKey( "Friend" ) );
      Assert.True( actual.ContainsKey( "Family" ) );
      Assert.Equal( identity.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
      Assert.True( Comparer.IsEqual( identity.Friend[ "Age" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] ) );
      Assert.True( ( (Dictionary<Object, Object>[]) actual[ "Family" ] ).Length == 2 );
    }

    [Fact]
    public void DeepSaveCreateObject_BlockCall_DS8()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );
      Identity identityExp = new Identity();

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      identityExp.ObjectId = (String) Backendless.Data.Of( "Identity" ).DeepSave( map )[ "objectId" ];
      identityExp.Age = 100;
      identityExp.Family.Add( new Dictionary<String, Object> { { "objectId", "1" }, { "Age", 10 } } );
      map.Clear();
      map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Friend" ] = null;

      var actual = Backendless.Data.Of( "Identity" ).DeepSave( map );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
      Assert.True( actual.ContainsKey( "Family" ) );
      Assert.False( actual.ContainsKey( "Friend" ) );
      Assert.True( Comparer.IsEqual( identity.Age, ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Age" ] ) );
    }

    [Fact]
    public void DeepSaveCreateObject_Callback_DS8()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );
      Identity identityExp = new Identity();

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      identityExp.ObjectId = (String) Backendless.Data.Of( "Identity" ).DeepSave( map )[ "objectId" ];
      identityExp.Age = 100;
      identityExp.Family.Add( new Dictionary<String, Object> { { "objectId", "1" }, { "Age", 10 } } );
      map.Clear();
      map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Friend" ] = null;

      Backendless.Data.Of( "Identity" ).DeepSave( map, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( identity.Name, actual[ "Name" ] );
        Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
        Assert.True( actual.ContainsKey( "Family" ) );
        Assert.False( actual.ContainsKey( "Friend" ) );
        Assert.True( Comparer.IsEqual( identity.Age, ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void DeepSaveCreateObject_Async_DS8()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );
      Identity identityExp = new Identity();

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      identityExp.ObjectId = (String) Backendless.Data.Of( "Identity" ).DeepSave( map )[ "objectId" ];
      identityExp.Age = 100;
      identityExp.Family.Add( new Dictionary<String, Object> { { "objectId", "1" }, { "Age", 10 } } );
      map.Clear();
      map = Test_sHelper.ConvertInstanceToMap( identityExp );
      map[ "Friend" ] = null;

      var actual = await Backendless.Data.Of( "Identity" ).DeepSaveAsync( map );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( identityExp.Age, actual[ "Age" ] ) );
      Assert.True( actual.ContainsKey( "Family" ) );
      Assert.False( actual.ContainsKey( "Friend" ) );
      Assert.True( Comparer.IsEqual( identity.Age, ( (Dictionary<Object, Object>[]) actual[ "Family" ] )[ 0 ][ "Age" ] ) );
    }

    [Fact]
    public void DeepSaveCreateAndDeleteRelation_BlockCall_DS9()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );
      Identity identityExp = new Identity();

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      identityExp.ObjectId = (String) Backendless.Data.Of( "Identity" ).DeepSave( map )[ "objectId" ];
      identityExp.Friend[ "Name" ] = "Lolla";
      map.Clear();
      map = Test_sHelper.ConvertInstanceToMap( identityExp );
      ( (Dictionary<String, Object>) map[ "Friend" ] )[ "Age" ] = null;

      var actual = Backendless.Data.Of( "Identity" ).DeepSave( map );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( identity.Age, actual[ "Age" ] ) );
      Assert.False( actual.ContainsKey( "Family" ) );
      Assert.True( actual.ContainsKey( "Friend" ) );
      Assert.Equal( identityExp.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
      Assert.Null( ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] );
    }

    [Fact]
    public void DeepSaveCreateAndDeleteRelation_Callback_DS9()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );
      Identity identityExp = new Identity();

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      identityExp.ObjectId = (String) Backendless.Data.Of( "Identity" ).DeepSave( map )[ "objectId" ];
      identityExp.Friend[ "Name" ] = "Lolla";
      map.Clear();
      map = Test_sHelper.ConvertInstanceToMap( identityExp );
      ( (Dictionary<String, Object>) map[ "Friend" ] )[ "Age" ] = null;
      Backendless.Data.Of( "Identity" ).DeepSave( map, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( identity.Name, actual[ "Name" ] );
        Assert.True( Comparer.IsEqual( identity.Age, actual[ "Age" ] ) );
        Assert.False( actual.ContainsKey( "Family" ) );
        Assert.True( actual.ContainsKey( "Friend" ) );
        Assert.Equal( identityExp.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
        Assert.Null( ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the expected of the operation" );
      } ) );
    }

    [Fact]
    public async void DeepSaveCreateAndDeleteRelation_Async_DS9()
    {
      Identity identity = new Identity();
      identity.Name = "Liza";
      identity.Age = 10;
      identity.Friend[ "Name" ] = "Brother";
      identity.Friend[ "Age" ] = 20;
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Anna" }, { "Age", 10 } } );
      identity.Family.Add( new Dictionary<String, Object> { { "Name", "Dad" }, { "Age", 20 } } );
      Identity identityExp = new Identity();

      var map = Test_sHelper.ConvertInstanceToMap( identity );
      identityExp.ObjectId = (String) Backendless.Data.Of( "Identity" ).DeepSave( map )[ "objectId" ];
      identityExp.Friend[ "Name" ] = "Lolla";
      map.Clear();
      map = Test_sHelper.ConvertInstanceToMap( identityExp );
      ( (Dictionary<String, Object>) map[ "Friend" ] )[ "Age" ] = null;

      var actual = await Backendless.Data.Of( "Identity" ).DeepSaveAsync( map );

      Assert.NotNull( actual );
      Assert.Equal( identity.Name, actual[ "Name" ] );
      Assert.True( Comparer.IsEqual( identity.Age, actual[ "Age" ] ) );
      Assert.False( actual.ContainsKey( "Family" ) );
      Assert.True( actual.ContainsKey( "Friend" ) );
      Assert.Equal( identityExp.Friend[ "Name" ], ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Name" ] );
      Assert.Null( ( (Dictionary<Object, Object>) actual[ "Friend" ] )[ "Age" ] );
    }
  }
}