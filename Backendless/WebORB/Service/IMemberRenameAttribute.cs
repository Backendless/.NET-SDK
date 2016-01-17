using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Weborb.Service
{
    public interface IMemberRenameAttribute
    {
        string GetClientName(Type type, MemberInfo memberInfo);
    }
}
