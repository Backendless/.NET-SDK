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
    public class PersistenceServiceTest
    {


        /*private TestContext testContextInstance;
        Random random = new Random((int)DateTime.Now.Ticks);
        PersistenceService persistenceService = new PersistenceService();
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        ///
        PersistenceService target = new PersistenceService();
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
                Person person = new Person { Name = "Bob" };
                var p = persistenceService.Save(person);
                Assert.AreEqual("Bob", p.Name);
                //Assert.AreEqual(21, p.Age);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void saveNewUserMyTableTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            try
            {
                var person1 = persistenceService.Save("userMy", person);
                Assert.AreEqual("Bob", person1["Name"].ToString());
                Assert.AreEqual("21", person1["Age"].ToString());
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
                Dictionary<string, object> person = new Dictionary<string, object>();
                var person1 = persistenceService.Save("MyPerson", person);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1001", ex.Code);
            }
        }
        [TestMethod()]
        public void saveNewObjectIntoUsersTableTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            try
            {
                var person1 = persistenceService.Save("Users", person);
                Assert.AreEqual("Bob", person1["Name"].ToString());
                Assert.AreEqual("21", person1["Age"].ToString());
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1002", ex.Code);
            }
        }
        [TestMethod()]
        public void saveNewObjectIntoUserTableTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            try
            {
                var person1 = persistenceService.Save("User", person);
                Assert.AreEqual("Bob", person1["Name"].ToString());
                Assert.AreEqual("Age", person1["Age"].ToString());
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1002", ex.Code); 
            }
        }
        [TestMethod()]
        public void saveNewObjectIntoUser1TableWithDifferentCaseTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("gender", 200000);
            person.Add("GENDER", true);
            try
            {
                var person1 = persistenceService.Save("User1", person);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("8001", ex.Code); 
            }
        }
        [TestMethod()]
        public void saveNewObjectWithObjectIdPropertyTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("ObjectId", "test");
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            try
            {
                var person1 = persistenceService.Save("Person", person);
                Assert.AreEqual("Bob", person1["Name"].ToString());
                Assert.AreEqual("21", person1["Age"].ToString());
                Assert.AreEqual("test", person1["ObjectId"].ToString());
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1003", ex.Code); 
            }
        }
        [TestMethod()]
        public void saveNewObjectsWithStringPropertyTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            string entity = "Person" + random.Next();
            try
            {
                var person1 = persistenceService.Save(entity, person);
                Assert.AreEqual("Bob", person1["Name"].ToString());
            }
            catch (BackendlessException ex)
            {
              //  Assert.AreEqual("1007", ex.Code);
                Assert.Fail(ex.Message); 
            }
        }
        [TestMethod()]
        public void saveNewObjectsWithBooleanPropertyTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", true);
            string entity = "Person" + random.Next();
            try
            {
                var person1 = persistenceService.Save(entity, person);
                Assert.AreEqual(true, person1["Name"]); 
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message); 
            }
        }
        [TestMethod()]
        public void saveNewObjectsWithIntPropertyTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", 12345);
            string entity = "Person" + random.Next();
            try
            {
                var person1 = persistenceService.Save(entity, person);
                Assert.AreEqual("12345", person1["Name"].ToString()); 
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void saveNewObjectsWithDoublePropertyTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", 12.12);
            string entity = "Person" + random.Next();
            try
            {
                var person1 = persistenceService.Save(entity, person);
                Assert.AreEqual("12.12", person1["Name"].ToString()); 
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
               
            }
        }
        [TestMethod()]
        public void saveNewObjectWithoutNameOfTableTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            string entity = "";
            try
            {
                var person1 = persistenceService.Save(entity, person);
                Assert.AreEqual("Bob", person1["Name"].ToString()); 
                //var person1 = persistenceService.Save("", person);
            }
            catch (BackendlessException ex)
            {
              //  Assert.Fail(ex.Message);
                Assert.AreEqual("2004", ex.Code); 
            }
        }






        [TestMethod()]
        public void retrieveObjectByIdTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            string entityName = "Person" + random.Next();
            try
            {
                var p1 = persistenceService.Save(entityName, person);
                var p2 = persistenceService.FindById<Person>(entityName, p1["objectId"].ToString());
                Assert.AreEqual("Bob", p2.Name);
                Assert.AreEqual("21", p2.Age.ToString());
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
                var person = persistenceService.FindById<Person>("Person", "objectId111");
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
                BackendlessDataQuery query = new BackendlessDataQuery();
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findAllWithoutParametersInUserTableTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 15; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
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
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = 1;
                query.PageSize = 2;
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
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
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict);
                }

                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = 1;
                query.PageSize = 2;
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
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
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Name", "test" + i);
                    dict.Add("Age", 20 + i);
                    persistenceService.Save(entityName, dict);
                }

                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = 1;
                query.PageSize = 2;
                query.QueryOptions = new QueryOptions { SortBy = new List<string> { "Name"} };
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
                BackendlessDataQuery query2 = new BackendlessDataQuery();
                query2.Offset = 3;
                query2.PageSize = 3;
                query.QueryOptions = new QueryOptions { SortBy = new List<string> { "Name" } };
                query2.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps2 = persistenceService.Find<Person>(entityName, query2);
                Assert.IsNotNull(ps2);
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
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = -1;
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1004", ex.Code);
            }
        }
        [TestMethod()]
        public void findWithOffsetGreaterThanMaxNumberOfRecordsTest()
        {
            string entityName = "Person" + random.Next();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Name", "test");
            dict.Add("Age", 20);
            persistenceService.Save(entityName, dict);
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = 3;
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1004", ex.Code);
            }
        }
        [TestMethod()]
        public void findWithNegativePageSizeOptionTest()
        {
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.PageSize = -2;
                query.Offset = 1;
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1005", ex.Code);
            }
        }
        [TestMethod()]
        public void findWithZeroPageSizeOptionTest()
        {
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.PageSize = 0;
                query.Offset = 0;
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                //Assert.AreEqual("Internal client exception", ex.Code);
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPageSizeGreaterThan100Test()
        {
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.PageSize = 102;
                query.Offset = 1;
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1005", ex.Code);
            }
        }
        [TestMethod()]
        public void findWithPropertiesTest()
        {
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.PageSize = 15;
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithPropsInUserTableTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 15; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
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
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Properties = new List<string> { "Name2", "Age2" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1006", ex.Code);
            }
        }
        [TestMethod()]
        public void findWithPropertiesWhichBelongOrNotBelongToTheObjectTest()
        {
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Properties = new List<string> { "ABC", "Name", "Age2" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1006", ex.Code);                                
            }
        }
        [TestMethod()]
        public void findWithPropertiesWhichBelongOrNotBelongToUserTableTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 15; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.Offset = 1;
                query.PageSize = 2;
                query.Properties = new List<string> { "Name", "Age2", "ddd" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForIntTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 15; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Age > 25";
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForStringTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 15; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);

                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Name LIKE 'test1%'";
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForOrTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 3; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 27 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Name='test1' or Age=28";
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForANDORTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 4; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Name='Test1' or (Name ='Test3' and Age=23)";
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithWhereConditionForBooleanTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 3; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.WhereClause = "Age<>30";
                query.Properties = new List<string> { "Name", "Age" };
                BackendlessCollection<Person> ps = persistenceService.Find<Person>(entityName, query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void findWithSortByConditionTest()
        {
            string entityName = "Person";
            //for (int i = 0; i < 15; i++)
            //{
            //    Dictionary<string, object> dict = new Dictionary<string, object>();
            //    dict.Add("Name", "test" + i);
            //    dict.Add("Age", 20 + i);
            //    persistenceService.Save(entityName, dict);
            //}

            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.QueryOptions = new QueryOptions { SortBy = new List<string> { "Name" } };
                var ps = persistenceService.Find<Person>(query);
                Assert.IsNotNull(ps);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("adafa", ex.Code);
            }

        }
        [TestMethod()]
        public void retrieveFirstObjectTest()
        {
            try
            {
                var p = persistenceService.First<Person>();
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                //Assert.AreEqual("Internal client exception", ex.Code);
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveFirstUserObjectTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 15; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                var p = persistenceService.First<Person>(entityName);
                Assert.IsNotNull(p);
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
                var p = persistenceService.First<Person>("testdddddd");
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {

                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrieveFirstObjectFromUserTableTest()
        {
            try
            {
                var p = persistenceService.First<Person>("User");
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrieveFirstObjectFromUsersTableTest()
        {
            try
            {
                var p = persistenceService.First<Person>("Users");
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrieveFirstForTableWithoutDataTest()
        {
            string entityName = "Person" + random.Next();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Name", "test");
            dict.Add("Age", 20);
            dict.Add("Birthday", DateTime.Now);

            try
            {
                var person = persistenceService.Save(entityName, dict);
                persistenceService.Remove<Person>(entityName, person["objectId"].ToString());
                var p = persistenceService.First<Person>(entityName);
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1010", ex.Code);
            }
        }
        [TestMethod()]
        public void retrieveLastObjectTest()
        {
            try
            {
                var p = persistenceService.Last<Person>();
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void retrieveLastUserObjectTest()
        {
            string entityName = "Person" + random.Next();
            for (int i = 0; i < 15; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                dict.Add("Age", 20 + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                var p = persistenceService.Last<Person>(entityName);
                Assert.IsNotNull(p);
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
                var p = persistenceService.Last<Person>("testdddddd2");
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrieveLastObjectFromUserTableTest()
        {
            try
            {
                var p = persistenceService.Last<Person>("User");
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrieveLastObjectFromUsersTableTest()
        {
            try
            {
                var p = persistenceService.Last<Person>("Users");
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrieveLastForTableWithoutDataTest()
        {
            string entityName = "Person" + random.Next();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Name", "test");
            dict.Add("Age", 20);
            dict.Add("Birthday", DateTime.Now);

            try
            {
                var person = persistenceService.Save(entityName, dict);
                persistenceService.Remove<Person>(entityName, person["objectId"].ToString());
                var p = persistenceService.Last<Person>(entityName);
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1010", ex.Code);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfObjectTest()
        {
            string entityName = "Person" + random.Next();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Name", "test");
            dict.Add("Age", 20);

            try
            {
                persistenceService.Save(entityName, dict);
                List<ObjectProperty> props = persistenceService.Describe(entityName);
                Assert.IsNotNull(props);
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
                List<ObjectProperty> props = persistenceService.Describe("User");
                Assert.IsNotNull(props);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfObjectForTheUsersTableTest()
        {
            try
            {
                List<ObjectProperty> props = persistenceService.Describe("Users");
                Assert.IsNotNull(props);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfUnknownObjectTest()
        {
            try
            {
                List<ObjectProperty> props = persistenceService.Describe("P12345");
                Assert.IsNotNull(props);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1009", ex.Code);
            }
        }
        [TestMethod()]
        public void retrievePropertiesOfUserObjectTest()
        {
            string entityName = "Person" + random.Next();
            Dictionary<string, object> dict= new Dictionary<string, object>();
            dict.Add("Name", "Bob222");
            dict.Add("Age", 20);
            dict.Add("Birthday", DateTime.Now);
           
            persistenceService.Save(entityName, dict);
           
            try
            {
                List<ObjectProperty> props = persistenceService.Describe(entityName);
                Assert.IsNotNull(props);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("Internal client exception", ex.Code);
               
            }
        }
        [TestMethod()]
        public void retrieveNextPageObjectTest()
        {
            string entityName = "Person";
            for (int i = 0; i < 6; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.QueryOptions = new QueryOptions { SortBy = new List<string> { "Name" }, PageSize = 5, Offset = 0 };
                var p = persistenceService.Find<Person>(entityName, query).NextPage();
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void retrieveGetPageObjectTest()
        {
            string entityName = "Person";
            for (int i = 0; i < 6; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "test" + i);
                persistenceService.Save(entityName, dict);
            }
            try
            {
                BackendlessDataQuery query = new BackendlessDataQuery();
                query.QueryOptions = new QueryOptions { SortBy = new List<string> { "Name" }, PageSize = 5, Offset = 0 };
                var p = persistenceService.Find<Person>(entityName,query).GetPage(0);
                Assert.IsNotNull(p);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void deletingObjectByIdTest()
        {
            string entityName = "Person" + random.Next();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Name", "Bob222");
            dict.Add("Age", 20);
            dict.Add("Birthday", DateTime.Now);
            
            try
            {
                var p = persistenceService.Save(entityName, dict);
                var result=persistenceService.Remove<Person>(entityName,p["objectId"].ToString());
                Assert.IsNotNull(result);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void deletingObjectWithInvalidIdTest()
        {
            string entityName = "Person" + random.Next();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Name", "Bob222");
            dict.Add("Age", 20);
            dict.Add("Birthday", DateTime.Now);

            try
            {
                var p = persistenceService.Save(entityName, dict);
                var result = persistenceService.Remove<Person>(entityName, "ddddddfsfasfs");
                Assert.IsNotNull(result);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1000", ex.Code);
            }
        }


        [TestMethod()]
        public void updatingObjectTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            string entityName = "Person" + random.Next();
            try
            {
                var p1 = persistenceService.Save(entityName, person);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob2");
                dict.Add("Age", 211);
                dict.Add("objectId", p1["objectId"].ToString());
                var p2 = persistenceService.Update(entityName, dict);
            }
            catch (BackendlessException ex)
            {
              //  Assert.AreEqual("Server.Processing", ex.Code);
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void updatingObjectWithWrongObjectIdTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            string entityName = "Person" + random.Next();
            try
            {
                var p1 = persistenceService.Save(entityName, person);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob2");
                dict.Add("Age", 211);
                dict.Add("objectId", p1["objectId"].ToString());
                var p2 = persistenceService.Update(entityName, dict);
            }
            catch (BackendlessException ex)
            {

                //Assert.AreEqual("Server.Processing", ex.Code);
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void updatingRecordWithoutBodyTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            string entityName = "Person" + random.Next();
            try
            {
                var p1 = persistenceService.Save(entityName, person);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("objectId", p1["objectId"].ToString());
                var p2 = persistenceService.Update(entityName, dict);
            }
            catch (BackendlessException ex)
            {
               // Assert.AreEqual("Server.Processing", ex.Code);
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void updatingRecordWithNonExistingFieldsTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            string entityName = "Person" + random.Next();
            try
            {
                var p1 = persistenceService.Save(entityName, person);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name2", "Bob2");
                dict.Add("Age2", 211);
                dict.Add("objectId", p1["objectId"].ToString());
                var p2 = persistenceService.Update(entityName, dict);
              //  Assert.AreEqual("objectId", p1["objectId"].ToString());
            }
            catch (BackendlessException ex)
            {
             //  Assert.AreEqual("Server.Processing", ex.Code);
               Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void updatingRecordWithInvalidStringValuesForNumericFieldTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            person.Add("Birthday", DateTime.Now);
            string entityName = "Person" + random.Next();
            try
            {
                var p1 = persistenceService.Save(entityName, person);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob");
                dict.Add("Age", "test");
                dict.Add("Birthday", DateTime.Now);
                dict.Add("objectId", p1["objectId"].ToString());
                var p2 = persistenceService.Update(entityName, dict);
               
            }
            catch (BackendlessException ex)
            {
               Assert.AreEqual("1007", ex.Code);
            }
        }
        [TestMethod()]
        public void updatingRecordWithInvalidNumericValuesForStringFieldTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            person.Add("Birthday", DateTime.Now);
            string entityName = "Person" + random.Next();
            try
            {
                var p1 = persistenceService.Save(entityName, person);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", 1234);
                dict.Add("Age", 21);
                dict.Add("Birthday", DateTime.Now);
                dict.Add("objectId", p1["objectId"].ToString());
                var p2 = persistenceService.Update(entityName, dict);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1007", ex.Code);
            }
        }
        [TestMethod()]
        public void updatingRecordWithInvalidStringValuesForDateFieldTest()
        {
            Dictionary<string, object> person = new Dictionary<string, object>();
            person.Add("Name", "Bob");
            person.Add("Age", 21);
            person.Add("Birthday", DateTime.Now);
            string entityName = "Person" + random.Next();
            try
            {
                var p1 = persistenceService.Save(entityName, person);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Name", "Bob");
                dict.Add("Age", 21);
                dict.Add("Birthday", "test");
                dict.Add("objectId", p1["objectId"].ToString());
                var p2 = persistenceService.Update(entityName, dict);
                
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("1007", ex.Code);
            }
        }*/
    }
}
