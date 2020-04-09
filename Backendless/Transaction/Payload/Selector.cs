using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
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
      get
      {
        return conditional;
      }
      set
      {
        conditional = value;
      }
    }

    public Object Unconditional
    {
      get
      {
        return unconditional;
      }
      set
      {
        unconditional = value;
      }
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
