using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  public class UnitOfWork
  {
    public static String REFERENCE_MARKER = "___ref";
    public static String OP_RESULT_ID = "opResultId";
    public static String RESULT_INDEX = "resultIndex";
    public static String PROP_NAME = "propName";

    private LevelEnum transactionIsolation = LevelEnum.REPEATABLE_READ;
    private List<Operation<Object>> operations;
    private List<String> opResultIdStrings;
  }
}
