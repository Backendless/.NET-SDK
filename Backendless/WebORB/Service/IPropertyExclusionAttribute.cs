using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Service
{
    public interface IPropertyExclusionAttribute
    {
        bool ExcludeProperty( object obj, string propName );
    }
}
