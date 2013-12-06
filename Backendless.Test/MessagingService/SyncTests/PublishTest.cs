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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );
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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );

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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );

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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new AsyncCallback<List<Message>>( messages =>
        {
          try
          {
            Assert.IsNotNull( messages, "Server returned a null object instead of messages list" );

            foreach( Message resultMessage in messages )
            {
              if( resultMessage.MessageId.Equals( messageStatus.MessageId ) )
              {
                Assert.AreEqual( message, resultMessage.Data, "Server returned a message with a wrong message data" );

                foreach( var key in PublishTest.headers.Keys )
                {
                  Assert.IsTrue( resultMessage.Headers.ContainsKey( key ),
                                 "Server returned a message with wrong headers" );
                  Assert.IsTrue( resultMessage.Headers[key].Equals( PublishTest.headers[key] ),
                                 "Server returned a message with wrong headers" );
                }

                latch.Signal();
              }
            }
          }
          catch( System.Exception t )
          {
            FailCountDownWith( t );
          }
        }, FailCountDownWith ), subscriptionOptions );

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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );
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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );
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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );
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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );
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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );
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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );
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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL, new PublishSubscription() );

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

      String channel = "channel" + GetRandomstringMessage();

      subscription = Backendless.Messaging.Subscribe( channel, new PublishSubscription() );

      DeliveryOptions deliveryOptions = new DeliveryOptions();
      deliveryOptions.RepeatEvery = 2;
      messageStatus = Backendless.Messaging.Publish( message, channel, null, deliveryOptions );

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

      subscription = Backendless.Messaging.Subscribe( TEST_CHANNEL,
                                                      new ResponseCallback<List<Message>>( this )
                                                        {
                                                          ResponseHandler = response =>
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

  internal class PublishSubscription : AsyncCallback<List<Message>>
  {
    public PublishSubscription() : base( messages =>
      {
        if( PublishTest.messageStatus == null )
          return;

        Assert.IsNotNull( messages, "Server returned a null object instead of messages list" );

        foreach( Message resultMessage in messages )
        {
          if( resultMessage.MessageId.StartsWith( PublishTest.messageStatus.MessageId ) )
          {
            try
            {
              if( PublishTest.message is WPPerson[] )
              {
                Assert.IsTrue( resultMessage.Data is WPPerson[], "Server returned wrong object type" );

                foreach( WPPerson wpPerson in (WPPerson[]) PublishTest.message )
                  Assert.IsTrue( ((List<WPPerson>) resultMessage.Data).Contains( wpPerson ),
                                 "Server return didn't contain expected object" );
              }
              else
              {
                Assert.AreEqual( PublishTest.message.ToString(), resultMessage.Data.ToString(),
                                 "Server returned a message with a wrong message data" );
              }

              if( PublishTest.publisher != null )
              {
                Assert.AreEqual( PublishTest.publisher, resultMessage.PublisherId,
                                 "Server returned a message with a wrong publisher" );
              }

              if( PublishTest.headers != null )
              {
                foreach( var key in PublishTest.headers.Keys )
                {
                  Assert.IsTrue( resultMessage.Headers.ContainsKey( key ),
                                 "Server returned a message with wrong headers" );
                  Assert.IsTrue( resultMessage.Headers[key].Equals( PublishTest.headers[key] ),
                                 "Server returned a message with wrong headers" );
                }
              }
            }
            catch( System.Exception t )
            {
              PublishTest.FailCountDownWith( t );
            }

            PublishTest.latch.Signal();
          }
        }
      }, fault => PublishTest.FailCountDownWith( fault ) )
    {
    }
  }
}