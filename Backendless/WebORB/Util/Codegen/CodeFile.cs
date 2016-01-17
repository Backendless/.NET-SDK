using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Weborb.Config;
using Weborb.Messaging.Server;
using System.Reflection;
#if CLOUD
using Weborb.Cloud;
#endif

namespace Weborb.Util.Codegen
{
    public class CodeFile:CodeItem
    {
        public const string HIDDEN_CONTENT = "Content is hidden";
        private string m_content;

        public string Content
        {
            get { return m_content; }
            set { m_content = value; }
        }

        private byte[] m_binaryData;
        public byte[] BinaryData
        {
            get
            {
              /*if( m_binaryData == null && Content != "")
                m_binaryData = System.Text.ASCIIEncoding.ASCII.GetBytes(Content);*/
              return m_binaryData == null ? new byte[0] : m_binaryData;
            }
            set { m_binaryData = value; }
        }

        public override int GetLineCount()
        {
            Regex regex = new Regex( "\n" );
            return regex.Matches( m_content ).Count;
        }

        public void Build(XmlElement xmlElement)
        {
          try
          {
            if (xmlElement.HasAttribute("name"))
            {
              Name = xmlElement.GetAttribute("name");
              if (xmlElement.GetAttribute("type") == "xml")
              {
                if (xmlElement.GetAttribute("addxmlversion") == "true")
                  Content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";

                Content += xmlElement.InnerXml;
                Content = Content.Replace("&gt;", ">");
                Content = Content.Replace("&lt;", "<");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                  XmlDocument xmlDocument = new XmlDocument();

                  xmlDocument.LoadXml(Content);
                  xmlDocument.Normalize();
                  xmlDocument.Save(memoryStream);
                  memoryStream.Flush();
                  memoryStream.Position = 0;
                  Content = new StreamReader(memoryStream).ReadToEnd();
                }
              }
              else
              {
                Content = xmlElement.InnerText; //.Replace("\r\n", "\n");
              }
              BinaryData = System.Text.ASCIIEncoding.ASCII.GetBytes(Content);
            }
            else if (xmlElement.HasAttribute("path"))
            {
              string filePath = Path.Combine(Paths.GetWebORBPath(),
                                             "weborbassets" + Path.DirectorySeparatorChar + "codegen" +
                                             Path.DirectorySeparatorChar + xmlElement.GetAttribute("path"));
              bool hideContent = xmlElement.HasAttribute("hideContent") &&
                                 xmlElement.GetAttribute("hideContent") == "true"
                                   ? true
                                   : false;
              Build(filePath, hideContent);
            }
          }
          catch ( Exception e )
          {
            throw new Exception(String.Format("Name: {0}; Content: {1}", Name, Content), e);
          }
        }

        public void Build( String filePath, bool hideContent )
        {
          Name = Path.GetFileName( filePath );
          if ( File.Exists( filePath ) )
          {
            FileStream file = new FileStream( filePath, FileMode.Open );
            StreamReader streamReader = new StreamReader( file );
            if ( hideContent )
              Content = HIDDEN_CONTENT;
            else
              Content = streamReader.ReadToEnd();
            file.Flush();
            file.Position = 0;
            BinaryData = new byte[file.Length];
            file.Read( BinaryData, 0, BinaryData.Length );
            streamReader.Close();
            file.Close();
          }
          else
          {
            Content = "";
          }
        }

        internal void Save(string filePath)
        {
#if CLOUD
        using ( Stream fileStream = AzureUtil.BlobToStream( filePath ) )
#else
            using (FileStream fileStream = File.OpenWrite(filePath))
#endif
            {
                if (BinaryData != null)
                    fileStream.Write(BinaryData, 0, BinaryData.Length);
            }
        }
    }
}
