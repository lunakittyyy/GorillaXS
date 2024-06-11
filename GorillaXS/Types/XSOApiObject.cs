using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaXS.Types
{
    /// <summary>
    /// API object that is serialized into JSON. This is what gets sent to XSOverlay via WebSockets and contains a command.
    /// </summary>
    internal class XSOApiObject
    {
        public string sender = null;
        public string target = null;
        public string command = null;
        public string jsonData = null;
        public string rawData = null;
    }
}
