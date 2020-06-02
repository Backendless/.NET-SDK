using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class OpResultIdGenerator
  {
    private List<String> opResultIdStrings;
    private Dictionary<String, Int32> opResultIdMaps = new Dictionary<String, Int32>();

    internal OpResultIdGenerator( List<String> opResultIdStrings )
    {
      this.opResultIdStrings = opResultIdStrings;
    }

    internal String GenerateOpResultId( OperationType operationType, String tableName )
    {
      String opResultIdGenerated;
      String key = OperationTypeUtil.GetOperationName( operationType ) + tableName;

      if( opResultIdMaps.ContainsKey( key ) )
      {
        Int32 count = opResultIdMaps[ key ];
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
