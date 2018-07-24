using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Messaging;
using BackendlessAPI.Test.PersistenceService.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.MessagingService.SyncTests
{
  [TestClass]
  public class PublishTest : TestsFrame
  {
    [TestMethod]
    public void TestBasicMessagePublish()
    {
      SetLatch();
      SetMessage();

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagePublishWithOptions()
    {
      SetLatch();
      SetMessage();
      SetPublisher();

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );

      PublishOptions publishOptions = new PublishOptions();
      publishOptions.PublisherId = publisher;

      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL, publishOptions );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagePublishWithHeaders()
    {
      SetLatch();
      SetMessage();
      SetHeaders();

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );

      PublishOptions publishOptions = new PublishOptions();
      publishOptions.Headers = headers;

      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL, publishOptions );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagePublishWithHeadersAndSubtopics()
    {
      SetLatch();
      SetMessage();
      SetHeaders();

      string subtopic = "foo.bar";

      SubscriptionOptions subscriptionOptions = new SubscriptionOptions();
      subscriptionOptions.Subtopic = subtopic;

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );

      channel.AddMessageListener<Message>((resultMessage) =>
      {
        if (resultMessage.MessageId.Equals(messageStatus.MessageId))
        {
          Assert.AreEqual(message, resultMessage.Data, "Server returned a message with a wrong message data");

          foreach (var key in PublishTest.headers.Keys)
          {
            Assert.IsTrue(resultMessage.Headers.ContainsKey(key),
              "Server returned a message with wrong headers");
            Assert.IsTrue(resultMessage.Headers[key].Equals(PublishTest.headers[key]),
              "Server returned a message with wrong headers");
          }
        }
      });
          
      PublishOptions publishOptions = new PublishOptions();
      publishOptions.Headers = headers;
      publishOptions.Subtopic = subtopic;

      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL, publishOptions );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagePublishPrimitiveValue()
    {
      SetLatch();
      SetMessage( 16 );

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagePublishStringValue()
    {
      SetLatch();
      SetMessage( "foomessage" );

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagePublishDateValue()
    {
      SetLatch();
      SetMessage( DateTime.Now );

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagePublishBoolValue()
    {
      SetLatch();
      SetMessage( true );

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [Ignore]
    [TestMethod]
    public void TestMessagePublishPOJOValue()
    {
      SetLatch();

      var wpPerson = new WPPerson {Age = 22, Name = "Vasya"};
      SetMessage( wpPerson );

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [Ignore]
    [TestMethod]
    public void TestMessagePublishArrayOfPOJOValue()
    {
      SetLatch();

      var person123451 = new WPPerson {Age = 22, Name = "Vasya"};
      var androidPerson123452 = new WPPerson {Age = 35, Name = "Petya"};

      SetMessage( new[] {person123451, androidPerson123452} );

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagePublishSinglecast()
    {
      SetLatch();
      SetMessage();

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );

      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.PushSinglecast = new List<string> {deviceRegistrationId};
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL, null, deliveryOptions );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.PUBLISHED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    [TestMethod]
    public void TestMessagesScheduling()
    {
      SetLatch( 5 );
      SetMessage();

      String channelName = "channel" + GetRandomstringMessage();

      channel = Backendless.Messaging.Subscribe( channelName );

      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.RepeatEvery = 2;
      messageStatus = Backendless.Messaging.Publish( message, channelName, null, deliveryOptions );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.SCHEDULED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }

    private int counter;

    [TestMethod]
    public void TestMessagesScheduleCancel()
    {
      SetLatch( 11 );
      SetMessage();
      counter = 0;

      channel = Backendless.Messaging.Subscribe( TEST_CHANNEL );
        
        channel.AddMessageListener<Message>((message) =>
          {
            if( ++counter == 10 )
            {
              try
              {
                Assert.IsTrue(
                  Backendless.Messaging.Cancel( messageStatus.MessageId ),
                  "Server returned a wrong result status" );
              }
              catch( System.Exception t )
              {
                FailCountDownWith( t );
              }
              finally
              {
                latch.Signal();
              }
            }        
          } );

      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.RepeatEvery = 2;
      messageStatus = Backendless.Messaging.Publish( message, TEST_CHANNEL, deliveryOptions );

      Assert.IsNotNull( messageStatus.MessageId, "Server didn't set a messageId for the message" );
      Assert.IsTrue( messageStatus.Status.Equals( PublishStatusEnum.SCHEDULED ),
                     "Message status was not set as published" );

      Await();
      CheckResult();
    }
  }
}