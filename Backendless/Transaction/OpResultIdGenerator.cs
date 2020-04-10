using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class OpResultIdGenerator
  {
    private List<String> opResultIdStrings;
    private Dictionary<String, Int32> opResultIdMaps = new Dictionary<String, Int32>();

    OpResultIdGenerator( List<String> opResultIdStrings )
    {
      this.opResultIdStrings = opResultIdStrings;
    }

    String GenerateOpResultId( OperationType operationType, String tableName )
    {
      String opResultIdGenerated;
      String key = operationType.OperationName + tableName;

      if( opResultIdMaps.ContainsKey( key ) )
      {
        int count = opResultIdMaps[ key ];
        opResultIdMaps[ key ] = ++count;
        opResultIdGenerated = key + count;
      }
      else 
      {
        opResultIdMaps[ key ] = 1;
        opResultIdGenerated = key + 1;
      }

      opResultIdStrings.Add( opResultIdGenerated );
      return opResultIdGenerated;
    }
  }
}
