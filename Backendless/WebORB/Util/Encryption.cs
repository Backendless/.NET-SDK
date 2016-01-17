using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Weborb.Util
{
    public class Encryption
    {
        public static byte[] IV
            = new byte[] { 0x97, 0xe0, 0x13, 0xcb, 0x87, 0x31, 0x4a, 0xa9 };

        public static DES prepareAlgorithm()
        {
            DES des = DES.Create();

            des.Mode = CipherMode.CFB;
            des.Padding = PaddingMode.None;
            des.IV = IV;
            des.BlockSize = 64;
            des.KeySize = 64;

            return des;
        }
    }
}
