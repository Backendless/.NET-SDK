using System;
using System.Collections.Generic;

namespace TestProject
{
  public class Person
  {
    public String name;
    public Int32? age;
    public String objectId;
    public List<Order> Surname;
  }

  public class Order
  {
    public String objectId;
    public String LastName{ get; set; }
  }
}
