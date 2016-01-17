using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Weborb.Util.Codegen
{
    public class CodeDirectory:CodeItem
    {
        private List<CodeItem> m_items = new List<CodeItem>();

        public override int GetLineCount()
        {
            int count = 0;

            foreach( CodeItem item in m_items )
                count += item.GetLineCount();

            return count;
        }

        internal void AddItem( CodeItem item )
        {
            m_items.Add( item );
        }

        public CodeItem[] Items
        {
            get { return m_items.ToArray(); }
            set
            {
                m_items = new List<CodeItem>( value );
            }
        }

      public void Build(XmlElement xmlElement)
      {
        string path = Path.Combine(Paths.GetWebORBPath(),
                                   "weborbassets" + Path.DirectorySeparatorChar + "codegen" +
                                   Path.DirectorySeparatorChar + xmlElement.GetAttribute("path"));
        Build(path);
      }

      public void Build(string path)
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        Name = directoryInfo.Name;
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        foreach (FileInfo fileInfo in fileInfos)
        {
          String exstantion = fileInfo.Extension;
          bool hideContent = exstantion == ".dll" || exstantion == ".swc" || exstantion == ".flw" ||
                             exstantion == ".jar" || exstantion == ".exe";
          CodeFile codeFile = new CodeFile();
          codeFile.Directory = this;
          codeFile.Build(path + "/" + fileInfo.Name, hideContent);
          AddItem(codeFile);
        }

        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
        foreach (DirectoryInfo info in directoryInfos)
        {
          if ( info.Name.Equals( ".svn" ) )
            continue;
          CodeDirectory codeDirectory = new CodeDirectory();
          codeDirectory.Directory = this;
          codeDirectory.Build(path + "/" + info.Name);
          AddItem(codeDirectory);
        }
      }
    }
}
