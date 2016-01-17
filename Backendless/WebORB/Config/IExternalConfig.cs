using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;

namespace Weborb.Config
{
    public interface IExternalConfig
    {
        Hashtable Configure(ORBConfig config, ArrayList sectionsToProcess);
        Stream GetConfigStream();
        void SaveConfig(XmlTextWriter writer);
        void FlushConfig();
    }
}
