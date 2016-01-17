using System;
using System.Collections;
using System.Text;
using Weborb.Types;

namespace Weborb.Reader
{
  public class ArrayCollectionType : ArrayType
  {
    private ArrayType _arrayType;

    public ArrayType GetArrayType()
    {
      return _arrayType;
    }

    public ArrayCollectionType( Object[] array, ArrayType arrayType ) : base( array )
    {
      _arrayType = arrayType;
    }

    public override object defaultAdapt()
    {
      return new WebORBArrayCollection( (ICollection) base.defaultAdapt() );
    }
  }
}
