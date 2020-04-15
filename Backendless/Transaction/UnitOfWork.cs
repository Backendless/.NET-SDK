using BackendlessAPI.Async;
using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  public class UnitOfWork// : IUnitOfWork
  {
    public static String REFERENCE_MARKER = "___ref";
    public static String OP_RESULT_ID = "opResultId";
    public static String RESULT_INDEX = "resultIndex";
    public static String PROP_NAME = "propName";

    private LevelEnum transactionIsolation = LevelEnum.REPEATABLE_READ;
    private List<Operation> operations;
    private List<String> opResultIdStrings;

    private UnitOfWorkExecutor unitOfWorkExecutor;

    public LevelEnum TransactionIsolation
    {
      get => transactionIsolation;
    }
    public List<Operation> Operations
    {
      get => operations;
    }

    public List<String> OpResultIdStrings
    {
      get => opResultIdStrings;
    }

    public UnitOfWorkResult Execute()
    {
      return unitOfWorkExecutor.Execute();
    }

    public void Execute( AsyncCallback<UnitOfWorkResult> callback )
    {
      unitOfWorkExecutor.Equals( callback );
    }

  }
}
