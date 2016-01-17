using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Weborb.Util;

namespace Weborb.Util.License
{
    public static class LicenseKeyDecoder
    {
        private static byte[] iv
            = new byte[] { 0x97, 0xe0, 0x13, 0xcb, 0x87, 0x31, 0x4a, 0xa9 };

        public static LicenseKeyInfo decode( String licenseKey, String activationKey )
        {
            if( licenseKey == null )
                throw new ArgumentNullException( "licenseKey" );

            if( activationKey == null )
                throw new ArgumentNullException( "activationKey" );

            if( licenseKey.Length != 19 )
                throw new ArgumentException( "Invalid license key length" );

            if( activationKey.Length != 19 )
                throw new ArgumentException( "Invalid activation key length" );

            // Create a new DES object.
            DES des = Encryption.prepareAlgorithm();

            using( CryptoStream cryptoStream = new CryptoStream(
                new MemoryStream( hexToBytes( licenseKey.Replace( "-", "" ) ), false ),
                des.CreateDecryptor( hexToBytes( activationKey.Replace( "-", "" ) ), iv ),
                CryptoStreamMode.Read ) )
            {
                byte[] decryptedData = new byte[ 8 ];

                cryptoStream.Read( decryptedData, 0, decryptedData.Length );

                return new LicenseKeyInfo( licenseKey, activationKey, decryptedData );
            }

        }

        private static byte[] hexToBytes( String hex )
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[ NumberChars / 2 ];

            for( int i = 0; i < NumberChars; i += 2 )
                bytes[ i / 2 ] = Convert.ToByte( hex.Substring( i, 2 ), 16 );

            return bytes;
        }
    }
}
