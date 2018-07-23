using System;
namespace BackendlessAPI.Utils
{
  public class TimeoutManagerImpl : ITimeoutManager
  {
    private static readonly int INITIAL_TIMEOUT = 200;
    private static readonly int MAX_TIMEOUT = 60 * 1000; //1 min
    private static readonly int REPEAT_TIMES_BEFORE_INCREASE = 10;

    private int repeatedTimes;
    private int currentTimeOut;

    public TimeoutManagerImpl()
    {
      Reset();
    }

    public int NextTimeout()
    {
      if( currentTimeOut > MAX_TIMEOUT )
        return MAX_TIMEOUT;

      if( repeatedTimes++ > 0 && (repeatedTimes % REPEAT_TIMES_BEFORE_INCREASE) == 0 )
        currentTimeOut *= 2;

      return currentTimeOut;
    }

    public int RepeatedTimes()
    {
      return repeatedTimes;
    }

    public void Reset()
    {
      repeatedTimes = 0;
      currentTimeOut = INITIAL_TIMEOUT;
    }
  }
}
