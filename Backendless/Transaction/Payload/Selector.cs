using System;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Payload
{
  public class Selector
  {
    public Selector()
    {
    }

    public Selector( String conditional, Object unconditional )
    {
      Conditional = conditional;
      Unconditional = unconditional;
    }

    [SetClientClassMemberName("conditional")]
    public String Conditional { get; set; }

    [SetClientClassMemberName("unconditional")]
    public Object Unconditional { get; set; }

    public override string ToString()
    {
      return "Selector{" +
             "conditional='" + Conditional + '\'' +
             ", unconditional=" + Unconditional +
             '}';
    }
  }
}
