using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  public enum OperationTypeEnum
  {
    CREATE,
    CREATE_BULK,
    UPDATE,
    UPDATE_BULK,
    DELETE,
    DELETE_BULK,
    FIND,
    ADD_RELATION,
    SET_RELATION,
    DELETE_RELATION
  };
  public class OperationType
  {
    private String operationName;

    public String OperationName
    {
      get
      {
        return operationName;
      }
      set
      {
        operationName = value;
      }
    }
    public static ReadOnlyCollection<OperationTypeEnum> supportCollectionEntityDescriptionType = new ReadOnlyCollection<OperationTypeEnum>
                                                 ( new List<OperationTypeEnum> { OperationTypeEnum.FIND } );

    public static ReadOnlyCollection<OperationTypeEnum> supportListIdsResultType = new ReadOnlyCollection<OperationTypeEnum>
                                                 ( new List<OperationTypeEnum> { OperationTypeEnum.CREATE_BULK } );

    public static ReadOnlyCollection<OperationTypeEnum> supportDeletionResultType = new ReadOnlyCollection<OperationTypeEnum>
                                                 ( new List<OperationTypeEnum> { OperationTypeEnum.DELETE } );

    public static ReadOnlyCollection<OperationTypeEnum> supportIntResultType = new ReadOnlyCollection<OperationTypeEnum>
                                                 ( new List<OperationTypeEnum> { OperationTypeEnum.UPDATE_BULK, OperationTypeEnum.DELETE_BULK,
                                                 OperationTypeEnum.ADD_RELATION, OperationTypeEnum.SET_RELATION, OperationTypeEnum.DELETE_RELATION } );

    public static ReadOnlyCollection<OperationTypeEnum> supportEntityDescriptionResultType = new ReadOnlyCollection<OperationTypeEnum>
                                                 ( new List<OperationTypeEnum> { OperationTypeEnum.CREATE, OperationTypeEnum.UPDATE } );
  }
}
