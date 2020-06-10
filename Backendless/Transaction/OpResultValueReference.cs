using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class OpResultValueReference
  {
    public OpResultValueReference( OpResult opResult, Int32? resultIndex, String propName )
    {
      OpResult = opResult;
      ResultIndex = resultIndex;
      PropName = propName;
    }

    public OpResultValueReference( OpResult opResult, Int32? resultIndex )
    {
      OpResult = opResult;
      ResultIndex = resultIndex;
      PropName = null;
    }

    public OpResultValueReference( OpResult opResult, String propName )
    {
      OpResult = opResult;
      ResultIndex = null;
      PropName = propName;
    }

    [SetClientClassMemberName("opResult")]
    public OpResult OpResult
    {
      get;
      private set;
    }

    [SetClientClassMemberName( "resultIndex" )]
    public Int32? ResultIndex
    {
      get;
      private set;
    }

    [SetClientClassMemberName( "propName" )]
    public String PropName
    {
      get;
      private set;
    }

    public OpResultValueReference ResolveTo( String propName )
    {
      return new OpResultValueReference( OpResult, ResultIndex, propName );
    }

    internal Dictionary<String, Object> MakeReference()
    {
      Dictionary<String, Object> referenceMap = OpResult.MakeReference();

      if( ResultIndex != null )
        referenceMap[ UnitOfWork.RESULT_INDEX ] = ResultIndex;

      if( PropName != null )
        referenceMap[ UnitOfWork.PROP_NAME ] = PropName;

      return referenceMap;
    }
  }
}
