using System;
using Xunit;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
  public class TestsCleaner
  {
    public class FindCleanup : IDisposable
    {
      public void Dispose()
      {
        Test_sHelper.DeleteTable( "Human" );
      }
    }

    [CollectionDefinition( "Tests" )]
    public class PersistenceFindCleanupRunner : ICollectionFixture<TestInitialization>
    {
      // This class has no code, and is never created. Its purpose is simply
      // to be the place to apply [CollectionDefinition] and the
      // ICollectionFixture<> interface.
    }
  }
}
