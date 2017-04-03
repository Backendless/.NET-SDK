using System;
using System.Collections.Generic;
using Backendless.Test;
using BackendlessAPI.Data;
using BackendlessAPI.Test.PersistenceService.AsyncEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.PersistenceService.AsyncTests
{
  [TestClass]
  public class TestsFrame: IAsyncTest
  {
    private Random random = new Random();
    public const string LOGIN_KEY = "login";
    public const string EMAIL_KEY = "email";
    public const string PASSWORD_KEY = "password";
    public const string ID_KEY = "id";

    public WPPersonAsync GetRandomWPPerson()
    {
      return new WPPersonAsync {Age = random.Next( 80 ), Name = "bot_" + DateTime.Now.Ticks};
    }

    public void AssertArgumentAndResultCollections<T>( List<T> entities, IList<T> backendlessCollection )
    {
      Assert.AreEqual( entities.Count, backendlessCollection.Count, "Server sent wrong number of objects" );

      foreach( T entity in entities )
        Assert.IsTrue( backendlessCollection.Contains( entity ),
                       "Server result didn't contain expected entity" );

      CountDown();
    }

    [TestInitialize]
    public void SetUp()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
    }
  }
}