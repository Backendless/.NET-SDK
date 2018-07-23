using System;
namespace BackendlessAPI.RT.Data
{
  public class DataSubscription : RTSubscription
  {
    public DataSubscription( RTDataEvents rtDataEvent, String tableName, IRTCallback callback ) : base( SubscriptionNames.OBJECTS_CHANGES, callback )
    {
      PutOption( "event", Enum.GetName( typeof( RTDataEvents ), rtDataEvent ) );
      PutOption( "tableName", tableName );
    }

    public DataSubscription WithWhere( String whereClause )
    {
      PutOption( "whereClause", whereClause );
      return this;
    }

    public RTDataEvents Event
    {
      get
      {
        String eventStr = (String) GetOption( "event" );
        return eventStr == null ? RTDataEvents.unknown : (RTDataEvents) Enum.Parse( typeof( RTDataEvents ), eventStr );
      }
    }

    public String TableName 
    {
      get 
      {
        return (String) GetOption( "tableName" );
      }
    }

    public String WhereClause 
    {
      get
      {
        return (String) GetOption( "whereClause" );
      }
    }
  }
}
