using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Weborb.Util.Logging;

namespace Weborb.Util.Logging
{
    public sealed class LogReader
    {
        static readonly LogReader instance = new LogReader();
        private FileStream fileStream;
        private static readonly string PATH = Path.Combine(Paths.GetWebORBPath(), "logs");
        public static readonly int BYTES_PER_PAGE = 20000;

        private long fileLength;

        static LogReader()
        {
        }

        LogReader()
        {
        }

        public String getPage(String fileName, int page)
        {
            string path = PATH;
            string file = fileName;
            FileInfo f = new FileInfo( path + Path.DirectorySeparatorChar + file );
            fileLength = f.Length;
            fileStream = f.Open( FileMode.Open, FileAccess.Read, FileShare.ReadWrite );

            fileStream.Seek(BYTES_PER_PAGE * page, SeekOrigin.Begin);
            int count = BYTES_PER_PAGE;
            int offset = BYTES_PER_PAGE * page;
            if (fileStream.Position + BYTES_PER_PAGE > fileLength)
                count = (int)(fileLength - fileStream.Position);
            byte[] b = new byte[count];

            fileStream.Read(b, 0, count);

            return Encoding.ASCII.GetString(b);            
        }

        public static LogReader Instance
        {
            get
            {
                return instance;
            }
        }

        public int getNumberOfPages( String fileName )
        {
           string path = PATH;
           string file = fileName;
           try
             {
             FileInfo f = new FileInfo( path + Path.DirectorySeparatorChar + file );
             fileLength = f.Length;
             fileStream = f.Open( FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
             }
           catch(Exception ex)
             {
             if ( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "Cann't get number of pages in " + fileName + " due to ", ex );
             return 0;
             }
           return (int)(fileLength / BYTES_PER_PAGE) + 1;
        }
    }
}
