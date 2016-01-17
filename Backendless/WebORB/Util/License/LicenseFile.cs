using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Weborb.Util.License
{
    public class LicenseFile
    {
        public const String FILE_NAME = "weborblicense.key.config";

        public string licenseKey;
        public string activationKey;
        public LicenseKeyInfo licenseKeyInfo;

        private String path;

        public bool IsValid
        {
            get
            {
                return licenseKeyInfo != null;
            }
        }

        public static LicenseFile Load( String path )
        {
            LicenseFile licenseFile = new LicenseFile();

            licenseFile.path = path;

            if( File.Exists( path ) )
            {
                using( StreamReader streamReader = File.OpenText( path ) )
                {
                    licenseFile.licenseKey = streamReader.ReadLine();

                    if( !streamReader.EndOfStream )
                    {
                        licenseFile.activationKey = streamReader.ReadLine();

                        try
                        {
                            licenseFile.licenseKeyInfo = LicenseKeyDecoder.decode(
                                licenseFile.licenseKey, licenseFile.activationKey );
                        }
                        catch
                        {
                            licenseFile.activationKey = null;
                        }
                    }
                }
            }

            return licenseFile;
        }

        public void Save()
        {
            using( StreamWriter streamWriter = new StreamWriter( File.OpenWrite( path ) ) )
            {
                streamWriter.WriteLine( licenseKey );
                streamWriter.WriteLine( activationKey );
            }
        }

    }
}
