
using System;
using System.Collections.Generic;
using BackendlessAPI.Property;


namespace BackendlessAPI.Test
{
    public class Person 
    {
        private string name;
        private int age;
        private DateTime birthday;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Age
        {
            get { return this.age; }
            set { this.age = value; }
        }
        public DateTime Birthday
        {
            get { return this.birthday; }
            set { this.birthday = value; }
        }
    }
}
