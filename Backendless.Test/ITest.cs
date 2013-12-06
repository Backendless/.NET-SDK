using BackendlessAPI.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test
{
  public abstract class ITest
  {
    public virtual void CheckErrorCode( string expectedCode, BackendlessFault resultFault )
    {
      СheckStringExpectation( expectedCode, resultFault.FaultCode );
    }

    public virtual void CheckErrorCode(int expectedCode, BackendlessFault resultFault)
    {
      CheckCodeExpectation( expectedCode, resultFault.FaultCode, resultFault.Message );
    }

    public virtual void CheckErrorCode( string expectedCode, System.Exception resultException )
    {
      СheckStringExpectation( expectedCode, resultException );
    }

    public virtual void CheckErrorCode( int expectedCode, System.Exception resultException )
    {
      if( resultException is BackendlessException )
        CheckCodeExpectation( expectedCode, ((BackendlessException) resultException).FaultCode, resultException.Message );
      else
        СheckStringExpectation( expectedCode.ToString(), resultException );
    }

    private void СheckStringExpectation( string expectedMessage, System.Exception actualMessage )
    {
      Assert.IsTrue( actualMessage.Message.Contains( expectedMessage ),
                     "Server returned a wrong error code. \n" + "Expected: " + expectedMessage + "\n" + "Got: " +
                     actualMessage.Message );
    }

    private void СheckStringExpectation( string expectedMessage, string actualMessage )
    {
      Assert.AreEqual( expectedMessage, actualMessage,
                       "Server returned a wrong error code. \n" + "Expected: " + expectedMessage + "\n" + "Got: " +
                       actualMessage );
    }

    private void CheckCodeExpectation( int expectedCode, string actualCode, string message )
    {
      Assert.AreEqual( expectedCode.ToString(), actualCode,
                       "Server returned a wrong error code. \n" + "Expected: " + expectedCode + "\n" + "Got: { " +
                       actualCode + "\n" + message + " }" );
    }
  }
}