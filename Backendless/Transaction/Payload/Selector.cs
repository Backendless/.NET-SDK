using System;

namespace BackendlessAPI.Transaction.Payload
{
  public class Selector
  {
    private String conditional;
    private Object unconditional;

    public Selector()
    {
    }

    public Selector( String conditional, Object unconditional )
    {
      this.conditional = conditional;
      this.unconditional = unconditional;
    }

    public String Conditional
    {
      get => conditional;
      set => conditional = value;
    }

    public Object Unconditional
    {
      get => unconditional;
      set => unconditional = value;
    }

    public override string ToString()
    {
      return "Selector{" +
             "conditional='" + conditional + '\'' +
             ", unconditional=" + unconditional +
             '}';
    }
  }
}
