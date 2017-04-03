using System;
using System.Collections.Generic;
using System.Threading;
using Backendless.Test;
using BackendlessAPI.Exception;
using BackendlessAPI.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.MessagingService.SyncTests
{
  [TestClass]
  public class TestsFrame : IAsyncTest
  {
    public string TEST_CHANNEL = "TestChannel";
    public string TEST_CHANNEL_2 = "TestChannel2";
    public string SENDER_ID = "386814968753";

    public string deviceRegistrationId;
    public static CountdownEvent latch;
    public static MessageStatus messageStatus;
    public static object message;
    public static string publisher;
    public static Dictionary<string, string> headers;
    public Subscription subscription;

    public static string testResult;

    public Random Random = new Random();

    public static void SetTestResult( System.Exception ex )
    {
      testResult = ex.Message;
    }

    public static void SetTestResult( string result )
    {
      testResult = result;
    }

    public void CheckResult()
    {
      if( testResult != null )
        FailCountDownWith( testResult );
    }

    public void SetLatch()
    {
      SetLatch( 1 );
    }

    public void SetLatch( int count )
    {
      latch = new CountdownEvent( count );
    }

    public static void FailCountDownWith( System.Exception ex )
    {
      SetTestResult( ex );
      CountDownAll();
    }

    public static void FailCountDownWith( BackendlessFault fault )
    {
      SetTestResult( fault.ToString() );
      CountDownAll();
    }

    private static void CountDownAll()
    {
      for( int i = 0; i < latch.CurrentCount; i++ )
        latch.Signal();
    }

    public void SetMessage()
    {
      message = GetRandomstringMessage();
    }

    public void SetMessage( Object value )
    {
      message = value;
    }

    public void SetPublisher()
    {
      publisher = GetRandomPublisher();
    }

    public void SetHeaders()
    {
      var stamp = (DateTime.Now.Ticks + Random.Next()).ToString();

      headers = new Dictionary<string, string>();
      headers.Add( "key#" + stamp, "value#" + stamp );
    }

    public void Await()
    {
      Assert.IsTrue( latch.Wait( new TimeSpan( 0, 2, 0 ) ), "Server didn't receive a message in time." );
    }

    public string GetRandomstringMessage()
    {
      string timestamp = (DateTime.Now.Ticks + Random.Next()).ToString();

      return "message#" + timestamp;
    }

    public string GetRandomPublisher()
    {
      string timestamp = (DateTime.Now.Ticks + Random.Next()).ToString();

      return "publisher#" + timestamp;
    }

    [TestInitialize]
    public void SetUp()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );

      messageStatus = null;
      latch = null;
      message = null;
      publisher = null;
      headers = null;
      testResult = null;
    }

    [TestCleanup]
    public void tearDown()
    {
      if( subscription != null )
        subscription.CancelSubscription();
    }
  }
}