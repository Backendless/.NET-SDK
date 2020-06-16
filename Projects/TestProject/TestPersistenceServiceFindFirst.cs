using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using System;
using BackendlessAPI.Persistence;

namespace TestProject
{
  [TestClass]
  public class TestPersistenceServiceFindFirst
  {
    [TestMethod]
    public void TestFindFirstMethod_Sync()
    {
      List<Dictionary<String, Object>> personList = new List<Dictionary<String, Object>>();
      Dictionary<String, Object> person = new Dictionary<String, Object>();
      person[ "name" ] = "Alexandra";
      person[ "age" ] = 17;
      personList.Add( person );
      IList<String> listId = Backendless.Data.Of( "Person" ).Create( personList );

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperty( "age" );

      Dictionary<String, Object> result = Backendless.Data.Of( "Person" ).FindFirst( queryBuilder );
      Assert.IsNotNull( result );
      Assert.IsTrue( result.ContainsKey( "age" ) );
      Assert.IsFalse( result.ContainsKey( "name" ) );
      Assert.AreEqual( result[ "objectId" ], listId[ 0 ], "objectId were not equivalent" );

      Backendless.Data.Of( "Person" ).Remove( "age = '17'" );
    }
  }
}
