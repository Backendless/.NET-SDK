using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Util.Codegen
{
    public class CodegenXsltExtentionAttribute :Attribute
    {
        public String Uri;

        public CodegenXsltExtentionAttribute(String uri)
        {
            Uri = uri;
        }
    }
}
