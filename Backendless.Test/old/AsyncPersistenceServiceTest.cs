using BackendlessAPI;
using BackendlessAPI.Property;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BackendlessAPI.Test
{
    
    
    /// <summary>
    ///This is a test class for PersistenceServicePersistenceServiceTest and is intended
    ///to contain all PersistenceServicePersistenceServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AsyncPersistenceServiceTest
    {


        /*private TestContext testContextInstance;
        Random random = new Random((int)DateTime.Now.Ticks);
        PersistenceService persistenceService = new PersistenceService();

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
        public void saveNewPropertiesIntoPersonTableTest()
        {
            try
            {
                AsyncCallback<Person> callback = new AsyncCallback<Person>(
                p =>
                {
                    Assert.AreEqual("Bob", p.Name);
                    Assert.AreEqual(21, p.Age);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Person person = new Person { Name = "Bob", Age = 21 };
                persistenceService.Save(person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewUserMyTableTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual("Bob", p["Name"].ToString());
                    Assert.AreEqual("21", p["Age"].ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", "Bob");
                person.Add("Age", 21);
                persistenceService.Save("userMy", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectWithoutBodyTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                persistenceService.Save("Person", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectIntoUsersTableTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual("Bob", p["Name"].ToString());
                    Assert.AreEqual("21", p["Age"].ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", "Bob");
                person.Add("Age", 21);
                persistenceService.Save("Users", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectIntoUserTableTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual("Bob", p["Name"].ToString());
                    Assert.AreEqual("21", p["Age"].ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", "Bob");
                person.Add("Age", 21);
                persistenceService.Save("User", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectIntoUser1TableWithDifferentCaseTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("gender", 200000);
                person.Add("GENDER", true);
                persistenceService.Save("User1", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectWithObjectIdPropertyTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual("test", p["objectId"].ToString());
                    Assert.AreEqual("Bob", p["Name"].ToString());
                    Assert.AreEqual("21", p["Age"].ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("objectId", "test");
                person.Add("Name", "Bob");
                person.Add("Age", 21);
                persistenceService.Save("Person", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectsWithStringPropertyTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual("Bob", p["Name"].ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", "Bob");
                persistenceService.Save("Person", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }

        }
        [TestMethod()]
        public void saveNewObjectsWithBooleanPropertyTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual("1", p["Name"].ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", true);
                persistenceService.Save("Person", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectsWithIntPropertyTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual("12345", p["Name"].ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", 12345);
                persistenceService.Save("Person", person);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1007", ex.Code);
            }
        }
        [TestMethod()]
        public void saveNewObjectsWithDoublePropertyTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual(12.12, (double)p["Name"]);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", 12.12);
                persistenceService.Save("Person", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectWithoutNameOfTableTest()
        {
            try
            {
                AsyncCallback<Dictionary<string, object>> callback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    Assert.AreEqual("Bob", p["Name"].ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", "Bob");
                persistenceService.Save("", person, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [TestMethod()]
        public void retrieveObjectByIdTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                p =>
                {
                    Assert.AreEqual("Bob", p.Name);
                    Assert.AreEqual("21", p.Age.ToString());
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                p =>
                {
                    persistenceService.FindById<Person>(entityName, p["objectId"].ToString(), findCallback);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> person = new Dictionary<string, object>();
                person.Add("Name", "Bob");
                person.Add("Age", 21);
                persistenceService.Save(entityName, person, saveCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveObjectWithInvalidObjectIdTest()
        {
            try
            {
                AsyncCallback<Person> callback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.FindById<Person>("Person", "objectId111");
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1000", ex.Code);
            }
        }

        [TestMethod()]
        public void findAllWithoutParametersInPersonTableTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> callback = new AsyncCallback<BackendlessCollection<Person>>(
                p =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                persistenceService.Find<Person>(query, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findAllWithoutParametersInUserTableTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                        Assert.IsNotNull(r);
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                BackendlessDataQuery query = new BackendlessDataQuery();
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPagesizeAndOffsetOptionTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = 1;
                query.PageSize = 2;
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void findWithPropsAndPagesizeAndOffsetOptionInUserTableTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                        Assert.IsNotNull(r);
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = 1;
                query.PageSize = 2;
                query.Properties = new List<string> { "Name", "Age" };
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findTwiceWithPropsAndPagesizeAndOffsetOptionWithSortByInUserTableTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                        Assert.IsNotNull(r);
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }

                AsyncCallback<BackendlessCollection<Person>> findCallback1 = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.QueryOptions = new QueryOptions { Offset = 1, PageSize = 2, SortBy = new List<string> { "Name" }, related = new List<string> { "Name", "Age" } };
                persistenceService.Find<Person>(entityName, query, findCallback1);
                AsyncCallback<BackendlessCollection<Person>> findCallback2 = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                BackendlessDataQuery query2 = new BackendlessDataQuery();
                query2.QueryOptions = new QueryOptions { Offset = 3, PageSize = 3, SortBy = new List<string> { "Name" }, related = new List<string> { "Name", "Age" } };
                persistenceService.Find<Person>(entityName, query2, findCallback2);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithNegativeOffsetOptionTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = -1;
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithOffsetGreaterThanMaxNumberOfRecordsTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                r =>
                {
                    BackendlessDataQuery query = new BackendlessDataQuery();
                    query.Offset = 3;
                    persistenceService.Find<Person>(entityName, query, findCallback);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test");
                dict.Add("Age", 20);
                persistenceService.Save(entityName, dict, saveCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithNegativePageSizeOptionTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.PageSize = -2;
                query.Offset = 1;
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithZeroPageSizeOptionTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                BackendlessDataQuery query = new BackendlessDataQuery();
                query.PageSize = 0;
                query.Offset = 0;
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPageSizeGreaterThan100Test()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.PageSize = 102;
                query.Offset = 1;
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPropertiesTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.PageSize = 15;
                query.Properties = new List<string> { "Name", "Age" };
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPropsInUserTableTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Properties = new List<string> { "Name", "Age" };
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPropertiesWhichDoNotBelongToTheObjectTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Properties = new List<string> { "Name2", "Age2" };
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPropertiesWhichBelongOrNotBelongToTheObjectTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Properties = new List<string> { "ABC", "Name", "Age2" };
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPropertiesWhichBelongOrNotBelongToUserTableTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }

                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = 1;
                query.PageSize = 2;
                query.Properties = new List<string> { "Name", "Age2", "ddd" };
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForIntTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Age > 25";
                query.Properties = new List<string> { "Name", "Age" };
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForStringTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Name LIKE 'test1%'";
                query.Properties = new List<string> { "Name", "Age" };
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForOrTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Name='test1' or Age=28";
                query.Properties = new List<string> { "Name", "Age" };
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForANDORTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict);
                }
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Name='Test1' or (Name ='Test3' and Age=23)";
                query.Properties = new List<string> { "Name", "Age" };
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForBooleanTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Age<>30";
                query.Properties = new List<string> { "Name", "Age" };
                persistenceService.Find<Person>(entityName, query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithSortByConditionTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<Person>> findCallback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.QueryOptions = new QueryOptions { SortBy = new List<string> { "Name" } };
                persistenceService.Find<Person>(query, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveFirstObjectTest()
        {
            try
            {
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.First<Person>(findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveFirstUserObjectTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }

                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                persistenceService.First<Person>(entityName, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveFirstOfUnknownObjectTest()
        {
            try
            {
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.First<Person>("testdddddd", findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveFirstObjectFromUserTableTest()
        {
            try
            {
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                persistenceService.First<Person>("User", findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveFirstObjectFromUsersTableTest()
        {
            try
            {
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                persistenceService.First<Person>("Users", findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveFirstForTableWithoutDataTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                r =>
                {
                    persistenceService.Remove<Person>(entityName, r["objectId"].ToString());
                    persistenceService.First<Person>(entityName, findCallback);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test");
                dict.Add("Age", 20);
                dict.Add("Birthday", DateTime.Now);
                persistenceService.Save(entityName, dict, saveCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveLastObjectTest()
        {
            try
            {
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Last<Person>(findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void retrieveLastUserObjectTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Last<Person>(entityName, findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveLastOfUnknownObjectTest()
        {
            try
            {
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Last<Person>("testdddddd2", findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveLastObjectFromUserTableTest()
        {
            try
            {
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Last<Person>("User", findCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveLastObjectFromUsersTableTest()
        {
            try
            {
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Last<Person>("Users");
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrieveLastForTableWithoutDataTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                AsyncCallback<Person> findCallback = new AsyncCallback<Person>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                r =>
                {
                    persistenceService.Remove<Person>(entityName, r["objectId"].ToString());
                    persistenceService.Last<Person>(entityName, findCallback);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test");
                dict.Add("Age", 20);
                dict.Add("Birthday", DateTime.Now);
                persistenceService.Save(entityName, dict, saveCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfObjectTest()
        {
            try
            {
                AsyncCallback<List<ObjectProperty>> callback = new AsyncCallback<List<ObjectProperty>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Describe("Person", callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfObjectForTheUserTableTest()
        {
            try
            {
                AsyncCallback<List<ObjectProperty>> callback = new AsyncCallback<List<ObjectProperty>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Describe("User", callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfObjectForTheUsersTableTest()
        {
            try
            {
                AsyncCallback<List<ObjectProperty>> callback = new AsyncCallback<List<ObjectProperty>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Describe("Users", callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfUnknownObjectTest()
        {
            try
            {
                AsyncCallback<List<ObjectProperty>> callback = new AsyncCallback<List<ObjectProperty>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                persistenceService.Describe("P123345", callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfUserObjectTest()
        {
            try
            {
                string entityName = "Person" + random.Next();
                AsyncCallback<List<ObjectProperty>> callback = new AsyncCallback<List<ObjectProperty>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                r =>
                {
                    persistenceService.Describe(entityName, callback);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob222");
                dict.Add("Age", 20);
                dict.Add("Birthday", DateTime.Now);
                persistenceService.Save(entityName, dict, saveCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void retrieveNextPageObjectTest()
        {
            
            try
            {
                string entityName = "Person";
                for (int i = 0; i < 15; i++)
                {
                    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                    r =>
                    {
                        Assert.IsNotNull(r);
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict, saveCallback);
                }

                AsyncCallback<BackendlessCollection<Person>> callback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.QueryOptions = new QueryOptions { SortBy = new List<string> { "Name" }, PageSize = 5, Offset = 0 };
                persistenceService.Find<Person>(entityName, query).NextPage(callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void retrieveGetPageObjectTest()
        {
            
            try
            {
                string entityName = "Person";
                //for (int i = 0; i < 15; i++)
                //{
                //    AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                //    r =>
                //    {
                //        Assert.IsNotNull(r);
                //    },
                //    f =>
                //    {
                //        Assert.Fail(f.Message);
                //    });
                //    Dictionary<string, object> dict = new Dictionary<string, object>();
                //    dict.Add("Name", "test" + i);
                //    dict.Add("Age", 20 + i);
                //    persistenceService.Save(entityName, dict, saveCallback);
                //}

                AsyncCallback<BackendlessCollection<Person>> callback = new AsyncCallback<BackendlessCollection<Person>>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.QueryOptions = new QueryOptions { SortBy = new List<string> { "Name" }, PageSize = 5, Offset = 0 };
                persistenceService.Find<Person>(entityName, query).GetPage(0,callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void deletingObjectByIdTest()
        {
            try
            {
                string entityName = "Person" + random.Next();

                AsyncCallback<long> callback = new AsyncCallback<long>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                r =>
                {
                    persistenceService.Remove<Person>(entityName, r["objectId"].ToString(), callback);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob222");
                dict.Add("Age", 20);
                dict.Add("Birthday", DateTime.Now);
                persistenceService.Save(entityName, dict, saveCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void deletingObjectWithInvalidIdTest()
        {
            try
            {
                string entityName = "Person" + random.Next();

                AsyncCallback<long> callback = new AsyncCallback<long>(
                r =>
                {
                    Assert.IsNotNull(r);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
                r =>
                {
                    persistenceService.Remove<Person>(entityName, "ddddddfsfasfs", callback);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob222");
                dict.Add("Age", 20);
                dict.Add("Birthday", DateTime.Now);
                persistenceService.Save(entityName, dict, saveCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }



















        [TestMethod()]
        public void updatingObjectTest()
        {
            string entityName = "Person" + random.Next();
            AsyncCallback<Dictionary<string, object>> updateCallback = new AsyncCallback<Dictionary<string, object>>(
            p =>
            {
                Assert.AreEqual("Bob2", p["Name"].ToString());
                Assert.AreEqual("22", p["Age"].ToString());
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
            p =>
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob2");
                dict.Add("Age", 22);
                dict.Add("objectId", p["objectId"].ToString());
                persistenceService.Update(entityName, dict,updateCallback);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            persistenceService.Save(entityName, person,saveCallback);
        }
         [TestMethod()]
        public void updatingObjectWithWrongObjectIdTest()
        {
            string entityName = "Person" + random.Next();
            AsyncCallback<Dictionary<string, object>> updateCallback = new AsyncCallback<Dictionary<string, object>>(
            p =>
            {
                Assert.AreEqual("Bob2", p["Name"].ToString());
                Assert.AreEqual("22", p["Age"].ToString());
            },
            f =>
            {
                Assert.Fail(f.Message);
            });
            AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
            p =>
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob2");
                dict.Add("Age", 22);
                dict.Add("objectId","123");
                persistenceService.Update(entityName, dict, updateCallback);
            },
            f =>
            {
                Assert.Fail(f.Message);
            });

            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            persistenceService.Save(entityName, person, saveCallback);
         }
         [TestMethod()]
         public void updatingRecordWithoutBodyTest()
         {
             string entityName = "Person" + random.Next();
             AsyncCallback<Dictionary<string, object>> updateCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });
             AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
                 Dictionary<string, object> dict = new Dictionary<string, object>();
                 dict.Add("objectId", p["objectId"].ToString());
                 persistenceService.Update(entityName, dict, updateCallback);
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });

             Dictionary<string, object> person = new Dictionary<string, object>();
             person.Add("Name", "Bob");
             person.Add("Age", 21);
             persistenceService.Save(entityName, person, saveCallback);

         }
         [TestMethod()]
         public void updatingRecordWithNonExistingFieldsTest()
         {
             string entityName = "Person" + random.Next();
             AsyncCallback<Dictionary<string, object>> updateCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
                 Assert.AreEqual("Bob2", p["Name2"].ToString());
                 Assert.AreEqual("22", p["Age2"].ToString());
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });
             AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
                 Dictionary<string, object> dict = new Dictionary<string, object>();
                 dict.Add("Name2", "Bob2");
                 dict.Add("Age2", 22);
                 dict.Add("objectId", p["objectId"].ToString());
                 persistenceService.Update(entityName, dict, updateCallback);
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });

             Dictionary<string, object> person = new Dictionary<string, object>();
             person.Add("Name", "Bob");
             person.Add("Age", 21);
             persistenceService.Save(entityName, person, saveCallback);
         }
         [TestMethod()]
         public void updatingRecordWithInvalidStringValuesForNumericFieldTest()
         {
             string entityName = "Person" + random.Next();
             AsyncCallback<Dictionary<string, object>> updateCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });
             AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
                 Dictionary<string, object> dict = new Dictionary<string, object>();
                 dict.Add("Name", "Bob");
                 dict.Add("Age", "test");
                 dict.Add("Birthday", DateTime.Now);
                 dict.Add("objectId", p["objectId"].ToString());
                 persistenceService.Update(entityName, dict,updateCallback);
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });

             Dictionary<string, object> person = new Dictionary<string, object>();
             person.Add("Name", "Bob");
             person.Add("Age", 21);
             person.Add("Birthday", DateTime.Now);
             persistenceService.Save(entityName, person,saveCallback);

         }
         [TestMethod()]
         public void updatingRecordWithInvalidNumericValuesForStringFieldTest()
         {
             string entityName = "Person" + random.Next();
             AsyncCallback<Dictionary<string, object>> updateCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });
             AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
                 Dictionary<string, object> dict = new Dictionary<string, object>();
                 dict.Add("Name", 222);
                 dict.Add("Age", 22);
                 dict.Add("Birthday", DateTime.Now);
                 dict.Add("objectId", p["objectId"].ToString());
                 persistenceService.Update(entityName, dict, updateCallback);
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });

             Dictionary<string, object> person = new Dictionary<string, object>();
             person.Add("Name", "Bob");
             person.Add("Age", 21);
             person.Add("Birthday", DateTime.Now);
             persistenceService.Save(entityName, person, saveCallback);
         }
         [TestMethod()]
         public void updatingRecordWithInvalidStringValuesForDateFieldTest()
         {
             string entityName = "Person" + random.Next();
             AsyncCallback<Dictionary<string, object>> updateCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });
             AsyncCallback<Dictionary<string, object>> saveCallback = new AsyncCallback<Dictionary<string, object>>(
             p =>
             {
                 Dictionary<string, object> dict = new Dictionary<string, object>();
                 dict.Add("Name", "Bob2");
                 dict.Add("Age", 22);
                 dict.Add("Birthday", "test");
                 dict.Add("objectId", p["objectId"].ToString());
                 persistenceService.Update(entityName, dict, updateCallback);
             },
             f =>
             {
                 Assert.Fail(f.Message);
             });

             Dictionary<string, object> person = new Dictionary<string, object>();
             person.Add("Name", "Bob");
             person.Add("Age", 21);
             person.Add("Birthday", DateTime.Now);
             persistenceService.Save(entityName, person, saveCallback);
         }*/
    }
}
