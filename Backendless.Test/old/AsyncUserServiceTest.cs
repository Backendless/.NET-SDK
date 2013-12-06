using BackendlessAPI;
using BackendlessAPI.Async;
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
    public class AsyncUserServiceTest
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
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
                Assert.IsNotNull(u.UserId, "Server returned a null id");
                Assert.AreEqual(login, u.Properties["login"], "Server returned wrong user login");
                Assert.AreEqual("test@hotmail.com", u.Properties["email"], "Server returned wrong user email");
                Assert.AreEqual("111222", u.Password, "Server returned wrong user password");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            Backendless.UserService.Register(user, callback);
        }
        [TestMethod()]
        public void registerWithoutBodyTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>
            (
                u => { Assert.IsNotNull(u, "Server returned a null result"); },
                f => { Assert.Fail(f.Message); }
            );

            try
            {
                Backendless.UserService.Register(null, callback);
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
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>
                (
                    u => { Assert.IsNotNull(u, "Server returned a null result"); },
                    f => { Assert.Fail(f.Message); }
                );
            Backendless.UserService.Register(new BackendlessUser(), callback);
        }
        [TestMethod()]
        public void registerUserWithoutPasswordTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
                Assert.IsNotNull(u.UserId, "Server returned a null id");
                Assert.AreEqual("test@hotmail.com", u.Properties["email"], "Server returned wrong user email");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            Backendless.UserService.Register(user, callback);
        }
        [TestMethod()]
        public void registerUserWithoutEmailTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
             u =>
             {
                 Assert.IsNotNull(u, "Server returned a null result");
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });
            
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("login", login);
            user.Password = "111222";
            Backendless.UserService.Register(user, callback);
        }
        [TestMethod()]
        public void registerCopyUserWithIdentityTest()
        {
            string login = "test" + random.Next();
            AsyncCallback<BackendlessUser> callback1 = new AsyncCallback<BackendlessUser>(
             user =>
             {
                
                 Assert.IsNotNull(user, "Server returned a null result");
             },
             fault =>
             {
                 Assert.Fail(fault.Message);
             });
            AsyncCallback<BackendlessUser> callback2 = new AsyncCallback<BackendlessUser>(
             user =>
             {
                 BackendlessUser u2 = new BackendlessUser { };
                 u2.SetProperty("email", "test111@hotmail.com");
                 u2.SetProperty("login", login);
                 u2.Password = "11122211";
                 Backendless.UserService.Register(u2, callback1);
             },
             fault =>
             {
                 Assert.Fail(fault.Message);
             });
           
            BackendlessUser u = new BackendlessUser { };
            u.SetProperty("email", "test@hotmail.com");
            u.SetProperty("login", login);
            u.Password = "111222";
            Backendless.UserService.Register(u,callback2);

        }
        [TestMethod()]
        public void registerWithProperStringValueAndIdTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.SetProperty("id", "12345");
            user.Password = "111222";
            Backendless.UserService.Register(user,callback);
        }
        [TestMethod()]
        public void registerUserForDisabledDynamicPropertiesTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.SetProperty("dynamic_property", "1111");
            user.Password = "111222";
            Backendless.UserService.Register(user,callback);
        }
        [TestMethod()]
        public void registerWithoutLoginAsIdentityTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.Password = "111222";
            Backendless.UserService.Register(user,callback);
        }
        [TestMethod()]
        public void registerWithWrongArgumentsCountTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", "test001");
            user.SetProperty("login2", "test002");
            user.SetProperty("password2", "111111");
            user.Password = "111222";
            Backendless.UserService.Register(user,callback);
        }
        [TestMethod()]
        public void registerWithNullEmailTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            string login = "test" + random.Next();
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", null);
            user.SetProperty("login", login);
            user.Password = "111222";
            Backendless.UserService.Register(user,callback);
        }

        [TestMethod()]
        public void registerWithIntValueAndWrongEmailTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test.com");
            user.SetProperty("login", random.Next());
            user.Password = "111222";
            Backendless.UserService.Register(user,callback);
        }
        [TestMethod()]
        public void registerWithIntValueTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", 1234);
            user.Password = "111222";
            Backendless.UserService.Register(user,callback);
        }
        [TestMethod()]
        public void registerWithBooleanValueTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", true);
            user.SetProperty("login", true);
            user.SetProperty("password", "test");
            try
            {
                Backendless.UserService.Register(user, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("3040", ex.Code);
            }
        }
        [TestMethod()]
        public void registerWithNullValueTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", null);
            user.SetProperty("login", null);
            user.SetProperty("password", null);
            Backendless.UserService.Register(user,callback);
        }
        /// <summary>
        ///A test for Login
        ///</summary>
        [TestMethod()]
        public void loginWithProperStringValueTest()
        {
            AsyncCallback<BackendlessUser> logincallback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            AsyncCallback<BackendlessUser> registercallback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Backendless.UserService.Login(u.Properties["login"].ToString(), u.Password, logincallback);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", "test"+random.Next());
            user.Password = "111222";
            Backendless.UserService.Register(user,registercallback);
        }

        /// <summary>
        ///A test for Login
        ///</summary>
        [TestMethod()]
        public void loginWithoutBodyTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });

            try
            {
                Backendless.UserService.Login(null, null, callback);
            }
            catch(BackendlessException ex)
            {
                //104:User login can not be null or empty.
                Assert.AreEqual("N/A", ex.Code);
            }

        }
        [TestMethod()]
        public void loginUserWithoutLoginValueTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });

            try
            {
                Backendless.UserService.Login(null, "111222", callback);
            }
            catch (BackendlessException ex)
            {
                //104:User login can not be null or empty.
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void loginUserWithNullPasswordTest()
        {
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.IsNotNull(u, "Server returned a null result");
            },
            f =>
            {
                Assert.Fail(f.Message);
            });

            try 
            {
                Backendless.UserService.Login("test001", null, callback);
            } 
            catch (BackendlessException ex)
            {
                //103:User password can not be null or empty.
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void loginUserWithInvalidUserIdTest()
        {
            string login = "test" + random.Next();
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.AreEqual(UserService.CurrentUser, login);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            AsyncCallback<BackendlessUser> regCallback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Backendless.UserService.Login(login+"test", "111222", callback);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            Backendless.UserService.Register(user, regCallback);
        }

        [TestMethod()]
        public void loginUserWithInvalidPasswordTest()
        {
            string login = "test" + random.Next();
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.AreEqual(UserService.CurrentUser, login);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            AsyncCallback<BackendlessUser> regCallback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Backendless.UserService.Login(login, "1111", callback);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            Backendless.UserService.Register(user, regCallback);
        }
        [TestMethod()]
        public void loginUserWithInvalidPasswordAndLoginTest()
        {
            string login = "test" + random.Next();
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.AreEqual(UserService.CurrentUser, login);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            AsyncCallback<BackendlessUser> regCallback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Backendless.UserService.Login(login+"test", "12", callback);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            Backendless.UserService.Register(user, regCallback);
        }



        [TestMethod()]
        public void logoutUserTest()
        {
            AsyncCallback<object> callback = new AsyncCallback<object>(
            u =>
            {
                Assert.IsNotNull(u);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            Backendless.UserService.Logout(callback);
        }
        [TestMethod()]
        public void getCurrentUserTest()
        {
            string login = "test" + random.Next();
            AsyncCallback<BackendlessUser> callback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Assert.AreEqual(UserService.CurrentUser, login);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            AsyncCallback<BackendlessUser> regCallback = new AsyncCallback<BackendlessUser>(
            u =>
            {
                Backendless.UserService.Login(login, "111222",callback);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            BackendlessUser user = new BackendlessUser { };
            user.SetProperty("email", "test@hotmail.com");
            user.SetProperty("login", login);
            user.Password = "111222";
            Backendless.UserService.Register(user,regCallback);
            

        }
        [TestMethod()]
        public void CurrentUserTest()
        {
            AsyncCallback<object> callback = new AsyncCallback<object>(
            u =>
            {
                Assert.IsNull(UserService.CurrentUser);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            Backendless.UserService.Logout(callback);
        }

        [TestMethod()]
        public void restorePasswordTest()
        {
            try
            {
                AsyncCallback<object> callback = new AsyncCallback<object>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                string login = "test" + random.Next();
                BackendlessUser user = new BackendlessUser();
                user.SetProperty("email", "605029159@qq.com");
                user.SetProperty("login", login);
                user.Password = "111222";
                Backendless.UserService.Register(user);
                Backendless.UserService.RestorePassword(login, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void restorePasswordForWrongLoginTest()
        {
            AsyncCallback<object> callback = new AsyncCallback<object>(
            u =>
            {
                Assert.IsNotNull(u);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            Backendless.UserService.RestorePassword("test",callback);
        }

        [TestMethod()]
        public void DescribeUserClassTest()
        {
            AsyncCallback<List<UserProperty>> callback = new AsyncCallback<List<UserProperty>>(
            u =>
            {
                Assert.IsNotNull(u);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            Backendless.UserService.DescribeUserClass(callback);
        }

    }
}
