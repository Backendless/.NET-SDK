using Xunit;

namespace TestProject
{
  public class TestsCleaner
  {
    [CollectionDefinition( "Tests" )]
    public class PersistenceFindCleanupRunner : ICollectionFixture<TestInitialization>
    {
      // This class has no code, and is never created. Its purpose is simply
      // to be the place to apply [CollectionDefinition] and the
      // ICollectionFixture<> interface.
    }
  }
}