using System;
using System.Collections.Generic;

namespace BackendlessAPI.Persistence
{
  public class JSONUpdateBuilder
  {
    public static String OPERATION_FIELD_NAME = "___operation";
    public static String ARGS_FIELD_NAME = "args";
    internal static Dictionary<String, Object> jsonUpdate = new Dictionary<String, Object>();

    public enum Operation
    {
      JSON_SET, JSON_INSERT, JSON_REPLACE, JSON_REMOVE, JSON_ARRAY_APPEND, JSON_ARRAY_INSERT
    }

    private JSONUpdateBuilder( Operation op )
    {
      jsonUpdate[ OPERATION_FIELD_NAME ] = op;
    }

    public static GeneralArgHolder SET()
    {
      new JSONUpdateBuilder( Operation.JSON_SET );
      return new GeneralArgHolder( jsonUpdate );
    }

    public static GeneralArgHolder INSERT()
    {
      new JSONUpdateBuilder( Operation.JSON_INSERT );
      return new GeneralArgHolder( jsonUpdate );
    }

    public static GeneralArgHolder REPLACE()
    {
      new JSONUpdateBuilder( Operation.JSON_REPLACE );
      return new GeneralArgHolder( jsonUpdate );
    }

    public static RemoveArgHolder REMOVE()
    {
      new JSONUpdateBuilder( Operation.JSON_REMOVE );
      return new RemoveArgHolder( jsonUpdate );
    }

    public static GeneralArgHolder ARRAY_APPEND()
    {
      new JSONUpdateBuilder( Operation.JSON_ARRAY_APPEND );
      return new GeneralArgHolder( jsonUpdate );
    }

    public static GeneralArgHolder ARRAY_INSERT()
    {
      new JSONUpdateBuilder( Operation.JSON_ARRAY_INSERT );
      return new GeneralArgHolder( jsonUpdate );
    }

    public abstract class ArgHolder
    {
      protected ArgHolder()
      {}

      public Dictionary<String, Object> Create()
      {
        return jsonUpdate;
      }
    }

    public class GeneralArgHolder : ArgHolder
    {
      private Dictionary<String, Object> jsonUpdateArgs = new Dictionary<String, Object>();

      internal GeneralArgHolder( Dictionary<String, Object> jsonUpdate )
      {
        jsonUpdate[ ARGS_FIELD_NAME ] = jsonUpdateArgs;
      }

      public GeneralArgHolder AddArgument( String jsonPath, Object value )
      {
        jsonUpdateArgs[ jsonPath ] = value;
        return this;
      }
    }

    public class RemoveArgHolder : ArgHolder
    {
      private List<String> jsonUpdateArgs = new List<String>();
      internal RemoveArgHolder( Dictionary<String, Object> jsonUpdate)
      {
        jsonUpdate[ ARGS_FIELD_NAME ] = jsonUpdateArgs;
      }

      public RemoveArgHolder AddArgument( String jsonPath )
      {
        jsonUpdateArgs.Add( jsonPath );
        return this;
      }
    }
  }
}
