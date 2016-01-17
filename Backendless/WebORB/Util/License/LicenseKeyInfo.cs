using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Weborb.Util.License
{
    public class LicenseKeyInfo
    {
        public DateTime expireDate;
        public bool oem;
        public int product;
        public int licenseType;
        public int limitOfInstances;
        public String activationKey;
        public String licenseKey;
        public int majorVersion;

        public LicenseKeyInfo( int productID )
        {
            product = productID;
            expireDate = DateTime.MaxValue;
            oem = false;

            if( productID == LicenseManager.WEBORB_PRODUCT_ID )
                licenseType = LicenseManager.DEVMODE;
            else
                licenseType = -1;

            limitOfInstances = 1;
            activationKey = null;
            licenseKey = null;

            Assembly weborbAssembly = AppDomain.CurrentDomain.Load( "weborb" );
            majorVersion = weborbAssembly.GetName().Version.Major;
        }

        public LicenseKeyInfo( LicenseKeyInfo copy )
        {
            this.product = copy.product;
            this.expireDate = copy.expireDate;
            this.oem = copy.oem;
            this.licenseType = copy.licenseType;
            this.limitOfInstances = copy.limitOfInstances;
            this.activationKey = copy.activationKey;
            this.licenseKey = copy.licenseKey;
            this.majorVersion = copy.majorVersion;
        }

        public LicenseKeyInfo( String licenseKey, String activationKey, byte[] data )
        {
            if( data.Length != 8 )
                throw new ArgumentException( "Message length must be 8 bytes" );

            this.licenseKey = licenseKey;
            this.activationKey = activationKey;

            product = data[ 0 ];
            oem = (data[ 1 ] >> 4 & 0x0f) == 1;
            licenseType = data[ 1 ] & 0x0f;
            limitOfInstances = data[ 2 ];

            int year = data[ 3 ];
            int month = data[ 4 ];
            int day = data[ 5 ];

            if( (year + day + month) > 0 )
                expireDate = new DateTime( year + 2000, month, day );
            else
                expireDate = DateTime.MaxValue;

            majorVersion = data[ 6 ];
        }
    }
}
