using Xunit;
using System;
using BackendlessAPI;
using System.Threading;
using System.Collections.Generic;
using BackendlessAPI.RT.Messaging;

namespace TestProject.Tests.RealTime
{
  [Collection("Tests")]
  public class TestSubscriptionAPI
  {
    [Fact]
    public void TestSubscribeDefaultChannel()
    {
      IChannel channel = Backendless.Messaging.Subscribe();
      
      while( !channel.IsJoined() )
      {
      }

      Assert.True( channel.IsJoined() );
    }

    [Fact]
    public void TestSubscribeCustomChannel()
    {
      IChannel channel = Backendless.Messaging.Subscribe( "TestChannel" );

      while( !channel.IsJoined() )
      {
      }

      Assert.True( channel.IsJoined() );
    }

    [Fact]
    public void TestReceivingAndPublishingMessages_String()
    {
      CountdownEvent countdown = new CountdownEvent( 1 );
      IChannel channel = Backendless.Messaging.Subscribe();

      channel.AddMessageListener<String>( message =>
      {
        Assert.Equal( "mes", message );
        countdown.Signal();
      } );

      Thread.Sleep(5000 );
      Backendless.Messaging.Publish( "mes" );
      countdown.Wait( 10000 );
    }

    [Fact]
    public void TestReceivingAndPublishMessages_Dictionary()
    {
      CountdownEvent countdown = new CountdownEvent( 1 );
      IChannel channel = Backendless.Messaging.Subscribe();
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "age" ] = 20;

      while( !channel.IsJoined() )
      {
      }

      MessageReceived<Dictionary<String, Object>> messageListener = ( personObject ) =>
      {
        Assert.IsType<Dictionary<String, Object>>( personObject );
        Assert.True( Comparer.IsEqual( personObject[ "age" ], person[ "age" ] ) );
        countdown.Signal();
      };

      channel.AddMessageListener<Dictionary<String, Object>>( messageListener );

      Thread.Sleep( 500 );
      Backendless.Messaging.Publish( person );
      countdown.Wait( 10000 );
    }

    [Fact]
    public void TestReceivingAndPublishMessages_Class()
    {
      CountdownEvent countdown = new CountdownEvent( 1 );
      IChannel channel = Backendless.Messaging.Subscribe();
      Person person = new Person();
      person.age = 20;

      while( !channel.IsJoined() )
      {
      }

      MessageReceived<Person> messageListener = ( personObject ) =>
      {
        Assert.IsType<Person>( personObject );
        Assert.True( Comparer.IsEqual( personObject.age, person.age ) );
        countdown.Signal();
      };

      channel.AddMessageListener<Person>( messageListener );

      Thread.Sleep( 500 );
      Backendless.Messaging.Publish( person );
      countdown.Wait( 10000 );
    }
  }
}
