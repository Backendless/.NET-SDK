using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Weborb.Service
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SetClientClassMemberNameAttribute : Attribute, IMemberRenameAttribute
    {
        private string clientClassMemberName;

        public SetClientClassMemberNameAttribute(string clientClassMemberName)
        {
            this.clientClassMemberName = clientClassMemberName;
        }

        public string GetClientName(Type type, MemberInfo memberInfo)
        {
            return clientClassMemberName;
        }
    }
}
