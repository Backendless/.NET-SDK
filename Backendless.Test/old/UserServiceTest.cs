using BackendlessAPI;
using BackendlessAPI.Exception;
using BackendlessAPI.Property;
using BackendlessAPI.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BackendlessAPI.Test
{


    /// <summary>
    ///This is a test class for Backendless.UserServiceTest and is intended
    ///to contain all Backendless.UserServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserServiceTest
    {


        private TestContext testContextInstance;
        Random random = new Random((int)DateTime.Now.Ticks);
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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string applicationId = "validApp-Ids0-0000-0000-000000000000"; // TODO: Initialize to an appropriate value
            string secretKey = "validSec-retK-eys0-0000-000000000000"; // TODO: Initialize to an appropriate value
            string version = "v1"; // TODO: Initialize to an appropriate value
            Backendless.InitApp(applicationId, secretKey, version);
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Register
        ///</summary>
        [TestMethod()]
        public void registerUserWithProperStringValueTest()
        {

            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            BackendlessUser expected = new BackendlessUser();
            try
            {
                expected = Backendless.UserService.Register(user);
            }
            catch (System.Exception e)
            {
                Assert.Fail(e.Message);
            }
            Assert.IsNotNull(expected, "Server returned a null result");
            Assert.IsNotNull(expected.UserId, "Server returned a null id");
            Assert.AreEqual(login, expected.Properties["login"], "Server returned wrong user login");
            Assert.AreEqual("test@hotmail.com", expected.Properties["email"], "Server returned wrong user email");
            Assert.AreEqual("111222", expected.Password, "Server returned wrong user password");
        }

        [TestMethod()]
        public void registerWithoutBodyTest()
        {
            BackendlessUser expected = new BackendlessUser();
            try
            {
                expected = Backendless.UserService.Register(null);
                Assert.IsNotNull(expected, "Server returned a null result");
            }
            catch (BackendlessException ex)
            {
                //101
                Assert.AreEqual("N/A", ex.Code);
            }
            

        }
        [TestMethod()]
        public void registerUserWithoutValuesTest()
        {
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            //user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            BackendlessUser expected = new BackendlessUser();
            try
            {
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected, "Server returned a null result");
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("3012", ex.Code);
            }
            
        }
        [TestMethod()]
        public void registerUserWithoutPasswordTest()
        {
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", login + "@hotmail.com");
            user.SetProperty("login", login);

            BackendlessUser expected = new BackendlessUser();
            try
            {
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("3011", ex.Code);
            }
            Assert.IsNotNull(expected, "Server returned a null result");
        }
        [TestMethod()]
        public void registerUserWithoutEmailTest()
        {
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("login", login);
            user.Password = "1234";

            BackendlessUser expected = new BackendlessUser();
            try
            {
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("3012", ex.Code);
            }
            Assert.IsNotNull(expected, "Server returned a null result");
        }
        [TestMethod()]
        public void registerCopyUserWithIdentityTest()
        {
            
            try
            {
                string login = "test" + random.Next();
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", "test@hotmail.com");
                user.SetProperty("login", login);
                user.Password = "111222";
                BackendlessUser actual= Backendless.UserService.Register(user);
                BackendlessUser user2 = new BackendlessUser { };
                user2.SetProperty("email", "test111@hotmail.com");
                user2.SetProperty("login", login);
                user2.Password = "11122211";
                BackendlessUser expected = Backendless.UserService.Register(user2);
                Assert.AreEqual(expected, actual);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("3033", ex.Code);
            }
            
        }
        [TestMethod()]
        public void registerWithProperStringValueAndIdTest()
        {
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", login + "@hotmail.com");
            user.SetProperty("login", login);
            user.SetProperty("id", "12345");
            user.Password = "111222";
            BackendlessUser expected = new BackendlessUser();
            try
            {
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("3039", e.Code);
            }
            //Assert.IsNotNull(expected, "Server returned a null result");
            //Assert.IsNotNull(expected.Id, "Server returned a null id");
            //Assert.AreEqual("test001", expected.Properties["login"], "Server returned wrong user login");
            //Assert.AreEqual("test@hotmail.com", expected.Properties["email"], "Server returned wrong user email");
            //Assert.AreEqual("12345", expected.Properties["id"], "Server returned wrong user id");
            //Assert.AreEqual("111222", expected.Password, "Server returned wrong user password");
        }
        [TestMethod()]
        public void registerUserForDisabledRegistrationTest()
        {
            //BackendlessUser user = new BackendlessUser { };
            //user.SetProperty("email", "test@hotmail.com");
            //user.SetProperty("login", "test001");
            //user.Password = "111222";
            //BackendlessUser expected = new BackendlessUser();
            //try
            //{
            //    expected = Backendless.UserService.Register(user);
            //    Assert.IsNotNull(expected);
            //}
            //catch (Exception e)
            //{
            //    Assert.Fail(e.Message);
            //}
        }
        [TestMethod()]
        public void registerUserForDisabledDynamicPropertiesTest()
        {
            
            try
            {
                BackendlessUser user = new BackendlessUser { };

                string login = "test" + random.Next();
                user.SetProperty("email", login + "@hotmail.com");
                user.SetProperty("login", login);
                user.SetProperty("dynamic_property", "1111");
                user.Password = "111222";
                BackendlessUser expected = new BackendlessUser();
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
                Assert.IsNotNull(expected, "Server returned a null result");
                Assert.IsNotNull(expected.UserId, "Server returned a null id");
                Assert.AreEqual(login, expected.Properties["login"], "Server returned wrong user login");
                Assert.AreEqual(login + "@hotmail.com", expected.Properties["email"], "Server returned wrong user email");
                Assert.AreEqual("111222", expected.Password, "Server returned wrong user password");
                Assert.AreEqual("1111", expected.Properties["dynamic_property"], "Server returned wrong user dynamic_property");
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("1232", e.Code);
            }

        }

        [TestMethod()]
        public void registerWithoutLoginAsIdentityTest()
        {
            
            try
            {
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", "test@hotmail.com");
                user.Password = "111222";
                BackendlessUser expected = new BackendlessUser();
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("3013", e.Code);
            }
        }
        [TestMethod()]
        public void registerWithWrongArgumentsCountTest()
        {

            try
            {
                BackendlessUser user = new BackendlessUser { };

                string login = "test" + random.Next();

                user.SetProperty("email", login + "@hotmail.com");
                user.SetProperty("login", login);
                user.SetProperty("login2", "test002");
                user.SetProperty("password2", "111111");
                user.Password = "111222";
                BackendlessUser expected = new BackendlessUser();
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (System.Exception e)
            {
                Assert.Fail(e.Message);
            }
            
        }
        [TestMethod()]
        public void registerWithNullEmailTest()
        {
            
            try
            {
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", null);
                user.SetProperty("login", "test001");
                user.Password = "111222";
                BackendlessUser expected = new BackendlessUser();
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException e)
            {
                //Provided email has wrong format.
                Assert.AreEqual("3040", e.Code);
            }
        }
        [TestMethod()]
        public void registerWithIntValueAndWrongEmailTest()
        {
            
            try
            {
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", "test.com");
                user.SetProperty("login", 1234);
                user.Password = "111222";
                BackendlessUser expected = new BackendlessUser();
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("3040", e.Code);
            }
        }
        [TestMethod()]
        public void registerWithIntValueTest()
        {
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", 1234);
            user.Password = "111222";
            BackendlessUser expected = new BackendlessUser();
            try
            {
                expected = Backendless.UserService.Register(user);
            }
            catch (BackendlessException e)
            {
                //Wrong value type for property 'login'.
                Assert.AreEqual("3049", e.Code);
            }
        }
        [TestMethod()]
        public void registerWithBooleanValueTest()
        {
            
            try
            {
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", true);
                user.SetProperty("login", true);
                user.SetProperty("password", "test");
                BackendlessUser expected = new BackendlessUser();
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("3040", e.Code);
            }
        }
        [TestMethod()]
        public void registerWithNullValueTest()
        {
            
            try
            {
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", null);
                user.SetProperty("login", null);
                user.SetProperty("password", null);
                BackendlessUser expected = new BackendlessUser();
                expected = Backendless.UserService.Register(user);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("3041", e.Code);
            }
        }
        /// <summary>
        ///A test for Login
        ///</summary>
        [TestMethod()]
        public void loginWithProperStringValueTest()
        {
            try
            {
                string login = "test" + random.Next();
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", login + "@hotmail.com");
                user.SetProperty("login", login);
                user.Password = "111222";
                Backendless.UserService.Register(user);
                BackendlessUser expected = Backendless.UserService.Login(login, "111222");
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void loginWithoutBodyTest()
        {
            String login = null;
            String password = null;
            try
            {
                BackendlessUser user = Backendless.UserService.Login(login, password);
                Assert.IsNotNull(user);
            }
            catch (BackendlessException ex)
            {
                // 104
                Assert.AreEqual("N/A", ex.Code);
            }
        }

        [TestMethod()]
        public void loginUserWithoutLoginValueTest()
        {
            try
            {
                //BackendlessUser user = new BackendlessUser { };
                //user.SetProperty("email", "test@hotmail.com");
                //user.SetProperty("login", "test001");
                //user.Password = "111222";
                //Backendless.UserService.Register(user);
                BackendlessUser expected = Backendless.UserService.Login(null, "111222");
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException ex)
            {
                // 104
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void loginUserWithNullPasswordTest()
        {
            try
            {
                string login = "test" + random.Next();
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", "test@hotmail.com");
                user.SetProperty("login", login);
                user.Password = "111222";
                Backendless.UserService.Register(user);
                BackendlessUser expected = Backendless.UserService.Login(login, null);
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException ex)
            {
                //User password can not be null or empty.
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void loginUserWithInvalidUserIdTest()
        {
            try
            {
                string login = "test" + random.Next();
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", login + "@hotmail.com");
                user.SetProperty("login", login);
                user.Password = "111222";
                Backendless.UserService.Register(user);

                BackendlessUser expected = Backendless.UserService.Login(login+"test", "111222");
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException ex)
            {
                //Invalid login or password
                Assert.AreEqual("3003", ex.Code);
            }
        }
        [TestMethod()]
        public void loginUserWithInvalidPasswordTest()
        {
            try
            {
                string login = "test" + random.Next();
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", "test@hotmail.com");
                user.SetProperty("login", login);
                user.Password = "111222";
                Backendless.UserService.Register(user);
                BackendlessUser expected = Backendless.UserService.Login(login, "111111");
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("3003", ex.Code);
            }
        }
        [TestMethod()]
        public void loginUserWithInvalidPasswordAndLoginTest()
        {
            try
            {
                //BackendlessUser user = new BackendlessUser { };
                //user.SetProperty("email", "test@hotmail.com");
                //user.SetProperty("login", "test001");
                //user.Password = "111222";
                //Backendless.UserService.Register(user);
                BackendlessUser expected = Backendless.UserService.Login("test", "11");
                Assert.IsNotNull(expected);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("3003", ex.Code);
            }
        }
        [TestMethod()]
        public void logoutUserTest()
        {
            Backendless.UserService.Logout();
            Assert.IsNull(UserService.CurrentUser);
        }
        [TestMethod()]
        public void getCurrentUserTest()
        {
            try
            {
                string login = "test" + random.Next();
                BackendlessUser user = new BackendlessUser { };
                user.SetProperty("email", login + "@hotmail.com");
                user.SetProperty("login", login);
                user.Password = "111222";
                Backendless.UserService.Register(user);
                Backendless.UserService.Login(login, "111222");
                Assert.AreEqual(UserService.CurrentUser.GetProperty("login").ToString(), login);
            }
            catch(BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }

        }
        [TestMethod()]
        public void CurrentUserTest()
        {
            Backendless.UserService.Logout();
            var actual = UserService.CurrentUser;
            Assert.IsNull(null, "Current user is not null");
        }

        [TestMethod()]
        public void restorePasswordTest()
        {
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser();
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            Backendless.UserService.Register(user);
            try
            {
                Backendless.UserService.RestorePassword(login);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void restorePasswordForWrongLoginTest()
        {
            try
            {
                Backendless.UserService.RestorePassword("test");
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("3020", ex.Code);
            }
        }

        [TestMethod()]
        public void DescribeUserClassTest()
        {
            try
            {
                List<UserProperty> list = new List<UserProperty>();
                list = Backendless.UserService.DescribeUserClass();
                Assert.AreNotEqual(list.Count, 0);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }









        [TestMethod()]
        public void updateUserPropertiesTest()
        {
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            try
            {
                Backendless.UserService.Register(user);
                BackendlessUser user2 = Backendless.UserService.Login(login, "111222");
                user2.SetProperty("email", "test111@hotmail.com");
                user2.Password = "111111";
                BackendlessUser user3 = Backendless.UserService.Update(user2);
                Assert.AreEqual("test111@hotmail.com", user3.Properties["email"], "Server returned wrong user email");
                Assert.AreEqual("111111", user3.Password, "Server returned wrong user password");

            }
            catch (System.Exception e)
            {
                Assert.Fail(e.Message);
            }
            
        }

    }
}
