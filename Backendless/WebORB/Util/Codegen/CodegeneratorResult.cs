using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Util.Codegen
{
    public class CodegeneratorResult
    {
        private CodeItem m_codeItem;
        private String m_downloadUri;
        private int _lineCount;
        private bool m_savedOnServer;
        private String m_info;

        public CodeItem Result
        {
            get { return m_codeItem; }
            set { m_codeItem = value; }
        }

        public bool SavedOnServer
        {
            get { return m_savedOnServer; }
            set { m_savedOnServer = value; }
        }

        public String DownloadUri
        {
            get { return m_downloadUri; }
            set { m_downloadUri = value; }
        }

        public String Info
        {
            get { return m_info; }
            set { m_info = value; }
        }

        public int LineCount
        {
            get { return _lineCount; }
            set { _lineCount = value; }
        }

        public CodegeneratorResult() { }
    }
}
