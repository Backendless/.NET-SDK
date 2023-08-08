using System;
using Weborb.Service;

namespace BackendlessAPI
{

  public class BackendlessExpression
  {
    [SetClientClassMemberName("value")]
    public string Value { get; set; }

    public BackendlessExpression() { }

    public BackendlessExpression(String value)
    {
      Value = value;
    }
  }
}