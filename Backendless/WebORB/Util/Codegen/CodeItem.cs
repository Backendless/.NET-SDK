using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCodeInternal.SharpZipLib.Zip;
using System.IO;

namespace Weborb.Util.Codegen
{
    public abstract class CodeItem
    {
        private String m_name;

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private CodeDirectory m_directoty;

        public CodeDirectory Directory
        {
            get { return m_directoty; }
            set { m_directoty = value; }
        }

        public bool IsDirectory()
        {
            return this is CodeDirectory;
        }

        public bool IsFile()
        {
            return this is CodeFile;
        }

        public String GetPath()
        {
            String path = Name;
            CodeDirectory codeDirectory = Directory;

            while (codeDirectory != null)
            {
                path = String.Format("{0}/{1}", codeDirectory.Name, path);
                codeDirectory = codeDirectory.Directory;
            }

            if (IsDirectory())
                path += "/";

            return path;
        }

        public abstract int GetLineCount();

        public static void SaveToZip( CodeItem codeItem, Stream stream, bool fullCode, int type )
        {
            ZipOutputStream zipOutput = new ZipOutputStream( stream );
            SaveToZip( zipOutput, codeItem, fullCode, type );
            zipOutput.Finish();
        }

        public static void SaveToZip( CodeItem codeItem, Stream stream )
        {
            ZipOutputStream zipOutput = new ZipOutputStream( stream );
            SaveToZip( zipOutput, codeItem );
            zipOutput.Finish();
        }

        public static void SaveToZip(ZipOutputStream zipOutput, CodeItem codeItem, bool fullCode, int type)
          {
          if ( codeItem.Name.Equals( "weborb-codegen" ) )
            {
            foreach ( CodeItem childItem in ( (CodeDirectory)codeItem ).Items )
              {
              // get rid of project files if necessary
			        if ( !fullCode && (childItem.Name.StartsWith( "." ) || childItem.Name.EndsWith(".csproj") || 
                    childItem.Name.EndsWith(".vbproj")) )
					        continue;

              childItem.Directory = null;
              CreateZip( childItem, zipOutput );
              }
            } 
            else
            {
                throw new Exception("Not supported");
            }
          }

        public static void SaveToZip(ZipOutputStream zipOutput, CodeItem codeItem )
        {
            if (codeItem.Name.Equals("weborb-codegen"))
            {
                foreach (CodeItem childItem in ((CodeDirectory)codeItem).Items)
                {
                    childItem.Directory = null;
                    CreateZip(childItem, zipOutput);
                }
            }
            else
            {
                throw new Exception("Not supported");
            }
        }

        private static void CreateZip(CodeItem codeItem, ZipOutputStream zipOutput)
        {

            ZipEntry zipEntry = new ZipEntry(codeItem.GetPath());
            zipOutput.PutNextEntry(zipEntry);

            if (codeItem.IsFile())
            {
                //Byte[] code = System.Text.ASCIIEncoding.ASCII.GetBytes(((CodeFile)codeItem).Content);

                zipOutput.Write(((CodeFile)codeItem).BinaryData, 0, ((CodeFile)codeItem).BinaryData.Length);
            }
            else
            {
                foreach (CodeItem childItem in ((CodeDirectory)codeItem).Items)
                    CreateZip(childItem, zipOutput);
            }

        }

        internal CodeItem Find(string name)
        {
            if (Name == name)
                return this;

            CodeItem result = null;

            if (IsDirectory())
            {
                foreach (CodeItem item in ((CodeDirectory)this).Items)
                {
                    result = item.Find(name);

                    if (result != null)
                        break;
                }
            }

            return result;
        }


    }
}
