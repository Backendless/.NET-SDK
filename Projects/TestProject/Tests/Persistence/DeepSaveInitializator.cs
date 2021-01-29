using System;
using BackendlessAPI;
using System.Collections.Generic;
using System.Text;
using TestProject.Tests.Utils;

namespace TestProject.Tests.Persistence
{
  public class DeepSaveInitializator : IDisposable
  {
    public DeepSaveInitializator()
    {
      Test_sHelper.CreateDefaultTable( "People" );
      Test_sHelper.CreateDefaultColumn( "People", "Age", "Int" );
      Test_sHelper.CreateDefaultColumn( "People", "Name", "String" );
      Test_sHelper.CreateDefaultTable( "Identity" );
      Test_sHelper.CreateDefaultColumn( "Identity", "Name", "String" );
      Test_sHelper.CreateDefaultColumn( "Identity", "Age", "Int" );
      Test_sHelper.CreateRelationColumn( "Identity", "People", "Family", true );
      Test_sHelper.CreateRelationColumn( "Identity", "People", "Friend", false );      
    }

    public void Dispose()
    {
      Test_sHelper.DeleteTable( "Identity" );
      Test_sHelper.DeleteTable( "People" );
    }
  }
}
