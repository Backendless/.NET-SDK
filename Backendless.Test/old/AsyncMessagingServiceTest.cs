using BackendlessAPI;
using BackendlessAPI.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI.Property;
using System.Collections.Generic;
using BackendlessAPI.Messaging;

namespace BackendlessAPI.Test
{
     
    
    /// <summary>
    ///This is a test class for MessagingServiceMessagFileTest and is intended
    ///to contain all MessagingServiceMessagFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AsyncMessagingServiceMessagFileTest
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
        //[TestMethod()]
        //public void publishSimpleMessageTest()
        //{
        //    AsyncCallback<MessageStatus> callback = new AsyncCallback<MessageStatus>(
        //    r =>
        //    {
        //        Assert.IsNotNull(r);
        //    },
        //    f =>
        //    {
        //        Assert.Fail(f.Message);
        //    });
        //    messagingService.Publish("testChannel", "Test message! Hello Word!!!",callback);
        //}

        //[TestMethod()]
        //public void publishMessageWithHeadersNoSubtopicTest()
        //{
        //    try
        //    {
        //        string channel = "testChannel";
        //        object message = "Test message! Hello Word!!!";
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        dict.Add("City", "Kiev");
        //        dict.Add("Temp", "22");
        //        PublishOptions option = new PublishOptions
        //        {
        //            PublisherId = "testPablisher",
        //            Headers = dict
        //        };
        //        var m = messagingService.Publish(channel, message, option);
        //        Assert.IsNotNull(m.MessageId);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}

        //[TestMethod()]
        //public void publishMessageToSubtopicWithHeadersTest()
        //{
        //    try
        //    {
        //        string channel = "testChannel";
        //        object message = "Test message! Hello Word!!!";
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        dict.Add("City", "Kiev");
        //        dict.Add("Temp", "22");
        //        PublishOptions option = new PublishOptions
        //        {
        //            PublisherId = "testPablisher",
        //            Headers = dict,
        //            Subtopic = "testSubtopic"
        //        };
        //        var m = messagingService.Publish(channel, message, option);
        //        Assert.IsNotNull(m.MessageId);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        //[TestMethod()]
        //public void publishMessagePrimitiveValueTest()
        //{
        //    try
        //    {
        //        string channel = "testChannel";
        //        object message = 12345;
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        dict.Add("City", "Kiev");
        //        dict.Add("Temp", "22");
        //        PublishOptions option = new PublishOptions
        //        {
        //            PublisherId = "testPablisher",
        //            Headers = dict,
        //            Subtopic = "testSubtopic"
        //        };
        //        var m = messagingService.Publish(channel, message, option);
        //        Assert.IsNotNull(m.MessageId);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        //[TestMethod()]
        //public void publishMessageStringValueTest()
        //{
        //    try
        //    {
        //        string channel = "testChannel";
        //        object message = "Test message! Hello Word!!!";
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        dict.Add("City", "Kiev");
        //        dict.Add("Temp", "22");
        //        PublishOptions option = new PublishOptions
        //        {
        //            PublisherId = "testPablisher",
        //            Headers = dict,
        //            Subtopic = "testSubtopic"
        //        };
        //        var m = messagingService.Publish(channel, message, option);
        //        Assert.IsNotNull(m.MessageId);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        //[TestMethod()]
        //public void publishMessageDateValueTest()
        //{
        //    try
        //    {
        //        string channel = "testChannel";
        //        object message = DateTime.Now;
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        dict.Add("City", "Kiev");
        //        dict.Add("Temp", "22");
        //        PublishOptions option = new PublishOptions
        //        {
        //            PublisherId = "testPablisher",
        //            Headers = dict,
        //            Subtopic = "testSubtopic"
        //        };
        //        var m = messagingService.Publish(channel, message, option);
        //        Assert.IsNotNull(m.MessageId);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        //[TestMethod()]
        //public void publishMessageBoolValueTest()
        //{
        //    try
        //    {
        //        string channel = "testChannel";
        //        object message = true;
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        dict.Add("City", "Kiev");
        //        dict.Add("Temp", "22");
        //        PublishOptions option = new PublishOptions
        //        {
        //            PublisherId = "testPablisher",
        //            Headers = dict,
        //            Subtopic = "testSubtopic"
        //        };
        //        var m = messagingService.Publish(channel, message, option);
        //        Assert.IsNotNull(m.MessageId);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        //[TestMethod()]
        //public void publishMessageObjectValueTest()
        //{
        //    try
        //    {
        //        string channel = "testChannel";
        //        object message = new Person { Name = "Vasya", Age = 22 };
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        dict.Add("City", "Kiev");
        //        dict.Add("Temp", "22");
        //        PublishOptions option = new PublishOptions
        //        {
        //            PublisherId = "testPablisher",
        //            Headers = dict,
        //            Subtopic = "testSubtopic"
        //        };
        //        var m = messagingService.Publish(channel, message, option);
        //        Assert.IsNotNull(m.MessageId);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        //[TestMethod()]
        //public void publishMessageArrayValueTest()
        //{
        //    try
        //    {
        //        string channel = "testChannel";
        //        object message = new List<Person> { new Person { Name = "Vasya", Age = 22 } };
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        dict.Add("City", "Kiev");
        //        dict.Add("Temp", "22");
        //        PublishOptions option = new PublishOptions
        //        {
        //            PublisherId = "testPablisher",
        //            Headers = dict,
        //            Subtopic = "testSubtopic"
        //        };
        //        var m = messagingService.Publish(channel, message, option);
        //        Assert.IsNotNull(m.MessageId);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        //[TestMethod()]
        //public void getRegistrationsTest()
        //{
        //    try
        //    {
        //        List<DeviceRegistration> list = messagingService.GetRegistrations();
        //        Assert.IsNotNull(list);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        //[TestMethod()]
        //public void getRegistrationByIdTest()
        //{
        //    try
        //    {
        //        DeviceRegistration device = messagingService.GetRegistrationById(messagingService.DeviceRegistrationId);
        //        Assert.IsNotNull(device);
        //    }
        //    catch (BackendlessException ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}





