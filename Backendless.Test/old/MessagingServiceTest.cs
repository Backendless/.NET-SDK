using BackendlessAPI;
using BackendlessAPI.Exception;
using BackendlessAPI.Messaging;
using BackendlessAPI.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI.Property;
using System.Collections.Generic;
namespace BackendlessAPI.Test
{
    
    
    /// <summary>
    ///This is a test class for MessagingServiceMessagFileTest and is intended
    ///to contain all MessagingServiceMessagFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MessagingServiceMessagFileTest
    {

        MessagingService messagingService = new MessagingService();
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string applicationId = "validApp-Ids0-0000-0000-000000000000"; // TODO: Initialize to an appropriate value
            string secretKey = "validSec-retK-eys0-0000-000000000000"; // TODO: Initialize to an appropriate value
            string version = "v1"; // TODO: Initialize to an appropriate value
            Backendless.InitApp(applicationId, secretKey, version);
        }
        [TestMethod()]
        public void publishSimpleMessageTest()
        {
            try
            {
                var message=messagingService.Publish("testChannel", "Test message! Hello Word!!!");
                Assert.IsNotNull(message.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void publishMessageWithHeadersNoSubtopicTest()
        {
            try
            {
                string channel = "testChannel";
                object message = "Test message! Hello Word!!!";
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPablisher",
                    Headers = dict
                };
                var m = messagingService.Publish(message, channel, option);
                Assert.IsNotNull(m.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void publishMessageToSubtopicWithHeadersTest()
        {
            try
            {
                string channel = "testChannel";
                object message = "Test message! Hello Word!!!";
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPablisher",
                    Headers = dict,
                    Subtopic = "testSubtopic"
                };
                var m = messagingService.Publish(message, channel, option);
                Assert.IsNotNull(m.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void publishMessagePrimitiveValueTest()
        {
            try
            {
                string channel = "testChannel";
                object message = 12345;
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPablisher",
                    Headers = dict,
                    Subtopic = "testSubtopic"
                };
                var m = messagingService.Publish(message, channel, option);
                Assert.IsNotNull(m.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void publishMessageStringValueTest()
        {
            try
            {
                string channel = "testChannel";
                object message = "Test message! Hello Word!!!";
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPablisher",
                    Headers = dict,
                    Subtopic = "testSubtopic"
                };
                var m = messagingService.Publish(message, channel, option);
                Assert.IsNotNull(m.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void publishMessageDateValueTest()
        {
            try
            {
                string channel = "testChannel";
                object message = DateTime.Now;
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPablisher",
                    Headers = dict,
                    Subtopic = "testSubtopic"
                };
                var m = messagingService.Publish(message, channel, option);
                Assert.IsNotNull(m.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void publishMessageBoolValueTest()
        {
            try
            {
                string channel = "testChannel";
                object message = true;
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPablisher",
                    Headers = dict,
                    Subtopic = "testSubtopic"
                };
                var m = messagingService.Publish(message, channel, option);
                Assert.IsNotNull(m.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void publishMessageObjectValueTest()
        {
            try
            {
                string channel = "testChannel";
                object message = new Person { Name = "Vasya", Age = 22 };
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPablisher",
                    Headers = dict,
                    Subtopic = "testSubtopic"
                };
                var m = messagingService.Publish(message, channel, option);
                Assert.IsNotNull(m.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void publishMessageArrayValueTest()
        {
            try
            {
                string channel = "testChannel";
                object message = new List<Person> { new Person { Name = "Vasya", Age = 22 } };
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPablisher",
                    Headers = dict,
                    Subtopic = "testSubtopic"
                };
                var m = messagingService.Publish(message, channel, option);
                Assert.IsNotNull(m.MessageId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        /*[TestMethod()]
        public void getRegistrationsTest()
        {
            try
            {
                messagingService.RegisterDevice(DateTime.Now.AddDays(2));
                List<DeviceRegistration> list = messagingService.GetRegistrations();
                Assert.IsNotNull(list);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }*/
        [TestMethod()]
        public void cancelScheduleMessageTest()
        {
            try
            {
                string channel = "testChannel";
                object message = "Test message! Hello Word!!!";
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPublisher",
                    Headers = dict
                };
                DeliveryOptions delivery = new DeliveryOptions
                {
                };
                var m = messagingService.Publish(message, channel, option);
                var cancel = messagingService.Cancel(m.MessageId);
                Assert.IsNotNull(cancel);
            }
            catch (BackendlessException ex)
            {
                //Can't cancel message. Already cancelled or there is no such message
                Assert.AreEqual("5040", ex.Code);
            }
        }
        [TestMethod()]
        public void cancelScheduleMessageWithWasPublishedTest()
        {
            try
            {
                string channel = "testChannel";
                object message = "Test message! Hello Word!!!";
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("City", "Kiev");
                dict.Add("Temp", "22");
                PublishOptions option = new PublishOptions
                {
                    PublisherId = "testPublisher",
                    Headers = dict
                };
                DeliveryOptions delivery = new DeliveryOptions
                {
                };
                var m = messagingService.Publish(message, channel, option);
                var cancel = messagingService.Cancel(m.MessageId);
                Assert.IsNotNull(cancel);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("5040", ex.Code);
            }
        }
        //[TestMethod()]
        //public void establishingSubscriptionWithSubtopicTest()
        //{
        //    messagingService.Subscribe("");
        //}
        /*[TestMethod()]
        public void RegisterDeviceTest()
        {
            try
            {
                string device = messagingService.RegisterDevice(DateTime.Now.AddDays(10));
                Assert.IsNotNull(device);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

        }
         [TestMethod()]
        public void registerDeviceWithExpirationTest()
        {
            try
            {
                DateTime expiration = DateTime.Now.AddDays(1);
                string registrationId = messagingService.RegisterDevice(expiration);
                Assert.IsNotNull(registrationId);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

         [TestMethod()]
         public void registerDeviceWithWrongExpirationTest()
         {
             try
             {
                 DateTime expiration = DateTime.Now.AddDays(-1);
                 string registrationId = messagingService.RegisterDevice(expiration);
                 Assert.IsNotNull(registrationId);
             }
             catch (BackendlessException ex)
             {
                 Assert.AreEqual("5004", ex.Code);
             }
         }
         [TestMethod()]
         public void registerDeviceWithMultipleChannelsTest()
         {
             try
             {
                 DateTime expiration = DateTime.Now.AddDays(1);
                 List<string> channels = new List<string> { "channel1", "channel2" };
                 string registrationId = messagingService.RegisterDevice(channels,expiration);
                 Assert.IsNotNull(registrationId);
             }
             catch (BackendlessException ex)
             {
                 Assert.Fail(ex.Message);
             }
         }





        /// <summary>
        ///A test for RegisterDevice
        ///</summary>
        [TestMethod()]
        public void RegisterDeviceWithProperStringValueTest()
        {

            List<string> channels = new List<string> { "test1","test2"}; // TODO: Initialize to an appropriate value
            try
            {
                string device = messagingService.RegisterDevice(channels, DateTime.Now.AddDays(2));
                Assert.IsNotNull(device);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("123", e.Code);
            }
           
        }

        [TestMethod()]
        public void RegisterDeviceTest2()
        {
            try
            {
                DateTime expiration = DateTime.Now.AddDays(2);
                List<string> channels = new List<string> { "channel1", "channel2" };
                string registrationId = messagingService.RegisterDevice(channels, expiration);
                Assert.IsNotNull(registrationId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void RegisterDeviceWithNotExistParameterTest()
        {
            try
            {
                List<string> channels = new List<string> { "channel103" };
                string registrationId = messagingService.RegisterDevice(channels, DateTime.Now.AddDays(2));
                Assert.IsNotNull(registrationId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void RegisterDeviceWithWrongValueOfParameterTest()
        {
            try
            {
                List<string> channels = new List<string> { "-132" };
                string registrationId = messagingService.RegisterDevice(channels, DateTime.Now.AddDays(2));
                Assert.IsNotNull(registrationId);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("123", ex.Code);
            }
        }
        [TestMethod()]
        public void RegisterAfterItExpiresTest()
        {
            try
            {
                DateTime actualTime = DateTime.Now.AddDays(1000);
                List<string> channels = new List<string> {"channel1", "channel2" };
                string registrationId = messagingService.RegisterDevice(channels,actualTime);
                Assert.IsNotNull(registrationId);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }



        /// <summary>
        ///A test for RegisterDevice
        ///</summary>
        [TestMethod()]
        public void RegisterDeviceWithProperStringValueTest1()
        {
            DateTime expiration = DateTime.Now.AddDays(10); // TODO: Initialize to an appropriate value
            try
            {
                 string expected= messagingService.RegisterDevice(expiration);
                 Assert.IsNotNull(expected);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
          
        }
        [TestMethod()]
        public void RegisterDeviceWithOutProperStringValueTest1()
        {
            DateTime expiration = new DateTime();
            try
            {
                string expected = messagingService.RegisterDevice(expiration);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("5004", e.Code);
            }
        }*/

        /*/// <summary>
        ///A test for RegisterDevice
        ///</summary>
        [TestMethod()]
        public void RegisterDeviceWithProperStringValueTest2()
        {

            List<string> channels = new List<string> { "test1", "test2" }; // TODO: Initialize to an appropriate value
            DateTime expiration = DateTime.Now.AddDays(2); // TODO: Initialize to an appropriate value
            try
            {
                string expected = messagingService.RegisterDevice(channels, expiration);
                Assert.IsNotNull(expected);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            } 
        }
        [TestMethod()]
        public void RegisterDeviceWithOutProperStringValueTest2()
        {
            List<string> channels = null; 
            DateTime expiration = new DateTime();
            try
            {
                string expected = messagingService.RegisterDevice(channels, expiration);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }
        }*/

        ///// <summary>
        /////A test for RegisterDevice
        /////</summary>
        //[TestMethod()]
        //public void RegisterDeviceWithProperStringValueTest3()
        //{
        //    try
        //    {
        //        string expected = messagingService.RegisterDevice();
        //        Assert.IsNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        ///// <summary>
        /////A test for GetRegistrations
        /////</summary>
        //[TestMethod()]
        //public void GetRegistrationsWithProperStringValueTest()
        //{
        //    try
        //    {
        //        List<DeviceRegistration> list = messagingService.GetRegistrations();
        //        Assert.IsNotNull(list);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        ///// <summary>
        /////A test for GetRegistrations
        /////</summary>
      
        //[TestMethod()]
        //public void GetRegistrationByIdWithProperStringValueTest()
        //{
        //    try
        //    {
        //        DeviceRegistration expected = messagingService.GetRegistrationById("1");
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    } 
        //}
        //[TestMethod()]
        //public void GetRegistrationByIdWithOutProperStringValueTest()
        //{
        //    try
        //    {
        //        DeviceRegistration expected = messagingService.GetRegistrationById("1");
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        ///// <summary>
        /////A test for GetRegistrationById
        /////</summary>
      
        //[TestMethod()]
        //public void UnregisterDeviceWithProperStringValueTest()
        //{
        //    try
        //    {
        //        bool expected = messagingService.UnregisterDevice();
        //        Assert.IsTrue(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        ///// <summary>
        /////A test for Publish
        /////</summary>
        //[TestMethod()]
        //public void PublishWithProperStringValueTest()
        //{
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish("test", "messagetest");
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        ///// <summary>
        /////A test for Publish
        /////</summary>
        //[TestMethod()]
        //public void PublishWithProperStringValueTest1()
        //{
            
        //    string channelName = string.Empty; 
        //    object message = null;
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, message, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PublishMessageWithHeadersNoSubtopicTest()
        //{
        //    string channelName = null;
        //    Dictionary<string, string> Headers = new Dictionary<string, string>();
        //    Headers.Add("message", "Test message! Hallo Word!!!");
        //    Headers.Add("City", "kiev");
        //    Headers.Add("Temp", "22");
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, Headers, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PublishMessageToSubtopicWithHeadersTest()
        //{
        //    string channelName = null;
        //    Dictionary<string, string> Headers = new Dictionary<string, string>();
        //    Headers.Add("message", "Test message! Hallo Word!!!");
        //    Headers.Add("City", "kiev");
        //    Headers.Add("Temp", "22");
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, Headers, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PublishMessagePrimitiveValueTest()
        //{
        //    string channelName = null;
        //    Dictionary<string, string> Headers = new Dictionary<string, string>();
        //    Headers.Add("message", "123");
        //    Headers.Add("City", "kiev");
        //    Headers.Add("Temp", "22");
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, Headers, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PublishMessageObjectValueTest()
        //{
        //    string channelName = "testb";
        //    Dictionary<string, string> Headers = new Dictionary<string, string>();
        //    Headers.Add("message", "{'Person':[{'name':'Vasya'},{'age':'22'}]}");
        //    Headers.Add("City", "kiev");
        //    Headers.Add("Temp", "22");
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, Headers, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PublishMessageArrayValueTest()
        //{
        //    string channelName = "testb";
        //    Dictionary<string, string> Headers = new Dictionary<string, string>();
        //    Headers.Add("message", "['Person',[{'name':'Vasya'},{'age':'22'}]]");
        //    Headers.Add("City", "kiev");
        //    Headers.Add("Temp", "22");
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, Headers, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PublishMessageAsSinglecastTest()
        //{
        //    string channelName = "testb";
        //    Dictionary<string, string> Headers = new Dictionary<string, string>();
        //    Random radom=new Random((int)DateTime.Now.Ticks);
        //    String strPublisherId ="1"+ radom.Next(1000,9999);
        //    Headers.Add("message", "Test message! Hallo Word!!!");
        //    Headers.Add("publisherId", strPublisherId);
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, Headers, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PublishMessageBoolValueTest()
        //{
   
        //    string channelName = "testb";
        //    object message =true;
            
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, message, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PublishWithDateProperStringValueTest1()
        //{

        //    string channelName = "testa";
        //    object message = DateTime.Now;

        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, message, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        ///// <summary>
        /////A test for Publish
        /////</summary>
        //[TestMethod()]
        //public void PublishWithProperStringValueTest2()
        //{

        //    string channelName = "test"; 
        //    object message = "123"; 
        //    PublishOptions publishOptions = new PublishOptions("1") ; 
        //    DeliveryOptions deliveryOptions = new DeliveryOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, message, publishOptions, deliveryOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}


        ///// <summary>
        /////A test for Cancel
        /////</summary>
        //[TestMethod()]
        //public void CancelWithProperStringValueTest()
        //{
        //    string messageId = "1";
        //    try
        //    {
        //        bool expected = messagingService.Cancel(messageId);
        //        Assert.IsTrue(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void CancelWithOutProperStringValueTest()
        //{
        //    try
        //    {
        //        bool expected = messagingService.Cancel("");
        //        Assert.IsTrue(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void CancelScheduleMessageTest()
        //{
        //    string channelName = "testb";
        //    Dictionary<string, string> Headers = new Dictionary<string, string>();
           
        //    Headers.Add("message", "Test message! Hallo Word!!!");
        //    Headers.Add("publishAt", "1571741270000");
        //    Headers.Add("messageId", "1");
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, Headers, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }

        //    string messageId = "1";
        //    try
        //    {
        //        bool expected = messagingService.Cancel(messageId);
        //        Assert.IsTrue(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void CancelScheduleMessageWithWasPublishedTest()
        //{
        //    string channelName = "testb";
        //    Dictionary<string, string> Headers = new Dictionary<string, string>();
        //    Headers.Add("message", "Test message! Hallo Word!!!");
        //    Headers.Add("messageId", "1");
        //    PublishOptions publishOptions = new PublishOptions();
        //    try
        //    {
        //        MessageStatus expected = messagingService.Publish(channelName, Headers, publishOptions);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }

        //    string messageId = "1";
        //    try
        //    {
        //        bool expected = messagingService.Cancel(messageId);
        //        Assert.IsTrue(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        ///// <summary>
        /////A test for Cancel
        /////</summary>
      
      

        ///// <summary>
        /////A test for Subscribe
        /////</summary>
        //[TestMethod()]
        //public void SubscribeWithProperStringValueTest()
        //{
        //    string channelName = "test";
        //    try
        //    {
        //        Messaging.Subscription subscription = messagingService.Subscribe(channelName);
        //        Assert.IsNotNull(subscription);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void SubscribeWithUnknownChannelTest()
        //{
        //    string channelName = "test999";
        //    try
        //    {
        //        Messaging.Subscription subscription = messagingService.Subscribe(channelName);
        //        Assert.IsNotNull(subscription);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        //[TestMethod()]
        //public void PollMessagesWithProperStringValueTest()
        //{
           
        //    string channelName = string.Empty; 
        //    string subscriptionId = string.Empty;
        //    try
        //    {
        //        List<Message> expected = messagingService.PollMessages(channelName, subscriptionId);
        //        Assert.IsNotNull(expected);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }  
        //}
    }
}
