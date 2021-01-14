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

  public class Area
  {
    public int UserId { get; set; }
    public string AreaA { get; set; }
    public bool Categories { get; set; }
  }

  public class Human
  {
    public List<Area> Related { get; set; }
    public String name { get; set; }
    public Int32? age { get; set; }
    public String objectId { get; set; }
  }
}