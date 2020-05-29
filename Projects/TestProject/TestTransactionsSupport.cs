using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
  public class Person
  {
    private String name;
    private int age;
    private String objectId;

    public String GetObjectId()
    {
      return objectId;
    }

    public void SetObjectId( String objectId )
    {
      this.objectId = objectId;
    }

    public String GetName()
    {
      return name;
    }

    public void SetName( String name )
    {
      this.name = name;
    }

    public int GetAge()
    {
      return age;
    }

    public void SetAge( int age )
    {
      this.age = age;
    }
  }

  public class Order
  {
    private String objectId;
    private String name;

    public String GetObjectId()
    {
      return objectId;
    }

    public void SetObjectId( String objectId )
    {
      this.objectId = objectId;
    }

    public String GetLastName()
    {
      return name;
    }

    public void SetLastName( String name )
    {
      this.name = name;
    }
  }
}
