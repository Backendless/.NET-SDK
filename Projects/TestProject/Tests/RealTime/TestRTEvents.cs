using Xunit;
using System;
using BackendlessAPI;
using System.Threading;
using BackendlessAPI.RT;
using TestProject.Tests.Utils;
using System.Collections.Generic;

namespace TestProject.Tests.RealTime
{
  [Collection("Tests")]
  public class TestRTCreateEvents : IDisposable
  {
    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age>'0'" );
    }

    Dictionary<String, Object> person = new Dictionary<String, Object>();

    [Fact]
    public void TestRTCreatedEvent()
    {
      person[ "age" ] = 20;

      CountdownEvent countdown = new CountdownEvent( 1 );

      ConnectErrorListener error = new ConnectErrorListener( fault =>
      {
        Assert.True( false, "Failed to establish connection:" + fault.Message );
        countdown.Signal();
      } );


      Test_sHelper.orderEventHandler.AddCreateListener(
      createdObject =>
      {
        countdown.Signal();
        Assert.NotNull( createdObject );
        Assert.Contains( "objectId", (IDictionary<String, Object>) createdObject );
      } );

      ConnectListener connect = new ConnectListener( () =>
      {
        Thread.Sleep( 500 );
        Backendless.Data.Of( "Person" ).Save( person );
      } );

      Backendless.RT.AddConnectListener( connect );
      Backendless.RT.AddConnectErrorListener( error );
      countdown.Wait(50000);
      Test_sHelper.orderEventHandler.RemoveCreateListeners();
      Backendless.RT.RemoveConnectListener( connect );
      Backendless.RT.RemoveConnectErrorListener( error );
    }

    [Fact]
    public void TestRTCreatedEventWhereClause()
    {
      person[ "age" ] = 20;

      CountdownEvent countdown = new CountdownEvent( 1 );

      ConnectErrorListener error = new ConnectErrorListener( fault =>
      {
        Assert.True( false, "Failed to establish connection:" + fault.Message );
        countdown.Signal();
      } );


      Test_sHelper.orderEventHandler.AddCreateListener( "age>'0'",
      createdObject =>
      {
        countdown.Signal();
        Assert.NotNull( createdObject );
        Assert.Contains( "objectId", (IDictionary<String, Object>) createdObject );
      } );

      ConnectListener connect = new ConnectListener( () =>
      {
        Thread.Sleep( 750 );
        Backendless.Data.Of( "Person" ).Save( person );
      } );

      Backendless.RT.AddConnectListener( connect );
      Backendless.RT.AddConnectErrorListener( error );
      countdown.Wait( 50000 );
      Test_sHelper.orderEventHandler.RemoveCreateListeners();
      Backendless.RT.RemoveConnectListener( connect );
      Backendless.RT.RemoveConnectErrorListener( error );
    }
  }
}
