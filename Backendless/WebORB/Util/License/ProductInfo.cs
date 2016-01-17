using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Util.License
{
    public class ProductInfo
    {
        public int id;
        public String name;
        public String licenseFileName;
        public String assemblyFullName;

        public ProductInfo()
        {
        }

        public ProductInfo( int id, String name, String licenseFileName, String assemblyFullName )
        {
            this.id = id;
            this.name = name;
            this.licenseFileName = licenseFileName;
            this.assemblyFullName = assemblyFullName;
        }
    }
}