        //[TestMethod()]
        //public void RegisterDeviceTest()
        //{
        //    try
        //    {
        //        string device = messagingService.RegisterDevice();
        //        Assert.IsNotNull(device);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }

        //}
        // [TestMethod()]
        //public void registerDeviceWithExpirationTest()
        //{
        //    try
        //    {
        //        DateTime expiration = DateTime.Now.AddDays(1);
        //        string registrationId = messagingService.RegisterDevice(expiration);
        //        Assert.IsNotNull(registrationId);
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        // [TestMethod()]
        // public void registerDeviceWithWrongExpirationTest()
        // {
        //     try
        //     {
        //         DateTime expiration = DateTime.Now.AddDays(-1);
        //         string registrationId = messagingService.RegisterDevice(expiration);
        //         Assert.IsNotNull(registrationId);
        //     }
        //     catch (BackendlessException ex)
        //     {
        //         Assert.Fail(ex.Message);
        //     }
        // }
        // [TestMethod()]
        // public void registerDeviceWithMultipleChannelsTest()
        // {
        //     try
        //     {
        //         DateTime expiration = DateTime.Now.AddDays(1);
        //         List<string> channels = new List<string> { "channel1", "channel2" };
        //         string registrationId = messagingService.RegisterDevice(channels,expiration);
        //         Assert.IsNotNull(registrationId);
        //     }
        //     catch (BackendlessException ex)
        //     {
        //         Assert.Fail(ex.Message);
        //     }
        // }


        // [TestMethod()]
        // public void cancelDeviceRegistrationTest()
        // {
        //     try
        //     {
        //         var result = messagingService.UnregisterDevice();
        //         Assert.IsTrue(result);
        //     }
        //     catch (BackendlessException ex)
        //     {
        //         Assert.Fail(ex.Message);
        //     }
        // }



    }
}
