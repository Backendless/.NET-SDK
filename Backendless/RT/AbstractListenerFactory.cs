using System;
namespace BackendlessAPI.RT
{
  public delegate T RTListenerCreator<T>();
  public abstract class AbstractListenerFactory<T>
  {
    public T Create( string key, RTListenerCreator<T> rTListenerCreator )
    {
      return rTListenerCreator();
    }
  }
}
