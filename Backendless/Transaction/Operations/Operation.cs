using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  public abstract class Operation<T>
  {
    private OperationType operationType;
    private String table;
    private String opResultId;

    public Operation()
    {
    }

    public Operation( OperationType operationType, String table, String opResultId )
    {
      this.operationType = operationType;
      this.table = table;
      this.opResultId = opResultId;
    }

    public OperationType OperationType
    {
      get => operationType;
      set => operationType = value;
    }

    public String Table
    {
      get => table;
      set => table = value;
    }

    public String OpResultId
    {
      get => opResultId;
      set => opResultId = value;
    }

    public abstract T Payload{ get; set; }

    public override string ToString()
    {
      return "Operation{operationType=" + operationType + ", table=" + table + ", opResultId=" + opResultId + ", payload=" + Payload + "}";
    }
  }
}
