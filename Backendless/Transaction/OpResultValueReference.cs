using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  public class OpResultValueReference
  {
    private OpResult opResult;
    private Int32? resultIndex;
    private String propName;

    public OpResultValueReference( OpResult opResult, Int32? resultIndex, String propName )
    {
      this.opResult = opResult;
      this.resultIndex = resultIndex;
      this.propName = propName;
    }

    public OpResultValueReference( OpResult opResult, Int32? resultIndex )
    {
      this.opResult = opResult;
      this.resultIndex = resultIndex;
      propName = null;
    }

    public OpResultValueReference( OpResult opResult, String propName )
    {
      this.opResult = opResult;
      resultIndex = null;
      this.propName = propName;
    }

    public OpResult GetOpResult()
    {
      return opResult;
    }

    public Int32? GetResultIndex()
    {
      return resultIndex;
    }

    public String GetPropName()
    {
      return propName;
    }

    public OpResultValueReference ResolveTo( String propName )
    {
      return new OpResultValueReference( opResult, resultIndex, propName );
    }

    /*Dictionary<String, Object> MakeReference()
    {
      Dictionary<String, Object> referenceMap = opResult.MakeReference();

      if( resultIndex != null )
        referenceMap[ UnitOfWork.RESULT_INDEX ] = resultIndex;

      if( propName != null )
        referenceMap[ UnitOfWork.PROP_NAME ] = propName;

      return referenceMap;
    }*/
  }
}
